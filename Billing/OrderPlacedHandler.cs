using System;
using System.Threading.Tasks;
using Messages.Events;
using Rebus.Bus;
using Rebus.Handlers;

namespace Billing
{
    public class OrderPlacedHandler : IHandleMessages<OrderPlaced>
    {
        private readonly IBus _bus;

        public OrderPlacedHandler(IBus bus)
        {
            _bus = bus;
        }

        public Task Handle(OrderPlaced message)
        {
            Console.WriteLine($"Received OrderPlaced, OrderId = {message.OrderId} - Charging credit card...");

            return _bus.Publish(new OrderBilled
            {
                OrderId = message.OrderId
            });
        }
    }
}