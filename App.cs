using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StateMachineDemo.Models;
using StateMachineDemo.Services;

namespace StateMachineDemo;

public class App
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<App> _logger;

    public App(
        IServiceProvider serviceProvider,
        ILogger<App> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public void Run()
    {
        using (var stateService = Resolve<ITimeLogEntryStateService>())
        {
            TimeLogEntryViewModel canceledEntry = new();
            stateService.Attach(canceledEntry);
            _logger.LogInformation($"\n---------------------------------------\n{nameof(canceledEntry)}, InitialState={canceledEntry.State}\n---------------------------------------\n");
            stateService.Fire(TimeLogEntryTrigger.Cancel);
            _logger.LogInformation(canceledEntry.ToString());
        }

        using (var stateService = Resolve<ITimeLogEntryStateService>())
        {
            TimeLogEntryViewModel entry = new();
            stateService.Attach(entry);
            _logger.LogInformation($"\n---------------------------------------\n{nameof(entry)}, InitialState={entry.State}\n---------------------------------------\n");
            stateService.Fire(TimeLogEntryTrigger.Update);
            stateService.Fire(TimeLogEntryTrigger.Complete);
            stateService.Fire(TimeLogEntryTrigger.Update);
            stateService.Fire(TimeLogEntryTrigger.SubmitToManager);
            stateService.Fire(TimeLogEntryTrigger.ManagerDeclines);
            stateService.Fire(TimeLogEntryTrigger.Update);
            stateService.Fire(TimeLogEntryTrigger.SubmitToManager);
            stateService.Fire(TimeLogEntryTrigger.ManagerValidates);
            _logger.LogInformation(entry.ToString());
        }


        using (var stateService = Resolve<ITimeLogEntryStateService>())
        {
            TimeLogEntryViewModel entryWithInitialState = new(TimeLogEntryState.InProgress);
            stateService.Attach(entryWithInitialState);
            _logger.LogInformation($"\n---------------------------------------\n{nameof(entryWithInitialState)}, InitialState={entryWithInitialState.State}\n---------------------------------------\n");
            stateService.Fire(TimeLogEntryTrigger.Update);
            stateService.Fire(TimeLogEntryTrigger.Complete);
            stateService.Fire(TimeLogEntryTrigger.SubmitToManager);
            stateService.Fire(TimeLogEntryTrigger.ManagerDeclines);
            stateService.Fire(TimeLogEntryTrigger.Update);
            stateService.Fire(TimeLogEntryTrigger.SubmitToManager);
            stateService.Fire(TimeLogEntryTrigger.ManagerValidates);
            _logger.LogInformation(entryWithInitialState.ToString());
        }


        using (var stateService = Resolve<ITimeLogEntryStateService>())
        {
            TimeLogEntryViewModel entryWithUndefinedState = new(TimeLogEntryState.Undefined);
            stateService.Attach(entryWithUndefinedState);
            _logger.LogInformation($"\n---------------------------------------\n{nameof(entryWithUndefinedState)}, InitialState={entryWithUndefinedState.State}\n---------------------------------------\n");
            stateService.Fire(TimeLogEntryTrigger.Update);
            stateService.Fire(TimeLogEntryTrigger.Complete);
            stateService.Fire(TimeLogEntryTrigger.Update);
            stateService.Fire(TimeLogEntryTrigger.SubmitToManager);
            stateService.Fire(TimeLogEntryTrigger.ManagerDeclines);
            stateService.Fire(TimeLogEntryTrigger.Update);
            stateService.Fire(TimeLogEntryTrigger.SubmitToManager);
            stateService.Fire(TimeLogEntryTrigger.Update); // interdit -> should warn
            stateService.Fire(TimeLogEntryTrigger.ManagerValidates);
            _logger.LogInformation(entryWithUndefinedState.ToString());
        }

        Thread.Sleep(500);
        Console.WriteLine("Press any key");
        Console.ReadKey();
    }

    private TService Resolve<TService>()
    {
        return _serviceProvider.GetRequiredService<TService>();
    }
}