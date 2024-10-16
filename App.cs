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
        _timeLogEntryStateService = timeLogEntryStateService;
        _logger = logger;
    }

    public void Run()
    {
        RunTestsWithStrategy(WorkflowProviderImplementationEnum.ManagerValidationWorkflowProvider);
        _logger.LogInformation("---------------------------------------------------------------------------------------------------------------------");
        RunTestsWithStrategy(WorkflowProviderImplementationEnum.ProgressWithoutValidationWorkFlow);
        _logger.LogInformation("---------------------------------------------------------------------------------------------------------------------");
        RunTestsWithStrategy(WorkflowProviderImplementationEnum.EntryWithoutValidationWorkFlow);
        _logger.LogInformation("---------------------------------------------------------------------------------------------------------------------");

        Console.WriteLine("Press any key");
        Console.ReadKey();
    }

    private void RunTestsWithStrategy(WorkflowProviderImplementationEnum workflowStrategy)
    {
        TimeLogEntryViewModel canceledEntry = new();
        _timeLogEntryStateService.Attach(canceledEntry, workflowStrategy);
        _logger.LogInformation($"\n---------------------------------------" +
                               $"\n{nameof(canceledEntry)}" +
                               $"\nInitialState={canceledEntry.State}" +
                               $"\nWorkflow={workflowStrategy}" +
                               $"\n---------------------------------------\n");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Create);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Cancel);
        _logger.LogInformation(canceledEntry.ToString());
        _timeLogEntryStateService.Detach();

        TimeLogEntryViewModel entry = new();
        _timeLogEntryStateService.Attach(entry, workflowStrategy);
        _logger.LogInformation($"\n---------------------------------------" +
                               $"\n{nameof(entry)}" +
                               $"\nInitialState={entry.State}" +
                               $"\nWorkflow={workflowStrategy}" +
                               $"\n---------------------------------------\n");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Create);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Complete);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.SubmitToManager);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.ManagerDeclines);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.SubmitToManager);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.ManagerValidates);
        _logger.LogInformation(entry.ToString());
        _timeLogEntryStateService.Detach();

        TimeLogEntryViewModel entryWithInitialState = new(TimeLogEntryState.InProgress);
        _timeLogEntryStateService.Attach(entryWithInitialState, workflowStrategy);
        _logger.LogInformation($"\n---------------------------------------" +
                               $"\n{nameof(entryWithInitialState)}" +
                               $"\nInitialState={entryWithInitialState.State}" +
                               $"\nWorkflow={workflowStrategy}" +
                               $"\n---------------------------------------\n");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Complete);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.SubmitToManager);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.ManagerDeclines);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.SubmitToManager);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.ManagerValidates);
        _logger.LogInformation(entryWithInitialState.ToString());
        _timeLogEntryStateService.Detach();

        TimeLogEntryViewModel entryWithUndefinedState = new(TimeLogEntryState.Undefined);
        _timeLogEntryStateService.Attach(entryWithUndefinedState, workflowStrategy);
        _logger.LogInformation($"\n---------------------------------------" +
                               $"\n{nameof(entryWithUndefinedState)}" +
                               $"\nInitialState={entryWithUndefinedState.State}" +
                               $"\nWorkflow={workflowStrategy}" +
                               $"\n---------------------------------------\n");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Create);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Complete);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.SubmitToManager);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.ManagerDeclines);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.SubmitToManager);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.ManagerValidates);
        _logger.LogInformation(entryWithUndefinedState.ToString());
        _timeLogEntryStateService.Detach();

        Thread.Sleep(500);
    }
}