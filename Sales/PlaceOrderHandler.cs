using System;
using System.Threading.Tasks;
using Messages.Commands;
using Messages.Events;
using Rebus.Bus;
using Rebus.Handlers;

namespace Sales
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        private readonly IBus _bus;

        static Random random = new Random();

        public PlaceOrderHandler(IBus bus)
        {
            _bus = bus;
        }

        public Task Handle(PlaceOrder message)
        {
            Console.WriteLine($"Received PlaceOrder, OrderId = {message.OrderId}");

            if (random.Next(0, 5) == 0)
            {
                throw new Exception("Oops");
            }

            var orderPlaced = new OrderPlaced
            {
                OrderId = message.OrderId
            };
            return _bus.Publish(orderPlaced);
        }
    }
}