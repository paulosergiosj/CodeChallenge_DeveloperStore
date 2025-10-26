using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.UnitOfWork;
using Ambev.DeveloperEvaluation.Messaging.EventHandlers;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.ORM.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Rebus.Config;
using Rebus.Serialization.Json;
using Rebus.Transport.InMem;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        InitializePostgreSQLConfiguration(builder);
        InitializeMongoDBConfiguration(builder);
        AddRebusConfiguration(builder);
    }

    private static void InitializePostgreSQLConfiguration(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<DefaultContext>());
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void InitializeMongoDBConfiguration(WebApplicationBuilder builder)
    {
        var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDbConnection");

        builder.Services.AddSingleton<IMongoClient>(s =>
            new MongoClient(mongoConnectionString));

        builder.Services.AddScoped<IMongoDatabase>(s =>
        {
            var client = s.GetRequiredService<IMongoClient>();
            return client.GetDatabase(builder.Configuration["MongoDb:Database"] ?? throw new ArgumentException("Mongo Database name not provided."));
        });

        //builder.Services.AddScoped<IProductReviewRepository, ProductReviewRepository>();
    }

    private static void AddRebusConfiguration(WebApplicationBuilder builder)
    {
        AddRebusHandlers(builder);
        builder.Services.AddSingleton<InMemNetwork>();

        builder.Services.AddRebus(
                    (configure, provider) => configure
                       .Transport(t => t.UseInMemoryTransport(
                        provider.GetRequiredService<InMemNetwork>(),
                        "main_application_queue"))
                       .Serialization(s => s.UseNewtonsoftJson()));
    }

    private static void AddRebusHandlers(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<CartCheckoutEvent>();
    }
}