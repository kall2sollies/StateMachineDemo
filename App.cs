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

        // Transition callbacks are async
        Thread.Sleep(500);
    }

    private void RunTestsWithStrategy(WorkflowProviderImplementationEnum workflowStrategy)
    {
        EntryWillBeCanceled(workflowStrategy);
        EntryWillCompleteWorkflow(workflowStrategy);
        EntryWillCompleteWorkflowFromIntermediaryState(TimeLogEntryState.Completed, workflowStrategy);
    }

    private void EntryWillBeCanceled(WorkflowProviderImplementationEnum workflowStrategy)
    {
        TimeLogEntryViewModel entryWillBeCanceled = new();
        _timeLogEntryStateService.Attach(entryWillBeCanceled, workflowStrategy);
        _logger.LogInformation($"\n---------------------------------------" +
                               $"\n{nameof(entryWillBeCanceled)}" +
                               $"\nInitialState={entryWillBeCanceled.State}" +
                               $"\nWorkflow={workflowStrategy}" +
                               $"\n---------------------------------------\n");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Create);
        _logger.LogInformation($"\u2699\ufe0f CurrentState={_timeLogEntryStateService.CurrentState}");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Cancel);
        _logger.LogInformation($"\u2699\ufe0f CurrentState={_timeLogEntryStateService.CurrentState}");
        _logger.LogInformation(entryWillBeCanceled.ToString());
        _timeLogEntryStateService.Detach();
    }

    private void EntryWillCompleteWorkflow(WorkflowProviderImplementationEnum workflowStrategy)
    {
        TimeLogEntryViewModel entryWillCompleteWorkflow = new();
        _timeLogEntryStateService.Attach(entryWillCompleteWorkflow, workflowStrategy);
        _logger.LogInformation($"\n---------------------------------------" +
                               $"\n{nameof(entryWillCompleteWorkflow)}" +
                               $"\nInitialState={entryWillCompleteWorkflow.State}" +
                               $"\nWorkflow={workflowStrategy}" +
                               $"\n---------------------------------------\n");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Create);
        _logger.LogInformation($"\u2699\ufe0f CurrentState={_timeLogEntryStateService.CurrentState}");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _logger.LogInformation($"\u2699\ufe0f CurrentState={_timeLogEntryStateService.CurrentState}");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Complete);
        _logger.LogInformation($"\u2699\ufe0f CurrentState={_timeLogEntryStateService.CurrentState}");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _logger.LogInformation($"\u2699\ufe0f CurrentState={_timeLogEntryStateService.CurrentState}");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.SubmitToManager);
        _logger.LogInformation($"\u2699\ufe0f CurrentState={_timeLogEntryStateService.CurrentState}");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.ManagerDeclines);
        _logger.LogInformation($"\u2699\ufe0f CurrentState={_timeLogEntryStateService.CurrentState}");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _logger.LogInformation($"\u2699\ufe0f CurrentState={_timeLogEntryStateService.CurrentState}");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.SubmitToManager);
        _logger.LogInformation($"\u2699\ufe0f CurrentState={_timeLogEntryStateService.CurrentState}");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.ManagerValidates);
        _logger.LogInformation($"\u2699\ufe0f CurrentState={_timeLogEntryStateService.CurrentState}");
        _logger.LogInformation(entryWillCompleteWorkflow.ToString());
        _timeLogEntryStateService.Detach();
    }

    private void EntryWillCompleteWorkflowFromIntermediaryState(TimeLogEntryState state, WorkflowProviderImplementationEnum workflowStrategy)
    {
        TimeLogEntryViewModel entryWillCompleteWorkflowFromIntermediaryState = new(state);
        _timeLogEntryStateService.Attach(entryWillCompleteWorkflowFromIntermediaryState, workflowStrategy);
        _logger.LogInformation($"\n---------------------------------------" +
                               $"\n{nameof(entryWillCompleteWorkflowFromIntermediaryState)}" +
                               $"\nInitialState={entryWillCompleteWorkflowFromIntermediaryState.State}" +
                               $"\nWorkflow={workflowStrategy}" +
                               $"\n---------------------------------------\n");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _logger.LogInformation($"\u2699\ufe0f CurrentState={_timeLogEntryStateService.CurrentState}");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Complete);
        _logger.LogInformation($"\u2699\ufe0f CurrentState={_timeLogEntryStateService.CurrentState}");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.SubmitToManager);
        _logger.LogInformation($"\u2699\ufe0f CurrentState={_timeLogEntryStateService.CurrentState}");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.ManagerDeclines);
        _logger.LogInformation($"\u2699\ufe0f CurrentState={_timeLogEntryStateService.CurrentState}");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.Update);
        _logger.LogInformation($"\u2699\ufe0f CurrentState={_timeLogEntryStateService.CurrentState}");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.SubmitToManager);
        _logger.LogInformation($"\u2699\ufe0f CurrentState={_timeLogEntryStateService.CurrentState}");
        _timeLogEntryStateService.Fire(TimeLogEntryTrigger.ManagerValidates);
        _logger.LogInformation($"\u2699\ufe0f CurrentState={_timeLogEntryStateService.CurrentState}");
        _logger.LogInformation(entryWillCompleteWorkflowFromIntermediaryState.ToString());
        _timeLogEntryStateService.Detach();
    }
}