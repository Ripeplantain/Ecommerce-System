using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;

namespace Ecommerce.Common.MongoDB
{
    public static class MongoExtensions
    {

        public static IServiceCollection AddMongo(this IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("MongoDb");
                return new MongoClient(connectionString);
            });

            return services;
        }

        public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string databaseName ,string collectionName) where T : IEntity
        {
            services.AddSingleton<IRepository<T>>(serviceProvider => 
            {
                var mongoClient = serviceProvider.GetRequiredService<IMongoClient>();
                return new MongoRespository<T>(mongoClient, databaseName ,collectionName);
            });

            return services;
        }
    }
}