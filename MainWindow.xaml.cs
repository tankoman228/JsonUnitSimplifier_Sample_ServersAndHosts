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
                ComponentService = new Service.ComponentService(new Repository<Entity.component>());
                ComponentTypeService = new Service.ComponentTypeService(new Repository<Entity.component_type>());
                HostService = new Service.HostService(new Repository<Entity.host>());
                ServerService = new Service.ServerService(new Repository<Entity.server>());
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
            btnSaveHosts.Click += BtnSaveHosts_Click;
            btnDeleteHost.Click += BtnDeleteHost_Click;
            btnCreateNewHost.Click += BtnCreateNewHost_Click;
            btnSaveAsNew.Click += BtnSaveAsNew_Click;
            cbServerComponentType.SelectionChanged += CbServerComponentType_SelectionChanged;
            
        }

        /// <summary>
        /// Обновляет все элементы в списках и таблицах
        /// </summary>
        private void LoadAsync()
        {
            TryAsyncOrShowError(() => {
                var t = ComponentTypeService.GetComponentTypes();
                Dispatcher.Invoke(() => {
                    cbComponentType.ItemsSource = t;
                    cbServerComponentType.ItemsSource = t;
                });
            });
            TryAsyncOrShowError(() => {
                var c = ComponentService.GetComponents();
                Dispatcher.Invoke(() => {
                    lbComponents.ItemsSource = c;
                });
            });
            TryAsyncOrShowError(() => {
                var s = ServerService.GetAllServers();
                Dispatcher.Invoke(() => {
                    dgServers.ItemsSource = s;
                    cbHostServer.ItemsSource = s;
                });
            });
            TryAsyncOrShowError(() => {
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

        Entity.server currentEditedServer = null;

        private void UpdateCurrentEditedServerInfoInUI_Lists()
        {
            lbServerInfo.Items.Clear();
            lbServerComponents.Items.Clear();
            foreach (var x in currentEditedServer.server_component)
            {
                lbServerComponents.Items.Add(x);
                try
                {
                    string about = $"{x.component.component_type.typename}: {x.component.name}\t";
                    if (x.component.memory != null)
                    {
                        about += $"size of: {x.component.memory}\t";
                    }
                    if (x.component.cores != null)
                    {
                        about += $"{x.component.cores} cores\t";
                    }
                    if (x.component.mhz != null)
                    {
                        about += $"Frequency is {x.component.mhz} MHz";
                    }
                    lbServerInfo.Items.Add(about);
                } catch (Exception ex) { Console.Error.WriteLine(ex.StackTrace); }
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
            tcServers.SelectedIndex = 0;

            Entity.server server;
            bool makeNew = false;

            if (currentEditedServer != null)
            {
                server = currentEditedServer;
            }
            else
            {
                server = new Entity.server
                {
                    address = tbServerAddress.Text,
                    name_in_network = tbServerName.Text,
                };
                makeNew = true;
            }

            int ram = 0;
            int disk = 0;
            int cores = 0;

            if (makeNew)
                server.server_component = new List<Entity.server_component>();
            else
                server.server_component.Clear();

            foreach (Entity.server_component c in lbServerComponents.Items)
            {
                try
                {
                    server.server_component.Add(c);
                    if (c.component.component_type.typename.ToLower().Equals("ram"))
                    {
                        ram += (int)c.component.memory;
                    }
                    else if (c.component.component_type.typename.ToLower().Equals("cpu"))
                    {
                        cores += (int)c.component.cores;
                    }
                    else if (c.component.component_type.typename.ToLower().Contains("hard"))
                    {
                        disk += (int)c.component.memory;
                    }
                    else if (c.component.component_type.typename.ToLower().Contains("ssd"))
                    {
                        disk += (int)c.component.memory;
                    }
                    c.component = null;
                }
                catch { c.component = null; }
            }

            server.ram_free_mb = ram ;
            server.ram_total_mb = ram;
            server.memory_free_kb = disk;
            server.memory_total_kb = cores;
            server.cores_free = cores;
            server.cores_total = cores;

            TryAsyncOrShowError(() =>
            {
                if (makeNew)
                {
                    ServerService.Insert(server);
                }
                else
                {
                    ServerService.Update(server);
                }
                LoadAsync();
            });

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
            {
                server_component.id_server = currentEditedServer.id;
            }
            else
            {
                server_component.id_server = -1;
            }

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
            throw new NotImplementedException();
        }

        private void BtnCreateNewHost_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnDeleteHost_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnSaveHosts_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        
        #endregion
    }
}
