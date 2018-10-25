using System;
using System.Threading.Tasks;
using Messages.Events;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;
using Rebus.Routing.TypeBased;

namespace Billing
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "Billing";

            using (var activator = new BuiltinHandlerActivator())
            {
                activator.Register(() => new OrderPlacedHandler(activator.Bus));

                var bus = Configure.With(activator)
                    .Transport(t => t.UseRabbitMq("amqp://localhost", "RetailDemo.Rebus.Billing"))
                    .Routing(r => r.TypeBased().Map<OrderPlaced>("RetailDemo.Rebus.Sales"))
                    .Logging(l => l.ColoredConsole(LogLevel.Info))
                    .Start();

                await bus.Subscribe<OrderPlaced>();

                Console.WriteLine("Press enter to quit");
                Console.ReadLine();
            }
        }
    }
}
