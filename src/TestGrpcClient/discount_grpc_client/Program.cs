using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace discount_grpc_client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:3333");
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var promoClient = new PromoDiscountService.PromoDiscountServiceClient(channel);
            using var promoStream = promoClient.GetAllPromos(new AllPromoRequest { });

            while(await promoStream.ResponseStream.MoveNext())
            {
                var pc = promoStream.ResponseStream.Current;
                Console.WriteLine($"{pc.Id}-{pc.Title}-{pc.Code}-{pc.Username}-{pc.ExpirationDate}-{pc.Discount}-{pc.IsValid}");
            }
            Console.ReadKey();

        }

    }
}
