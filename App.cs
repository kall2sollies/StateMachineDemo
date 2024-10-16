using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using StateMachineDemo.Models;
using StateMachineDemo.Services;

namespace StateMachineDemo;

public class App
{
    private readonly ILogger<App> _logger;
    private readonly ITimeLogEntryStateService _timeLogEntryStateService;

    public App(
        ITimeLogEntryStateService timeLogEntryStateService,
        ILogger<App> logger)
    {
        _logger = logger;
        _timeLogEntryStateService = timeLogEntryStateService;
    }

    public void Run()
    {
        TimeLogEntryViewModel canceledEntry = new();
        _timeLogEntryStateService.Attach(canceledEntry);
        _logger.LogInformation($"\n---------------------------------------\n{nameof(canceledEntry)}, InitialState={canceledEntry.State}\n---------------------------------------\n");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Cancel);
        _logger.LogInformation(canceledEntry.ToString());

        TimeLogEntryViewModel entry = new();
        _timeLogEntryStateService.Attach(entry);
        _logger.LogInformation($"\n---------------------------------------\n{nameof(entry)}, InitialState={entry.State}\n---------------------------------------\n");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Complete);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.SubmitToManager);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.ManagerDeclines);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.SubmitToManager);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.ManagerValidates);
        _logger.LogInformation(entry.ToString());

        TimeLogEntryViewModel entryWithInitialState = new(TimeLogEntryState.Completed);
        _timeLogEntryStateService.Attach(entryWithInitialState);
        _logger.LogInformation($"\n---------------------------------------\n{nameof(entryWithInitialState)}, InitialState={entryWithInitialState.State}\n---------------------------------------\n");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.SubmitToManager);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.ManagerDeclines);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.SubmitToManager);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.ManagerValidates);
        _logger.LogInformation(entryWithInitialState.ToString());

        TimeLogEntryViewModel entryWithUndefinedState = new(TimeLogEntryState.Undefined);
        _timeLogEntryStateService.Attach(entryWithUndefinedState);
        _logger.LogInformation($"\n---------------------------------------\n{nameof(entryWithUndefinedState)}, InitialState={entryWithUndefinedState.State}\n---------------------------------------\n");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Complete);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.SubmitToManager);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.ManagerDeclines);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.SubmitToManager);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update); // interdit -> should warn
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.ManagerValidates);
        _logger.LogInformation(entryWithUndefinedState.ToString());

        Thread.Sleep(500);
        Console.WriteLine("Press any key");
        Console.ReadKey();
    }
}