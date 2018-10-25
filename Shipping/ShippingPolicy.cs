using System;
using System.Threading.Tasks;
using Messages.Commands;
using Messages.Events;
using Rebus.Bus;
using Rebus.Sagas;

namespace Shipping
{
    public class ShippingPolicy : Saga<ShippingPolicyData>,
        IAmInitiatedBy<OrderPlaced>,
        IAmInitiatedBy<OrderBilled>
    {
        private readonly IBus _bus;

        public ShippingPolicy(IBus bus)
        {
            _bus = bus;
        }

        public Task Handle(OrderPlaced message)
        {
            Console.WriteLine($"Received OrderPlaced, OrderId = {message.OrderId}");
            Data.IsOrderPlaced = true;
            return ProcessOrder();
        }

        public Task Handle(OrderBilled message)
        {
            Console.WriteLine($"Received OrderBilled, OrderId = {message.OrderId}");
            Data.IsOrderBilled = true;
            return ProcessOrder();
        }

        private async Task ProcessOrder()
        {
            if (Data.IsOrderPlaced && Data.IsOrderBilled)
            {
                await _bus.SendLocal(new ShipOrder() { OrderId = Data.OrderId });
                MarkAsComplete();
            }
        }

        protected override void CorrelateMessages(ICorrelationConfig<ShippingPolicyData> config)
        {
            config.Correlate<OrderPlaced>(message => message.OrderId, sagaData => sagaData.OrderId);
            config.Correlate<OrderBilled>(message => message.OrderId, sagaData => sagaData.OrderId);
        }
    }
}