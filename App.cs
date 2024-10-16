using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using StateMachineDemo.Models;
using StateMachineDemo.Services;

namespace StateMachineDemo;

public class App
{
    private readonly ILogger<App> _logger;
    private readonly IWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger> _workflowProvider;

    public App(
        IWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger> workflowProvider,
        ILogger<App> logger)
    {
        _logger = logger;
        _workflowProvider = workflowProvider;
    }

    public void Run()
    {
        TimeLogEntryViewModel canceledEntry = new(_workflowProvider);
        _logger.LogInformation($"{nameof(canceledEntry)}, InitialState={canceledEntry.State}");
        canceledEntry.Fire(TimeLogEntryTrigger.Cancel);
        _logger.LogInformation(canceledEntry.ToString());
        
        TimeLogEntryViewModel entry = new(_workflowProvider);
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

        TimeLogEntryViewModel entryWithInitialState = new(_workflowProvider, TimeLogEntryState.Completed);
        _logger.LogInformation($"{nameof(entryWithInitialState)}, InitialState={entryWithInitialState.State}");
        entryWithInitialState.Fire(TimeLogEntryTrigger.Update);
        entryWithInitialState.Fire(TimeLogEntryTrigger.SubmitToManager);
        entryWithInitialState.Fire(TimeLogEntryTrigger.ManagerDeclines);
        entryWithInitialState.Fire(TimeLogEntryTrigger.Update);
        entryWithInitialState.Fire(TimeLogEntryTrigger.SubmitToManager);
        entryWithInitialState.Fire(TimeLogEntryTrigger.ManagerValidates);
        _logger.LogInformation(entryWithInitialState.ToString());

        TimeLogEntryViewModel entryWithUndefinedState = new(_workflowProvider, TimeLogEntryState.Undefined);
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

        Thread.Sleep(500);
        Console.WriteLine("Press any key");
        Console.ReadKey();
    }
}