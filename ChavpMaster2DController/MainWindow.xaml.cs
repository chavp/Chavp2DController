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

namespace ChavpMaster2DController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BoxViewModel _BoxVM = null;
        IAdvancedBus _bus;
        Point _currentPoint;
        public MainWindow()
        {
            InitializeComponent();

            string rabbitMQBrokerHost = "localhost";
            string virtualHost = "chavp-games";
            string username = "guest";
            string password = "guest";

            string connectionString = string.Format(
                "host={0};virtualHost={1};username={2};password={3}",
                rabbitMQBrokerHost, virtualHost, username, password);

            _bus = RabbitHutch.CreateBus(connectionString).Advanced;

            _BoxVM = new BoxViewModel(_bus);

            _currentPoint = new Point
            {
                X = 10,
                Y = 10,
            };

            _BoxVM.X = _currentPoint.X;
            _BoxVM.Y = _currentPoint.Y;

            DataContext = _BoxVM;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = Mouse.GetPosition(Application.Current.MainWindow);

            _BoxVM.X = point.X;
            _BoxVM.Y = point.Y;

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
