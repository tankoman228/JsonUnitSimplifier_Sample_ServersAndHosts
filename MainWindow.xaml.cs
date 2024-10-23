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

        private async void LoadAsync()
        {
            cbComponentType.ItemsSource = await ComponentTypeService.GetComponentTypes();
            lbComponents.ItemsSource = await ComponentService.GetComponents();
        }


        #region ComponentsTab

        private void BtnDeleteComp_Click(object sender, RoutedEventArgs e)
        {
            if (Alert.AreYouSure("Do you really want to delete this component from database?"))
                ComponentService.RemoveComponent(lbComponents.SelectedItem as string);
        }

        private void BtnSaveComp_Click(object sender, RoutedEventArgs e)
        {

            throw new NotImplementedException();
        }
        #endregion


    }
}
