using Newtonsoft.Json;
using RabbitMQ.Client;
using Shared.Dtos;
using System.Text;

namespace RabbitMQ_Producer
{
    public static class vCommerceTest
    {
        public static string accountId => "07F70723-1B96-4B97-B891-7BF708594EEA";
        public static string agentId => "03D14CC4-2A5A-3CD3-F441-35C2BE32A4B9";
        public static string exchangeName => accountId;
        public static string queueName => "orders";
        public static string routingKey => "order.created";

        public static void Publish(IModel channel)
        {
            //var senderId = Guid.NewGuid().ToString();
            //senderId = senderId.Substring(0, senderId.IndexOf('-'));

            var args = new Dictionary<string, object>
            {
                //{ "x-message-ttl", 60000 }
            };

            channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Direct,
                arguments: args
            );

            channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            channel.QueueBind(queueName, exchangeName, routingKey);

            var order = new Order
            {
                Id = String.Empty,
                OrderNumber = String.Empty,
                OrderReference = "FSA12203SON00165",
                SalesSite = "FSA1",
                DeliveryCustomer = "ONT-C014",
                GroupCustomer = "ONT-C014",
                InvoiceCustomer = "ONT-C014",
                CustomerReference = "PUR172852",
                CurrencyCode = "ZAR",
                CurrencyRate = 1,
                GrossValue = 3171.47,
                NettValue = 3171.47,
                TaxValue = 3647.19 - 3171.47,
                VatNumber = "",
                DeliveryCarrier = "DSV",
                DeliveryMode = "DSV-6",
                DeliveryStatus = 1,
                InvoiceStatus = 1,
                PaymentStatus = 0,
                RequiredDate = new DateTime(2022, 3, 8),
                ShippingDate = new DateTime(2022, 3, 2),
                OrderDate = new DateTime(2022, 3, 2),
                BillingAddress = new Address()
                {
                    Id = String.Empty,
                    Company = "ON TAP - WEST COAST t/a",
                    Address1 = "PO BOX 815",
                    Address2 = "",
                    City = "VREDENBURG",
                    PostCode = "7380",
                    FirstName = "LOUIS",
                    LastName = "MULLER",
                    Email = "LOUIS@ONTAP.CO.ZA",
                    VatNumber = "",
                    TelephoneNumber = "227191999",
                    FaxNumber = "227151334",
                    MobileNumber = ""
                },
                ShipingAddress = new Address()
                {
                    Id = String.Empty,
                    Company = "ON TAP - WEST COAST t/a",
                    Address1 = "26 HEUNINGKLIPWEG",
                    Address2 = "VREDENBURG",
                    City = "VREDENBURG",
                    PostCode = "7380",
                    FirstName = "LOUIS",
                    LastName = "MULLER",
                    Email = "LOUIS@ONTAP.CO.ZA",
                    VatNumber = "",
                    TelephoneNumber = "227191999",
                    FaxNumber = "227151334",
                    MobileNumber = ""
                }
            };
            order.Items.Add(new OrderItem()
            {
                Id = String.Empty,
                LineNumber = "1000",
                Product = "1990095",
                Description = "SLX110-40 90mm WWK OF - BOXED",
                GrossPrice = 1386.00,
                NettPrice = 1032.57,
                Discount = 25.50,
                Quantity = 2,
                Allocated = 2,
                Shortage = 0,
                Remaining = 2,
                Type = "Kit Header",
            });
            order.Items.Add(new OrderItem()
            {
                Id = String.Empty,
                LineNumber = "2000",
                Product = "1220042",
                Description = "SLX110-40 90mm WWK OF - BOXED",
                GrossPrice = 0.00,
                NettPrice = 0.00,
                Discount = 0.00,
                Quantity = 2,
                Allocated = 0,
                Shortage = 0,
                Remaining = 2,
                Type = "Kit Component",
            });
            order.Items.Add(new OrderItem()
            {
                Id = String.Empty,
                LineNumber = "3000",
                Product = "1120141",
                Description = "90mm WASTE KIT - SELENE ",
                GrossPrice = 0.00,
                NettPrice = 0.00,
                Discount = 0.00,
                Quantity = 2,
                Allocated = 0,
                Shortage = 0,
                Remaining = 2,
                Type = "Kit Component",
            });
            order.Items.Add(new OrderItem()
            {
                Id = String.Empty,
                LineNumber = "4000",
                Product = "1150037",
                Description = "SATURN ARC SWIVEL SS MIXER",
                GrossPrice = 1485.00,
                NettPrice = 1106.33,
                Discount = 25.50,
                Quantity = 1,
                Allocated = 0,
                Shortage = 0,
                Remaining = 1,
                Type = "Standard",
            });

            var payload = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(order));

            Console.WriteLine($"Sending order {order.OrderReference}");

            var props = channel.CreateBasicProperties();
            props.Headers = new Dictionary<string, object>();
            props.Headers.Add("AccountId", accountId);
            props.Headers.Add("AgentId", agentId);
            props.Headers.Add("Reference", order.OrderReference);

            channel.BasicPublish(exchangeName, routingKey, props, payload);

        }
    }
}
