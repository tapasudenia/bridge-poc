using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Shared;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Samples.MessagingBridge.Bridge";

        var connectionString = Constants.AZURE_SB_CONNECTION_STRING;
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
        }

        await Host.CreateDefaultBuilder()
            .UseNServiceBusBridge((ctx, bridgeConfiguration) =>
            {
                #region create-asb-endpoint-of-bridge
                var asbBridgeEndpoint = new BridgeEndpoint(Constants.AZURE_SB_ENDPOINT_NAME);
                #endregion

                #region asb-subscribe-to-event-via-bridge
                asbBridgeEndpoint.RegisterPublisher<MyEvent>(Constants.RABBIT_MQ_ENDPOINT_NAME);
                #endregion

                #region asb-bridge-configuration
                var asbBridgeTransport = new BridgeTransport(new AzureServiceBusTransport(connectionString));
                asbBridgeTransport.AutoCreateQueues = true;
                asbBridgeTransport.HasEndpoint(asbBridgeEndpoint);
                bridgeConfiguration.AddTransport(asbBridgeTransport);
                #endregion

                #region create-rabbit-endpoint-of-bridge
                var rabbitBridgeEndpoint = new BridgeEndpoint(Constants.RABBIT_MQ_ENDPOINT_NAME);
          
                #endregion

                #region rabbit-subscribe-to-event-via-bridge
                rabbitBridgeEndpoint.RegisterPublisher<OtherEvent>(Constants.AZURE_SB_ENDPOINT_NAME);
                #endregion

                #region rabbit-queue-configuration
                var rabbitMQEndpoint = new EndpointConfiguration(Constants.RABBIT_MQ_ENDPOINT_NAME);
                var rabbitTransport = rabbitMQEndpoint.UseTransport<RabbitMQTransport>();
                rabbitTransport.ConnectionString(Constants.Rabbit_MQ_CONNECTION_STRING);
                rabbitTransport.UseConventionalRoutingTopology(QueueType.Quorum);
                #endregion

                #region rabbit-bridge-configuration
                var rabbitBridgeTransport = new BridgeTransport(rabbitTransport.Transport);
                rabbitBridgeTransport.AutoCreateQueues = true;
                rabbitBridgeTransport.HasEndpoint(rabbitBridgeEndpoint);
                bridgeConfiguration.AddTransport(rabbitBridgeTransport);
                #endregion
            })
            .Build()
            .RunAsync().ConfigureAwait(false);
    }
}
