using System;
using System.Threading.Tasks;
using Messages.Commands;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Logging;
using Rebus.Routing.TypeBased;

namespace ClientUI
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "ClientUI";

            using (var activator = new BuiltinHandlerActivator())
            {
                var bus = Configure.With(activator)
                    .Transport(t => t.UseRabbitMqAsOneWayClient("amqp://localhost"))
                    .Logging(l => l.ColoredConsole(LogLevel.Info))
                    .Routing(r => r.TypeBased().Map<PlaceOrder>("RetailDemo.Rebus.Sales"))
                    .Start();

                await RunLoop(bus)
                    .ConfigureAwait(false);
            }
        }

        static async Task RunLoop(IBus bus)
        {
            while (true)
            {
                Console.WriteLine("Press 'P' to place an order, or 'Q' to quit.");
                var key = Console.ReadKey();
                Console.WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.P:
                        // Instantiate the command
                        var command = new PlaceOrder
                        {
                            OrderId = Guid.NewGuid().ToString()
                        };

                        // Send the command to the local endpoint
                        Console.WriteLine($"Sending PlaceOrder command, OrderId = {command.OrderId}");
                        await bus.Send(command)
                            .ConfigureAwait(false);

                        break;

                    case ConsoleKey.Q:
                        return;

                    default:
                        Console.WriteLine("Unknown input. Please try again.");
                        break;
                }
            }
        }
    }
}
