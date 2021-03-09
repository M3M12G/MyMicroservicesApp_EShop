using MongoDB.Driver;
using Promocode.API.Entities;
using Promocode.API.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Promocode.API.Data
{
    public class PromocodeContextSeed
    {
        public static void SeedData(IMongoCollection<PromoCode> promocodes)
        {
            bool existPromocodes = promocodes.Find(pc => true).Any();
            if (!existPromocodes)
            {
                promocodes.InsertManyAsync(GetOpeningPromocodesAsync());
            }
        }

        private static IEnumerable<PromoCode> GetOpeningPromocodesAsync()
        {
            return new List<PromoCode>() {
                new PromoCode(){
                    Code = PromocodeGenerator.GenerateCode(),
                    Title = "EShop Opening",
                    Target = "all",
                    ExpirationDate = DateTime.Now.AddDays(15),
                    Discount = 0.3
                },
                new PromoCode(){
                    Code = PromocodeGenerator.GenerateCode(),
                    Title = "EShop Opening",
                    Target = "all",
                    ExpirationDate = DateTime.Now.AddDays(15),
                    Discount = 0.3
                },
                new PromoCode(){
                    Code = PromocodeGenerator.GenerateCode(),
                    Title = "EShop Opening",
                    Target = "all",
                    ExpirationDate = DateTime.Now.AddDays(15),
                    Discount = 0.3
                },
                new PromoCode(){
                    Code = PromocodeGenerator.GenerateCode(),
                    Title = "EShop Opening",
                    Target = "all",
                    ExpirationDate = DateTime.Now.AddDays(15),
                    Discount = 0.3
                },
                new PromoCode(){
                    Code = PromocodeGenerator.GenerateCode(),
                    Title = "EShop Opening",
                    Target = "all",
                    ExpirationDate = DateTime.Now.AddDays(15),
                    Discount = 0.3
                },
                new PromoCode(){
                    Code = PromocodeGenerator.GenerateCode(),
                    Title = "EShop Opening",
                    Target = "all",
                    ExpirationDate = DateTime.Now.AddDays(15),
                    Discount = 0.3
                },
                new PromoCode(){
                    Code = PromocodeGenerator.GenerateCode(),
                    Title = "EShop Opening",
                    Target = "all",
                    ExpirationDate = DateTime.Now.AddDays(15),
                    Discount = 0.3
                },
                new PromoCode(){
                    Code = PromocodeGenerator.GenerateCode(),
                    Title = "EShop Opening",
                    Target = "all",
                    ExpirationDate = DateTime.Now.AddDays(15),
                    Discount = 0.3
                },
                new PromoCode(){
                    Code = PromocodeGenerator.GenerateCode(),
                    Title = "EShop Opening",
                    Target = "all",
                    ExpirationDate = DateTime.Now.AddDays(15),
                    Discount = 0.3
                },
                new PromoCode(){
                    Code = PromocodeGenerator.GenerateCode(),
                    Title = "EShop Opening",
                    Target = "all",
                    ExpirationDate = DateTime.Now.AddDays(15),
                    Discount = 0.3
                }
            };
        }
    }
}
