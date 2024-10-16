using Autofac;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using StateMachineDemo.Models;
using StateMachineDemo.Services;

namespace StateMachineDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ContainerBuilder builder = new();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(
                    path: "./Logs/StateMachineDemo_.log", 
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}][{Level:u3}] {Message:lj} {NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day)
                .WriteTo.Console(
                    theme: AnsiConsoleTheme.Code,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}][{Level:u3}] {Message:lj} {NewLine}{Exception}")
                .CreateLogger();

            builder
                .RegisterInstance(LoggerFactory.Create(x => x.AddSerilog()))
                .As<ILoggerFactory>();

            builder
                .RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();

            builder
                .RegisterType<TimeLogEntryWorkflowProviderFactory>()
                .SingleInstance();

            builder
                .RegisterType<TimeLogEntryManagerValidationWorkflowProvider>()
                .Keyed<IWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger>>(WorkflowProviderImplementationEnum.ManagerValidationWorkflowProvider)
                .InstancePerDependency();

            builder
                .RegisterType<TimeLogEntryProgressWithoutValidationWorkFlow>()
                .Keyed<IWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger>>(WorkflowProviderImplementationEnum.ProgressWithoutValidationWorkFlow)
                .InstancePerDependency();

            builder
                .RegisterType<EntryWithoutValidationWorkFlow>()
                .Keyed<IWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger>>(WorkflowProviderImplementationEnum.EntryWithoutValidationWorkFlow)
                .InstancePerDependency();

            builder
                .RegisterType<App>();

            builder.Build().Resolve<App>().Run();
        }
    }
}
