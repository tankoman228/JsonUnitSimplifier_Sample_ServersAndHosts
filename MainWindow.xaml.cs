using ServersAndHosts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServersAndHosts
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Service.ComponentService ComponentService;
        Service.ComponentTypeService ComponentTypeService;
        Service.HostService HostService;
        Service.ServerService ServerService;

        public MainWindow()
        {
            InitializeComponent();

            // Services
            ComponentService = new Service.ComponentService(new Repository<Entity.component>());
            ComponentTypeService = new Service.ComponentTypeService(new Repository<Entity.component_type>());
            HostService = new Service.HostService(new Repository<Entity.host>());
            ServerService = new Service.ServerService(new Repository<Entity.server>());

            LoadAsync();

            btnSaveComp.Click += BtnSaveComp_Click;
            btnDeleteComp.Click += BtnDeleteComp_Click;
        }

        private void LoadAsync()
        {
            TryAsyncOrShowError(() => {
                var t = ComponentTypeService.GetComponentTypes();
                var c = ComponentService.GetComponents();

                Dispatcher.Invoke(() => {
                    cbComponentType.ItemsSource = t;
                    lbComponents.ItemsSource = c;
                });
            });
        }

        /// <summary>
        /// Оборачивает в try catch и выполняет задачу в отдельном потоке
        /// </summary>
        private void TryAsyncOrShowError(Action action, string error = "")
        {
            Task.Run(() =>
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Alert.Error($"Error: {error}\n{ex.Message}");
                    });
                }
            });
        }


        private int? ParseOrNull(string text)
        {
            int res;
            if (int.TryParse(text, out res)) return res;
            return null;
        }


        #region ComponentsTab

        private void BtnDeleteComp_Click(object sender, RoutedEventArgs e)
        {
            var c = lbComponents.SelectedItem as string;

            if (Alert.AreYouSure("Do you really want to delete this component from database?")) {
                TryAsyncOrShowError(() => ComponentService.RemoveComponent(c));
            }    
        }

        private void BtnSaveComp_Click(object sender, RoutedEventArgs e)
        {
            if (cbComponentType.Text == "" || tbCompName.Text == "")
            {
                Alert.Warning("Empty field"); return;
            }
            TryAsyncOrShowError(() =>
            {
                string type = "", name = ""; int? cores = null, mem = null, mhz = null;
                Dispatcher.Invoke(() =>
                {
                    type = cbComponentType.Text;
                    name = tbCompName.Text;
                    cores = ParseOrNull(tbCompCores.Text);
                    mem = ParseOrNull(tbCompMemory.Text);
                    mhz = ParseOrNull(tbCompFreq.Text);
                });

                int id_type = ComponentTypeService.IdOrAddComponentTypeIfNotExists(type);
                ComponentService.AddComponent(new Entity.component
                {
                    name = name,
                    cores = cores,
                    memory = mem,
                    mhz = mhz,
                    id_component_type = id_type
                });
                Dispatcher.Invoke(() => LoadAsync());

            }, "Error while saving, maybe name is duplicated?");
        }
        #endregion
    }
}
