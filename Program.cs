using System;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StateMachineDemo.Models;
using StateMachineDemo.Services;

namespace StateMachineDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ContainerBuilder builder = new();

            builder.RegisterInstance(BuildLoggerFactory()).As<ILoggerFactory>();

            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();

            builder.RegisterType<TimeLogEntryStateService>().As<ITimeLogEntryStateService>().InstancePerDependency();
            builder.RegisterType<EntryWithoutValidationWorkFlow>().As<IWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger>>().InstancePerDependency();
            builder.RegisterType<App>().SingleInstance();

            //builder.RegisterType<RegiondoToApidaeConverter>().Keyed<IBaseApidaeConverter>(ApidaeConvertersEnum.RegiondoToApidae);

            IContainer container = builder.Build();

            container.Resolve<App>().Run();
        }

        private static ILoggerFactory BuildLoggerFactory()
        {
            return LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });
        }
    }
}
