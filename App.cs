using Microsoft.Extensions.Logging;
using StateMachineDemo.Models;
using StateMachineDemo.Services;

namespace StateMachineDemo;

public class App(
    TimeLogEntryWorkflowProviderFactory workflowProviderFactory,
    ILogger<App> logger)
{
    public void Run()
    {
        RunTestsWithStrategy(WorkflowProviderImplementationEnum.ManagerValidationWorkflowProvider);
        logger.LogInformation("---------------------------------------------------------------------------------------------------------------------");
        RunTestsWithStrategy(WorkflowProviderImplementationEnum.ProgressWithoutValidationWorkFlow);
        logger.LogInformation("---------------------------------------------------------------------------------------------------------------------");
        RunTestsWithStrategy(WorkflowProviderImplementationEnum.EntryWithoutValidationWorkFlow);
        logger.LogInformation("---------------------------------------------------------------------------------------------------------------------");
    }

    private void RunTestsWithStrategy(WorkflowProviderImplementationEnum workflowStrategy)
    {
        EntryWillBeCanceled(workflowStrategy);
        EntryWillCompleteWorkflow(workflowStrategy);
        EntryWillCompleteWorkflowFromIntermediaryState(TimeLogEntryState.Completed, workflowStrategy);
    }

    private void FireAndLog(TimeLogEntryTrigger trigger, TimeLogEntryViewModel entry, IWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger> workflowProvider)
    {
        workflowProvider.Fire(trigger);
        entry.State = workflowProvider.CurrentState;
        logger.LogInformation($"\u2699\ufe0f CurrentState={workflowProvider.CurrentState}");
    }

    private void EntryWillBeCanceled(WorkflowProviderImplementationEnum workflowStrategy)
    {
        TimeLogEntryViewModel entry = new() {Name = "Entry will be canceled", Strategy = workflowStrategy};
        logger.LogInformation(entry.ToString());

        IWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger> workflowProvider = workflowProviderFactory.Create(entry.State, workflowStrategy);

        workflowProvider.TransitionCompleted += (_, transition) => entry.AddHistory(transition.Trigger, transition.Source, transition.Destination);

        FireAndLog(TimeLogEntryTrigger.Create, entry, workflowProvider);
        FireAndLog(TimeLogEntryTrigger.Cancel, entry, workflowProvider);

        logger.LogInformation(entry.ToString());
    }
    
    private void EntryWillCompleteWorkflow(WorkflowProviderImplementationEnum workflowStrategy)
    {
        TimeLogEntryViewModel entry = new() {Name = "Entry will complete workflow", Strategy = workflowStrategy};
        logger.LogInformation(entry.ToString());

        IWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger> workflowProvider = workflowProviderFactory.Create(entry.State, workflowStrategy);

        workflowProvider.TransitionCompleted += (_, transition) => entry.AddHistory(transition.Trigger, transition.Source, transition.Destination);

        FireAndLog(TimeLogEntryTrigger.Create, entry, workflowProvider);
        FireAndLog(TimeLogEntryTrigger.Update, entry, workflowProvider);
        FireAndLog(TimeLogEntryTrigger.Complete, entry, workflowProvider);
        FireAndLog(TimeLogEntryTrigger.Update, entry, workflowProvider);
        FireAndLog(TimeLogEntryTrigger.SubmitToManager, entry, workflowProvider);
        FireAndLog(TimeLogEntryTrigger.ManagerDeclines, entry, workflowProvider);
        FireAndLog(TimeLogEntryTrigger.Update, entry, workflowProvider);
        FireAndLog(TimeLogEntryTrigger.SubmitToManager, entry, workflowProvider);
        FireAndLog(TimeLogEntryTrigger.ManagerValidates, entry, workflowProvider);

        logger.LogInformation(entry.ToString());
    }
    
    private void EntryWillCompleteWorkflowFromIntermediaryState(TimeLogEntryState state, WorkflowProviderImplementationEnum workflowStrategy)
    {
        TimeLogEntryViewModel entry = new(state) {Name = "Entry will complete workflow with initial state", Strategy = workflowStrategy};
        logger.LogInformation(entry.ToString());

        IWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger> workflowProvider = workflowProviderFactory.Create(entry.State, workflowStrategy);

        workflowProvider.TransitionCompleted += (_, transition) => entry.AddHistory(transition.Trigger, transition.Source, transition.Destination);

        FireAndLog(TimeLogEntryTrigger.Create, entry, workflowProvider);
        FireAndLog(TimeLogEntryTrigger.Update, entry, workflowProvider);
        FireAndLog(TimeLogEntryTrigger.Complete, entry, workflowProvider);
        FireAndLog(TimeLogEntryTrigger.Update, entry, workflowProvider);
        FireAndLog(TimeLogEntryTrigger.SubmitToManager, entry, workflowProvider);
        FireAndLog(TimeLogEntryTrigger.ManagerDeclines, entry, workflowProvider);
        FireAndLog(TimeLogEntryTrigger.Update, entry, workflowProvider);
        FireAndLog(TimeLogEntryTrigger.SubmitToManager, entry, workflowProvider);
        FireAndLog(TimeLogEntryTrigger.ManagerValidates, entry, workflowProvider);

        logger.LogInformation(entry.ToString());
    }
}