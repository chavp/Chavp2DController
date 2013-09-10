using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ChavpMaster2DController
{
    using Chavp.Worlds;
    using EasyNetQ;
    using EasyNetQ.Topology;
    using RabbitMQ.Client;

    public class BoxViewModel
        : INotifyPropertyChanged
    {

        public Box Box { get; set; }

        public string Name 
        {
            get
            {
                return Box.Name;
            }
            set
            {
                Box.Name = value;
                RaisePropertyChanged("Name");
            }
        }

        public double X
        {
            get
            {
                return Box.X;
            }
            set
            {
                Box.X = value;
                RaisePropertyChanged("X");
            }
        }

        public double Y
        {
            get
            {
                return Box.Y;
            }
            set
            {
                Box.Y = value;
                RaisePropertyChanged("Y");
            }
        }

        IAdvancedBus _bus;
        IAdvancedPublishChannel _boxPublishChannel;

        IExchange _exchange;
 
        public BoxViewModel(IAdvancedBus bus)
        {
            Box = new Box
            {
                Name = "player-1",
            };

            _bus = bus;
 
            // create a direct exchange
            _exchange = _bus.ExchangeDeclare("box", EasyNetQ.Topology.ExchangeType.Fanout);
            //_queue = _bus.QueueDeclare("update." + Box.Name, durable: true, exclusive: false, autoDelete: false);

            //_bus.Bind(_exchange, _queue, "update");

            _boxPublishChannel = _bus.OpenPublishChannel();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }

            // Update Property
            //_boxPublishChannel.Publish<Box>(
            var message = new Message<Box>(Box);

            _boxPublishChannel.Publish<Box>(_exchange, Box.Name, message);
        }

    }
}
