using Discount_gRPC.Entites;
using Discount_gRPC.Helpers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Discount_gRPC.Data
{
    public class DiscountContextSeed
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
                    Code = PromoGenerator.GenerateCode(),
                    Title = "EShop Opening",
                    ExpirationDate = DateTime.Now.AddDays(15),
                    Discount = 0.3
                },
                new PromoCode(){
                    Code = PromoGenerator.GenerateCode(),
                    Title = "EShop Opening",
                    ExpirationDate = DateTime.Now.AddDays(15),
                    Discount = 0.3
                },
                new PromoCode(){
                    Code = PromoGenerator.GenerateCode(),
                    Title = "EShop Opening",
                    ExpirationDate = DateTime.Now.AddDays(15),
                    Discount = 0.3
                },
                new PromoCode(){
                    Code = PromoGenerator.GenerateCode(),
                    Title = "EShop Opening",
                    ExpirationDate = DateTime.Now.AddDays(15),
                    Discount = 0.3
                },
                new PromoCode(){
                    Code = PromoGenerator.GenerateCode(),
                    Title = "EShop Opening",
                    ExpirationDate = DateTime.Now.AddDays(15),
                    Discount = 0.3
                },
                new PromoCode(){
                    Code = PromoGenerator.GenerateCode(),
                    Title = "EShop Opening",
                    ExpirationDate = DateTime.Now.AddDays(15),
                    Discount = 0.3
                },
                new PromoCode(){
                    Code = PromoGenerator.GenerateCode(),
                    Title = "EShop Opening",
                    ExpirationDate = DateTime.Now.AddDays(15),
                    Discount = 0.3
                },
                new PromoCode(){
                    Code = PromoGenerator.GenerateCode(),
                    Title = "EShop Opening",
                    ExpirationDate = DateTime.Now.AddDays(15),
                    Discount = 0.3
                },
                new PromoCode(){
                    Code = PromoGenerator.GenerateCode(),
                    Title = "EShop Opening",
                    ExpirationDate = DateTime.Now.AddDays(15),
                    Discount = 0.3
                },
                new PromoCode(){
                    Code = PromoGenerator.GenerateCode(),
                    Title = "EShop Opening",
                    ExpirationDate = DateTime.Now.AddDays(15),
                    Discount = 0.3
                }
            };
        }
    }
}
