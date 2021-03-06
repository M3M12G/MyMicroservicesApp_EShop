﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Discount_gRPC.Entites
{
    public class PromoCode
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Code { get; set; }
        //The title of promocode - event name that is invoked generation of this product
        public string Title { get; set; }
        //in order to track promo codes and avoid multi usage of promocodes, it would be good to keep username of users
        public string? Username { get; set; }
        [BsonDateTimeOptions]
        public DateTime ExpirationDate { get; set; }
        public double Discount { get; set; }
        public bool IsValid { get; set; } = true;
    }
}
