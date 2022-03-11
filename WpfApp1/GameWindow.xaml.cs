using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для ConnectWindow.xaml
    /// </summary>
    public partial class GameWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<string> _fieldCollection;
        private int _serverPort;
        private bool isConnectionEstablished = false;
        private bool _isServer = false;
        private BitmapImage _userImage = new BitmapImage(new Uri("Images/user.png", UriKind.Relative));
        private bool _isFirstEnemyNameBoxClick=true, _isFirstPortTBClick=true;

        private Player serverPlayer = new Player();
        private Player clientPlayer = new Player();

        public GameWindow(bool isServer, string username)
        {
            InitializeComponent();
            DataContext = this;
            _fieldCollection = new ObservableCollection<string>();                
            SetInitialDataInFieldCollection();
            SetUserName(username);
            DefinePort();

            if (isServer)
            {
                ServerStart();
            }
            else
            {
                ClientStart();              
            }    
        }

        private void ServerStart()
        {
            _isServer = true;           
            serverPlayer.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress hostIp = GetHostIp();

            serverPlayer.Socket.Bind(new IPEndPoint(IPAddress.Any, _serverPort));
            serverPlayer.Socket.Listen(0);
            enemyName.Text = hostIp.ToString();
            portTB.Text = _serverPort.ToString();
            WaitConnectionAsync();        
        }

        private void DefinePort()
        {
            TcpListener tl = new TcpListener(IPAddress.Loopback, 0);
            tl.Start();
            _serverPort = ((IPEndPoint)tl.LocalEndpoint).Port;
            tl.Stop();
        }

        private void ClientStart()
        {
            _isServer = false;
            btnConnectToServer.Visibility = Visibility.Visible;
            enemyName.IsReadOnly = false;
            portTB.IsReadOnly = false;
            enemyName.Text = "Введите IP";
            portTB.Text = "Введите порт";
            enemyName.Foreground = Brushes.Gray;
            portTB.Foreground = Brushes.Gray;
        }

        public ObservableCollection<string> FieldCollection
        {
            get { return _fieldCollection; }
            set
            {
                _fieldCollection = value;
                OnPropertyChanged(nameof(FieldCollection)); 
            }
        }
        public string UserName
        {
            get { return serverPlayer.Name; }
            set
            {
                serverPlayer.Name = value;
                OnPropertyChanged(nameof(UserName));
            }
        }
        public string EnemyName
        {
            get { return clientPlayer.Name; }
            set
            {
                clientPlayer.Name = value;
                OnPropertyChanged(nameof(EnemyName));
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetInitialDataInFieldCollection()
        {
            for (int i = 0; i < 9; ++i)
                _fieldCollection.Add(string.Empty);
        }

        private void SetUserName(string username)
        {
            serverPlayer.Name = username;
            userNameTB.Text = serverPlayer.Name;
        }

        private IPAddress GetHostIp()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            return IPAddress.Parse("127.0.0.1");
        }

        private void Cell_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock? tb = sender as TextBlock;
            string name = tb.Name;
            int id = Int32.Parse(name.Replace("C", ""));
            if (FieldCollection[id] == string.Empty) {
                FieldCollection[id] = _isServer ? "X" : "O";
                SendDataAsync(id.ToString());
                CheckGame();
            }           
        }

        private async void WaitConnectionAsync()
        {           
            await Task.Run(() =>
            {
                clientPlayer.Socket= serverPlayer.Socket.Accept();               
            });


            if (clientPlayer.Socket != null)
            {      
                portTB.Visibility = Visibility.Hidden;
                enemyCountTextBlock.Visibility = Visibility.Visible;
                enemyImage.Source = _userImage;
                myCountTB.Visibility = Visibility.Visible;
                currentMoveInfo.Visibility = Visibility.Visible;

                ReceiveDataAsync();
                SendDataAsync($"name={UserName}");
                isConnectionEstablished = true;
                field.IsEnabled = true;

                DefineCurrentUsersMove();

                EstablishedConnectionWindow establishedConnectionWindow = new EstablishedConnectionWindow();
                establishedConnectionWindow.Owner = this;
                establishedConnectionWindow.ShowDialog();
            }
        }

        private async void ReceiveDataAsync()
        {
            while (true)
            {

                try
                {

                    string result = string.Empty;

                    result = await Task.Run(() =>
                    {
                        string receivedString = string.Empty;
                        while (true)
                        {
                            byte[] data = new byte[64];
                            int byteCount = clientPlayer.Socket.Receive(data);
                            receivedString += Encoding.UTF8.GetString(data, 0, byteCount);

                            if (receivedString.IndexOf("[FINAL]", StringComparison.Ordinal) > -1)
                                break;
                        }

                        return receivedString;
                    });

                    result = result.Replace("[FINAL]", "");

                    if (result.Contains("name="))
                    {
                        result = result.Replace("name=", "");
                        enemyName.Text = result;
                        clientPlayer.Name = result;
                        DefineCurrentUsersMove();
                    }
                    else
                    {
                        int id = Int32.Parse(result);
                        FieldCollection[id] = _isServer ? "O" : "X";

                        CheckGame();

                        field.IsEnabled = true;
                        DefineCurrentUsersMove();
                    }
                }
                catch (Exception)
                {
                    clientPlayer.Socket.Shutdown(SocketShutdown.Both);
                    clientPlayer.Socket.Close();
                    MessageBox.Show("Друг разорвал соединение! Спасибо, что воспользовались услугами нашего приложения! Если хотите сыграть с кем-нибудь ещё, пожалуйста, перезайдите в программу ещё раз!");
                    Environment.Exit(0);
                }


            }
        }

        private void DefineCurrentUsersMove()
        {
            if (field.IsEnabled)
            {
                currentMoveInfo.Text = "Ход игрока: " + serverPlayer.Name;
            }
            else
            {
                currentMoveInfo.Text = "Ход игрока: " + clientPlayer.Name;
            }
        }

        private async void SendDataAsync(string data)
        {
            if (isConnectionEstablished) { 
                field.IsEnabled = false;

                DefineCurrentUsersMove();
            }

            await Task.Run(() =>
            {
                byte[] byteData = Encoding.UTF8.GetBytes(data + "[FINAL]");
                clientPlayer.Socket.Send(byteData);
            });
        }

        private void CheckGame()
        {
            var f = _fieldCollection;
            bool isEmptyCells=true;
            foreach (var emptyCell in f)
            {
                if (emptyCell == String.Empty)
                {
                    isEmptyCells = true;
                    break;
                }
                else
                {
                    isEmptyCells = false;
                }
            }

            List<string> results = new List<string>();

            results.Add(f[0] + f[1] + f[2]);
            results.Add(f[3] + f[4] + f[5]);
            results.Add(f[6] + f[7] + f[8]);

            results.Add(f[0] + f[3] + f[6]);
            results.Add(f[1] + f[4] + f[7]);
            results.Add(f[2] + f[5] + f[8]);

            results.Add(f[0] + f[4] + f[8]);
            results.Add(f[2] + f[4] + f[6]);

            string winner = string.Empty;

            foreach (var r in results)
            {
                if (r == "XXX")
                {
                    winner = "X";
                    break;
                }

                else if (r == "OOO")
                {
                    winner = "O";
                    break;
                }
            }

            if (winner != string.Empty)
            {
                currentMoveInfo.Visibility = Visibility.Hidden;

                if (_isServer && winner == "X")
                {                  
                    RunWinWindow();
                }                    

                if (_isServer && winner == "O")
                {
                    RunLooseWindow();
                }

                if (!_isServer && winner == "O")
                {
                    RunWinWindow();
                }

                if (!_isServer && winner == "X")
                {
                    RunLooseWindow();                    
                }
            }
            else if (!isEmptyCells)
            {             
                RunNobodyWonWindow();
            }

        }

        private void btnConnectToServer_Click(object sender, RoutedEventArgs e)
        {
            clientPlayer.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                IPAddress serverIp = IPAddress.Parse(enemyName.Text);
                int serverPort = Int32.Parse(portTB.Text);

                clientPlayer.Socket.Connect(new IPEndPoint(serverIp, serverPort));

                SendDataAsync($"name={UserName}");
                field.IsEnabled = false;              
                ReceiveDataAsync();

                EstablishedConnectionWindow establishedConnectionWindow = new EstablishedConnectionWindow();
                establishedConnectionWindow.Owner = this;
                establishedConnectionWindow.ShowDialog();


                portTB.Visibility = Visibility.Hidden;
                btnConnectToServer.Visibility = Visibility.Hidden;
                enemyCountTextBlock.Visibility = Visibility.Visible;
                enemyImage.Visibility = Visibility.Visible;
                isConnectionEstablished = true;
                enemyImage.Source = _userImage;
                currentMoveInfo.Visibility = Visibility.Visible;
                enemyName.IsReadOnly = true;
                myCountTB.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Environment.Exit(0);
        }
        
        private void enemyName_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_isFirstEnemyNameBoxClick && !_isServer)
            {
                enemyName.Text = String.Empty;
                enemyName.Foreground = Brushes.Black;
                _isFirstEnemyNameBoxClick = false;
            }
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {           
            if (_isServer)
            {
                ServerInformWindow w = new ServerInformWindow();
                w.Owner = this;
                w.Show();
            }
        }
        
        private void portTB_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_isFirstPortTBClick && !_isServer)
            {
                portTB.Text = String.Empty;
                portTB.Foreground = Brushes.Black;
                _isFirstPortTBClick = false;
            }
        }

        private void RunWinWindow()
        {
            WinWindow winWindow = new WinWindow();
            winWindow.Owner = this;
            if (winWindow.ShowDialog() == true)
            {
                FieldCollection.Clear();
                _fieldCollection.Clear();
                currentMoveInfo.Visibility = Visibility.Visible;
                SetInitialDataInFieldCollection();
                serverPlayer.Count++;
                myCountTB.Text = serverPlayer.Count.ToString();
            }
            else
            {
                MessageBox.Show("Спасибо, что воспользовались услугами нашего приложения! До свидания!");
                Environment.Exit(0);
            }
        }

        private void RunLooseWindow()
        {
            LooseWindow looseWindow = new LooseWindow();
            looseWindow.Owner = this;
            if (looseWindow.ShowDialog() == true)
            {
                FieldCollection.Clear();
                _fieldCollection.Clear();           
                currentMoveInfo.Visibility = Visibility.Visible;
                SetInitialDataInFieldCollection();
                clientPlayer.Count++;
                enemyCountTextBlock.Text = clientPlayer.Count.ToString();
            }
            else
            {
                MessageBox.Show("Спасибо, что воспользовались услугами нашего приложения! До свидания!");
                Environment.Exit(0);
            }
        }

        private void RunNobodyWonWindow()
        {
            NobodyWonWindow nobodyWonWindow = new NobodyWonWindow();
            nobodyWonWindow.Owner = this;
            if (nobodyWonWindow.ShowDialog() == true)
            {
                FieldCollection.Clear();
                _fieldCollection.Clear();                
                currentMoveInfo.Visibility = Visibility.Visible;
                SetInitialDataInFieldCollection();
            }
            else
            {
                MessageBox.Show("Спасибо, что воспользовались услугами нашего приложения! До свидания!");
                Environment.Exit(0);
            }
        }
    }
}
