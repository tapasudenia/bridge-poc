// Copyright 2022 Motorola Solutions, Inc. All Rights Reserved

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQEndpoint
{
    public class MyCommndHandler : IHandleMessages<MyCommand>
    {
        public Task Handle(MyCommand message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Recceived {message.Property} at rabbitmq endpoint");
            return context.Publish<MyEvent>(ev => ev.Property = $"{message.Property}-MyEvent from rabbit endpoint");
        }
    }
}
