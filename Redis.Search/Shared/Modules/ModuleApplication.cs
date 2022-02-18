using Autofac;
using MediatR;
using Microsoft.Extensions.Options;
using Redis.Search.Domain.Configuration;
using Redis.Search.Shared.Domain.Enums;
using StackExchange.Redis;
using System.Reflection;

namespace Redis.Search.Shared.Modules
{
    public class ModuleApplication : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Enumeration.LoadValue<FundType>();
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            _ = builder.Register(container =>
            {
                var options = container.Resolve<IOptions<ConnectionStringsOptions>>();
                return ConnectionMultiplexer.Connect(options.Value.ConnectionStringRedis).GetDatabase(0);

            }).As<IDatabase>().SingleInstance();
        }
    }
}
