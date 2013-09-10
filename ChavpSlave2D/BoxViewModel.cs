using Chavp.Worlds;
using EasyNetQ;
using EasyNetQ.Topology;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChavpSlave2D
{
    public class BoxViewModel : INotifyPropertyChanged
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

        public BoxViewModel(string name, IAdvancedBus bus)
        {
            Box = new Box
            {
                Name = name,
            };

            var exchange = bus.ExchangeDeclare("box", EasyNetQ.Topology.ExchangeType.Fanout);

            var queue = bus.QueueDeclare(
                "update." + Box.Name, 
                durable: false, 
                exclusive: true, 
                autoDelete: true);

            bus.Bind(exchange, queue, name);

            bus.Consume<Box>(queue, (body, info) => Task.Factory.StartNew(() =>
            {
                var box = body.Body;

                X = box.X;
                Y = box.Y;
            }));

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

        }
    }
}
