﻿using GreenPipes;
using Ledger.Activations.Domain.IntegrationEventHandlers.ActivationAggregate;
using Ledger.Companies.Domain.IntegrationEventHandlers.CompanyAggregate;
using Ledger.CrossCutting.ServiceBus;
using Ledger.CrossCutting.ServiceBus.Abstractions;
using Ledger.CrossCutting.ServiceBus.BackgroundTasks;
using Ledger.HelpDesk.Domain.IntegrationEventHandlers.UserAggregate;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Ledger.CrossCutting.IoC
{
    public static class ServiceBusBootstrapper
    {
        public static void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            InitializeDomainBus(services);
            InitializeMassTransit(services, configuration);
            InitializeHostedService(services);
        }

        //"Local" Domain Service Bus
        private static void InitializeDomainBus(IServiceCollection services)
        {
            services.AddScoped<IDomainServiceBus, DomainServiceBus>();
        }

        //Bounded Context Integration Service Bus
        private static void InitializeMassTransit(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumer<CompanyIntegrationEventHandler>();
                cfg.AddConsumer<ActivationIntegrationEventHandler>();
                cfg.AddConsumer<UserIntegrationEventHandler>();
            });

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(transport =>
            {
                Uri hostAddress = new Uri(configuration["MassTransit:RabbitMqHost"]);

                IRabbitMqHost host = transport.Host(hostAddress, cfg =>
                {
                    cfg.Username("guest");
                    cfg.Password("guest");
                });
                
                transport.ReceiveEndpoint(host, "company_events", endpoint =>
                {
                    endpoint.LoadFrom(provider);
                    endpoint.UseRetry(r => r.Immediate(5));
                });
            }));

            services.AddSingleton<IIntegrationServiceBus, IntegrationServiceBus>();
        }

        private static void InitializeHostedService(IServiceCollection services)
        {
            services.AddHostedService<MassTransitProcess>();
        }
    }
}
