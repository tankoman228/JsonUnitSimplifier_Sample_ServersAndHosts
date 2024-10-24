using ServersAndHosts.Entity;
using ServersAndHosts.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        Service.IComponentService ComponentService;
        Service.IComponentTypeService ComponentTypeService;
        Service.IHostService HostService;
        Service.IServerService ServerService;


        public MainWindow()
        {
            InitMainWindow();
        }
        public MainWindow(int THIS_CONSTRUCTOR_MAKES_MOCKS_OF_SERVICES)
        {
            InitMainWindow(1);
        }

        private void InitMainWindow(int THIS_CONSTRUCTOR_MAKES_MOCKS_OF_SERVICES = 0)
        {
            InitializeComponent();

            // Services
            if (THIS_CONSTRUCTOR_MAKES_MOCKS_OF_SERVICES == 0)
            {
                ComponentService = new Service.ComponentService(new RepositoryUniversal<Entity.component>());
                ComponentTypeService = new Service.ComponentTypeService(new RepositoryUniversal<Entity.component_type>());
                HostService = new Service.HostService(new RepositoryHost());
                ServerService = new Service.ServerService(new RepositoryServer());
            }
            else
            {
                // mocks

            }

            LoadAsync();

            // Components tab
            btnSaveComp.Click += BtnSaveComp_Click;
            btnDeleteComp.Click += BtnDeleteComp_Click;
            
            // Servers tab
            btnAddComponent.Click += BtnAddComponent_Click;
            btnServerRemoveSelected.Click += BtnServerRemoveSelected_Click;
            btnSaveServer.Click += BtnSaveServer_Click;
            btnEditServer.Click += BtnEditServer_Click;
            btnDeleteServer.Click += BtnDeleteServer_Click;
            btnCreateNewServer.Click += BtnCreateNewServer_Click;

            // Hosts tab
            btnDeleteHost.Click += BtnDeleteHost_Click;
            btnCreateNewHost.Click += BtnCreateNewHost_Click;
            btnSaveAsNew.Click += BtnSaveAsNew_Click;
            cbServerComponentType.SelectionChanged += CbServerComponentType_SelectionChanged;
            dgHosts.CellEditEnding += DgHosts_CellEditEnding;
        }



        /// <summary>
        /// Обновляет все элементы в списках и таблицах
        /// </summary>
        private void LoadAsync()
        {
            TryAsyncOrShowError(() => { // Component types
                var t = ComponentTypeService.GetComponentTypes();
                Dispatcher.Invoke(() => {
                    cbComponentType.ItemsSource = t;
                    cbServerComponentType.ItemsSource = t;
                });
            });
            TryAsyncOrShowError(() => { // Components
                var c = ComponentService.GetComponents();
                Dispatcher.Invoke(() => {
                    lbComponents.ItemsSource = c;
                });
            });
            TryAsyncOrShowError(() => { // Servers
                var s = ServerService.GetAllServers();
                Dispatcher.Invoke(() => {
                    dgServers.ItemsSource = s;
                    cbHostServer.ItemsSource = s;

                    dgcbServer.ItemsSource = s;
                });
            });
            TryAsyncOrShowError(() => { // Hosts
                var h = HostService.GetAllHosts();
                Dispatcher.Invoke(() => {
                    dgHosts.ItemsSource = h;
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
                        Alert.Error($"Error: {error}\n{ex.Message}\n{ex.InnerException?.Message}");
                    });
                    throw ex;
                }
            });
        }

        private int? ParseOrNull(string text)
        {
            int res;
            if (int.TryParse(text, out res)) return res;
            return null;
        }


        #region Components Tab

        private void BtnDeleteComp_Click(object sender, RoutedEventArgs e)
        {
            var c = lbComponents.SelectedItem as string;

            if (Alert.AreYouSure("Do you really want to delete this component from database?")) {
                TryAsyncOrShowError(() => { 
                    ComponentService.RemoveComponent(c);
                    Dispatcher.Invoke(() => LoadAsync());
                });

            }    
        }

        private void BtnSaveComp_Click(object sender, RoutedEventArgs e)
        {
            if (cbComponentType.Text == "" || tbCompName.Text == "")
            {
                Alert.Warning("Empty field"); return;
            }

            string type = cbComponentType.Text;
            string name = tbCompName.Text;
            var component = new Entity.component
            {
                name = name,
                id_component_type = ComponentTypeService.IdOrAddComponentTypeIfNotExists(type)
            };
            component.cores = ParseOrNull(tbCompCores.Text);
            component.memory = ParseOrNull(tbCompMemory.Text);
            component.mhz = ParseOrNull(tbCompFreq.Text);

            TryAsyncOrShowError(() =>
            {
                ComponentService.AddComponent(component);           
                LoadAsync();
            }, "Error while saving, maybe name is duplicated?");
        }
        #endregion

        #region Servers Tab

        // Null - creating new, else - editing already existing
        Entity.server currentEditedServer = null;

        private void UpdateCurrentEditedServerInfoInUI_Lists()
        {
            lbServerInfo.ItemsSource = ServerService.GetAbout(currentEditedServer);
            lbServerComponents.Items.Clear();
            foreach (var x in currentEditedServer.server_component)
            {
                lbServerComponents.Items.Add(x);
            }
        }

        private void BtnCreateNewServer_Click(object sender, RoutedEventArgs e)
        {
            currentEditedServer = null;
            tcServers.SelectedIndex = 1;

            tbServerAddress.Text = "";
            tbServerName.Text = "";
        }

        private void BtnDeleteServer_Click(object sender, RoutedEventArgs e)
        {
            if (dgServers.SelectedItem == null) return;
            var item = dgServers.SelectedItem as Entity.server;

            if (!Alert.AreYouSure("Delete?")) return;

            TryAsyncOrShowError(() => {
                ServerService.Delete(item);
                LoadAsync();
            });
        }

        private void BtnEditServer_Click(object sender, RoutedEventArgs e)
        {
            currentEditedServer = dgServers.SelectedItem as Entity.server;
            tbServerAddress.Text = currentEditedServer.address;
            tbServerName.Text = currentEditedServer.name_in_network;

            UpdateCurrentEditedServerInfoInUI_Lists();
            tcServers.SelectedIndex = 1;
        }

        private void BtnSaveServer_Click(object sender, RoutedEventArgs e)
        {
            Entity.server server;

            if (currentEditedServer != null)
            {
                server = currentEditedServer;
                server.address = tbServerAddress.Text;
                server.name_in_network = tbServerName.Text;

                var server_component = new List<Entity.server_component>();
                foreach (Entity.server_component c in lbServerComponents.Items)
                {
                    server_component.Add(c);
                }
                TryAsyncOrShowError(() =>
                {
                    ServerService.Update(server, server_component); 
                    LoadAsync();
                    Dispatcher.Invoke(() => tcServers.SelectedIndex = 0);
                });
            }
            else
            {
                var address = tbServerAddress.Text;
                var name_in_network = tbServerName.Text;
                var server_component = new List<Entity.server_component>();

                foreach (Entity.server_component c in lbServerComponents.Items) {
                    server_component.Add(c); 
                }

                TryAsyncOrShowError(() =>
                {                   
                    ServerService.Create(address, name_in_network, server_component);
                    LoadAsync();
                    Dispatcher.Invoke(() => tcServers.SelectedIndex = 0);
                });
            }
        }

        private void BtnServerRemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            if (lbServerComponents.SelectedItem == null) return;
            lbServerComponents.Items.Remove(lbServerComponents.SelectedItem);
        }

        private void BtnAddComponent_Click(object sender, RoutedEventArgs e)
        {
            var server_component = new Entity.server_component();

            if (currentEditedServer != null)
                server_component.id_server = currentEditedServer.id;
            else
                server_component.id_server = -1;

            var c = cbServerComponent.Text;
            TryAsyncOrShowError(() =>
            {
                var component = ComponentService.SearchComponent(c);
                server_component.id_component = component.id;
                server_component.component = component;

                Dispatcher.Invoke(() => lbServerComponents.Items.Add(server_component));
            });
        }

        private void CbServerComponentType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = cbServerComponentType.SelectedItem as Entity.component_type;
            if (selectedItem == null) return;

            TryAsyncOrShowError(() =>
            {
                var components = ComponentService.GetComponents().Where(x => x.Contains(selectedItem.typename));
                Dispatcher.Invoke(() =>
                {
                    cbServerComponent.ItemsSource = components;
                });
            });
        }

        #endregion

        #region Hosts Tab

        private void BtnSaveAsNew_Click(object sender, RoutedEventArgs e)
        {
            int ram, cpu, memory;
            string hostname, hostaddr, comment;
            Entity.server server = cbHostServer.SelectedItem as Entity.server;

            hostname = tbHostHostname.Text;
            hostaddr = tbHostAddress.Text;
            comment = tbHostComment.Text;

            if (server == null)
            {
                Alert.Warning("The server wasn't selected"); return;
            }
            if (!int.TryParse(tbHostRam.Text, out ram) || ram < 0)
            {
                Alert.Warning("Not valid RAM"); return;
            }
            if (!int.TryParse(tbHostCPU.Text, out cpu) || cpu < 0)
            {
                Alert.Warning("Not valid CPU cores number"); return;
            }
            if (!int.TryParse(tbHostMemory.Text, out memory) || memory <= 0)
            {
                Alert.Warning("Not valid memory use limit"); return;
            }

            var newHost = new Entity.host
            {
                comment = comment,
                cpu_cores = cpu,
                hostname = hostname,
                id_server = server.id,
                host_addr = hostaddr,
                memory_kb_limit = memory,
                memory_kb_took = 0,
                ram_mb = ram
            };

            if (!ServerService.MayHost(server, newHost))
            {
                Alert.Warning("This server has not enough resources"); return;
            }

            TryAsyncOrShowError(() =>
            {
                HostService.Insert(newHost);

                Dispatcher.Invoke(() => {

                    tbHostAddress.Text = "";
                    tbHostMemory.Text = "";
                    tbHostRam.Text = "";
                    tbHostCPU.Text = "";
                    tbHostComment.Text = "";

                    tcSubHosts.SelectedIndex = 0;
                    LoadAsync();
                });
            });
        }

        private void BtnCreateNewHost_Click(object sender, RoutedEventArgs e)
        {
            tbHostAddress.Text = "";
            tbHostMemory.Text = "";
            tbHostRam.Text = "";
            tbHostCPU.Text = "";
            tbHostComment.Text = "";

            tcSubHosts.SelectedIndex = 1;
        }

        private void BtnDeleteHost_Click(object sender, RoutedEventArgs e)
        {
            var host = dgHosts.SelectedItem as Entity.host;
            if (host == null) return;
            if (!Alert.AreYouSure("Delete?")) return;

            TryAsyncOrShowError(() =>
            {
                HostService.Delete(host);
                LoadAsync();
            });
        }

        object lck = new object();

        private void DgHosts_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var host = dgHosts.CurrentItem as host;
            if (host == null) return;

            // Получаем новое значение из выпадающего списка
            if (e.Column is DataGridComboBoxColumn comboBoxColumn)
            {
                var comboBox = e.EditingElement as ComboBox;
                if (comboBox != null)
                {
                    // Получение выбранного значения
                    server selectedValue = comboBox.SelectedValue as server;
                    if (selectedValue != null)
                    {
                        host.server = null;
                        host.id_server = selectedValue.id;
                        //MessageBox.Show("UPD to " + selectedValue.name_in_network);
                    }
                }
            }  

            TryAsyncOrShowError(() =>
            {
                lock (lck)
                {
                    HostService.Update(host);
                    LoadAsync();
                }
            });
        }

        #endregion

    }
}
