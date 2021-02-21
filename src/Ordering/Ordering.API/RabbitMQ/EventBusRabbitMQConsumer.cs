using AutoMapper;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Common;
using EventBusRabbitMQ.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Ordering.API.RabbitMQ
{
    public class EventBusRabbitMQConsumer
    {
        private readonly IRabbitMQConnection _connection;
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EventBusRabbitMQConsumer(IRabbitMQConnection connection, IMapper mapper, IServiceScopeFactory serviceScopeFactory)
        {
            _connection = connection;
            _mapper = mapper;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Consume()
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: EventBusConstants.BasketCheckoutQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += OnReceivedEvent;

            channel.BasicConsume(queue: EventBusConstants.BasketCheckoutQueue, autoAck: true, consumer: consumer);
        }
        //Task 5. -> EventBusRabbitMQConsumer:ReceivedEvent - completed
        private async void OnReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == EventBusConstants.BasketCheckoutQueue)
            {
                // encoding message taken from EventBus and deserialize it to basket checkout event object
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var basketCheckoutEvent = JsonConvert.DeserializeObject<BasketCheckoutEvent>(message);

                // mapping order basket checkout to order entity for storing to db
                var orderEntity = _mapper.Map<Order>(basketCheckoutEvent);
                if (orderEntity == null)
                    throw new ApplicationException("Entity could not be mapped.");

                // getting IOrderRepository using service scope factory and executing method of saving order to db
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var orderRepository = scope.ServiceProvider.GetService<IOrderRepository>();
                    await orderRepository.AddAsync(orderEntity);
                }
            }
        }

        public void Disconnect()
        {
            _connection.Dispose();
        }
    }
}
