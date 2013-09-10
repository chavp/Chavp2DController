using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChavpSlave2D
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IAdvancedBus _bus;
        BoxViewModel _BoxVM = null;

        public MainWindow()
        {
            InitializeComponent();

            string rabbitMQBrokerHost = "localhost";
            string virtualHost = "chavp-games";
            string username = "player-1";
            string password = "123456789";

            string connectionString = string.Format(
                "host={0};virtualHost={1};username={2};password={3}",
                rabbitMQBrokerHost, virtualHost, username, password);

            _bus = RabbitHutch.CreateBus(connectionString).Advanced;

            _BoxVM = new BoxViewModel(username, _bus)
            {
                Name = username,
            };

            _BoxVM.X = 10;
            _BoxVM.Y = 10;

            DataContext = _BoxVM;
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_bus != null)
            {
                _bus.Dispose();
            }
            base.OnClosed(e);
        }
    }
}
