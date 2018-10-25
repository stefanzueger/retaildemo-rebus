using System;
using System.Threading.Tasks;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Logging;

namespace Sales
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "Sales";

            using (var activator = new BuiltinHandlerActivator())
            {
                activator.Register(() => new PlaceOrderHandler(activator.Bus));
                
                Configure.With(activator)
                    .Transport(t => t.UseRabbitMq("amqp://localhost", "RetailDemo.Rebus.Sales"))
                    .Logging(l => l.ColoredConsole(LogLevel.Info))
                    .Start();

                Console.WriteLine("Press enter to quit");
                Console.ReadLine();
            }
        }
    }
}
