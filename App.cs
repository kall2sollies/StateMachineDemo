using Microsoft.Extensions.Logging;
using StateMachineDemo.Models;

namespace StateMachineDemo;

public class App
{
    private readonly ILogger<App> _logger;


    public App(
        ILogger<App> logger)
    {
        _logger = logger;
    }

    public void Run()
    {
        TimeLogEntryViewModel canceledEntry = new();
        _logger.LogInformation($"{nameof(canceledEntry)}, InitialState={canceledEntry.State}");
        canceledEntry.Fire(TimeLogEntryTrigger.Cancel);
        _logger.LogInformation(canceledEntry.ToString());
        
        TimeLogEntryViewModel entry = new();
        _logger.LogInformation($"{nameof(entry)}, InitialState={entry.State}");
        entry.Fire(TimeLogEntryTrigger.Update);
        entry.Fire(TimeLogEntryTrigger.Complete);
        entry.Fire(TimeLogEntryTrigger.Update);
        entry.Fire(TimeLogEntryTrigger.SubmitToManager);
        entry.Fire(TimeLogEntryTrigger.ManagerDeclines);
        entry.Fire(TimeLogEntryTrigger.Update);
        entry.Fire(TimeLogEntryTrigger.SubmitToManager);
        entry.Fire(TimeLogEntryTrigger.ManagerValidates);
        _logger.LogInformation(entry.ToString());

        TimeLogEntryViewModel entryWithInitialState = new(TimeLogEntryState.Completed);
        _logger.LogInformation($"{nameof(entryWithInitialState)}, InitialState={entryWithInitialState.State}");
        entryWithInitialState.Fire(TimeLogEntryTrigger.Update);
        entryWithInitialState.Fire(TimeLogEntryTrigger.SubmitToManager);
        entryWithInitialState.Fire(TimeLogEntryTrigger.ManagerDeclines);
        entryWithInitialState.Fire(TimeLogEntryTrigger.Update);
        entryWithInitialState.Fire(TimeLogEntryTrigger.SubmitToManager);
        entryWithInitialState.Fire(TimeLogEntryTrigger.ManagerValidates);
        _logger.LogInformation(entryWithInitialState.ToString());

        TimeLogEntryViewModel entryWithUndefinedState = new(TimeLogEntryState.Undefined);
        _logger.LogInformation($"{nameof(entryWithUndefinedState)}, InitialState={entryWithUndefinedState.State}");
        entryWithUndefinedState.Fire(TimeLogEntryTrigger.Update);
        entryWithUndefinedState.Fire(TimeLogEntryTrigger.Complete);
        entryWithUndefinedState.Fire(TimeLogEntryTrigger.Update);
        entryWithUndefinedState.Fire(TimeLogEntryTrigger.SubmitToManager);
        entryWithUndefinedState.Fire(TimeLogEntryTrigger.ManagerDeclines);
        entryWithUndefinedState.Fire(TimeLogEntryTrigger.Update);
        entryWithUndefinedState.Fire(TimeLogEntryTrigger.SubmitToManager);
        entryWithUndefinedState.Fire(TimeLogEntryTrigger.ManagerValidates);
        _logger.LogInformation(entryWithUndefinedState.ToString());
    }
}