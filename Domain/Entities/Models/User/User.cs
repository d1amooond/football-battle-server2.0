using AspNetCore.Identity.MongoDbCore.Models;
using Domain.Entities;
using MongoDB.Bson;
using System;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
