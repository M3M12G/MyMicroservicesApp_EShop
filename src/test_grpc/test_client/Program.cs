using Grpc.Net.Client;
using System;
using System.Threading.Tasks;
using test_grpc;

namespace test_client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:3333");
            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var client = new PromoDiscountService.PromoDiscountServiceClient(channel);

            var activate_promo_res = await client.ActivatePromoCodeAsync(new ActivationPromoRequest { Username = "test", Code = "FLFoVGDY" });
            Console.WriteLine(activate_promo_res.ToString());
            Console.ReadKey();
        }
    }
}
