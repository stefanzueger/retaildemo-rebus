using System;
using System.Threading.Tasks;
using Messages.Commands;
using Rebus.Handlers;

namespace Shipping
{
    class ShipOrderHandler : IHandleMessages<ShipOrder>
    {
        public Task Handle(ShipOrder message)
        {
            Console.WriteLine($"Order [{message.OrderId}] - Succesfully shipped.");
            return Task.CompletedTask;
        }
    }
}