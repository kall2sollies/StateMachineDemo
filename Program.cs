using System;
using System.IO;
using Microsoft.Extensions.Configuration;
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
            // create service collection
            ServiceCollection serviceCollection = new();
            ConfigureServices(serviceCollection);

            // create service provider
            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            // run app
            serviceProvider.GetService<App>().Run();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // add logging
            serviceCollection.AddSingleton(LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            }));
            serviceCollection.AddLogging(); 

            // add services
            serviceCollection.AddTransient<IWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger>, ManagerValidationWorkflowProvider>();

            // add app
            serviceCollection.AddTransient<App>();
        }
    }
}
