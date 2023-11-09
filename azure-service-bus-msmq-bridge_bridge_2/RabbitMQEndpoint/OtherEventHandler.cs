// Copyright 2022 Motorola Solutions, Inc. All Rights Reserved

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQEndpoint
{
    public class OtherEventHandler : IHandleMessages<OtherEvent>
    {
        public Task Handle(OtherEvent message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Recceived OtherEvent {message.Property} at rabbitmq endpoint");
            return Task.CompletedTask;
        }
    }
}
