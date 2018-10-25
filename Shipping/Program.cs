using System;
using System.Threading.Tasks;
using Messages.Events;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;
using Rebus.Persistence.InMem;
using Rebus.Routing.TypeBased;

namespace Shipping
{
    class Program
    {
        private const string AzureServiceBusConnectionString =
            "Endpoint=sb://tbd";

        static async Task Main()
        {
            Console.Title = "Shipping";

            using (var activator = new BuiltinHandlerActivator())
            {
                activator.Register(() => new ShipOrderHandler());
                activator.Register(() => new ShippingPolicy(activator.Bus));
                
                var bus = Configure.With(activator)
                    //.Transport(t => t.UseRabbitMq("amqp://localhost", "RetailDemo.Rebus.Shipping"))
                    .Transport(t => t.UseAzureServiceBus(AzureServiceBusConnectionString, "RetailDemo.Rebus.Shipping"))
                    .Routing(r =>
                    {
                        r.TypeBased()
                            .Map<OrderPlaced>("RetailDemo.Rebus.Sales")
                            .Map<OrderBilled>("RetailDemo.Rebus.Sales");
                    })
                    .Logging(l => l.ColoredConsole(LogLevel.Info))
                    .Sagas(c => c.StoreInMemory())
                    .Start();

                await bus.Subscribe<OrderPlaced>();
                await bus.Subscribe<OrderBilled>();

                Console.WriteLine("Press enter to quit");
                Console.ReadLine();
            }
        }
    }
}
