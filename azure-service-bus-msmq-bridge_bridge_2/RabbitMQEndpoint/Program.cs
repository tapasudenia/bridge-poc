using System;
using System.Threading.Tasks;
using NServiceBus;
using Shared;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = Constants.RABBIT_MQ_ENDPOINT_NAME;

        var endpointConfiguration = new EndpointConfiguration(Constants.RABBIT_MQ_ENDPOINT_NAME);
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UsePersistence<NonDurablePersistence>();
        

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionString(Constants.Rabbit_MQ_CONNECTION_STRING);
        transport.UseConventionalRoutingTopology(QueueType.Quorum);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
