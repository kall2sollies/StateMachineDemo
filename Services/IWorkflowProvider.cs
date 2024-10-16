using System;
using Microsoft.Extensions.Logging;
using Stateless;
using StateMachineDemo.Models;

namespace StateMachineDemo.Services;

public enum WorkflowProviderImplementationEnum
{
    ManagerValidationWorkflowProvider,
    ProgressWithoutValidationWorkFlow,
    EntryWithoutValidationWorkFlow
}

public interface IWorkflowProvider<TState, TTrigger>
    where TState : Enum
    where TTrigger : Enum 
{
    public event EventHandler<StateMachine<TState, TTrigger>.Transition> TransitionCompleted;

    StateMachine<TState, TTrigger> BuildStateMachine(
        Func<TState> stateAccessor,
        Action<TState> stateMutator);
}

public abstract class BaseWorkflowProvider<TState, TTrigger>(
    ILogger<BaseWorkflowProvider<TState, TTrigger>> logger) :
    IWorkflowProvider<TState, TTrigger>
    where TState : Enum
    where TTrigger : Enum
{
    public event EventHandler<StateMachine<TState, TTrigger>.Transition> TransitionCompleted;

    public StateMachine<TState, TTrigger> BuildStateMachine(Func<TState> stateAccessor, Action<TState> stateMutator)
    {
        StateMachine<TState, TTrigger> stateMachine = new(stateAccessor, stateMutator);

        stateMachine.OnTransitionCompleted(transition =>
        {
            logger.LogInformation($"[OnTransitionCompleted] Action: {transition.Trigger}, Transition: {transition.Source} -> {transition.Destination}");
            
            TransitionCompleted?.Invoke(this, transition);
        });

        ConfigureTransitions(stateMachine);

        return stateMachine;
    }

    protected abstract void ConfigureTransitions(StateMachine<TState, TTrigger> stateMachine);
}

public class ManagerValidationWorkflowProvider(
    ILogger<BaseWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger>> logger) :
    BaseWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger>(logger)
{
    protected override void ConfigureTransitions(StateMachine<TimeLogEntryState, TimeLogEntryTrigger> stateMachine)
    {
        stateMachine.Configure(TimeLogEntryState.Undefined)
            .Permit(TimeLogEntryTrigger.Create, TimeLogEntryState.InProgress)
            .Permit(TimeLogEntryTrigger.Update, TimeLogEntryState.InProgress)
            .Permit(TimeLogEntryTrigger.Complete, TimeLogEntryState.Completed)
            .Permit(TimeLogEntryTrigger.Cancel, TimeLogEntryState.Canceled)
            ;

        stateMachine.Configure(TimeLogEntryState.InProgress)
            .PermitReentry(TimeLogEntryTrigger.Update)
            .Permit(TimeLogEntryTrigger.Complete, TimeLogEntryState.Completed)
            .Permit(TimeLogEntryTrigger.Cancel, TimeLogEntryState.Canceled)
            ;

        stateMachine.Configure(TimeLogEntryState.Completed)
            .PermitReentry(TimeLogEntryTrigger.Update)
            .Permit(TimeLogEntryTrigger.SubmitToManager, TimeLogEntryState.AwaitingManagerValidation)
            ;

        stateMachine.Configure(TimeLogEntryState.AwaitingManagerValidation)
            .Permit(TimeLogEntryTrigger.ManagerValidates, TimeLogEntryState.ManagerValidated)
            .Permit(TimeLogEntryTrigger.ManagerDeclines, TimeLogEntryState.DeclinedByManager)
            ;

        stateMachine.Configure(TimeLogEntryState.DeclinedByManager)
            .PermitReentry(TimeLogEntryTrigger.Update)
            .Permit(TimeLogEntryTrigger.SubmitToManager, TimeLogEntryState.AwaitingManagerValidation)
            ;

        stateMachine.Configure(TimeLogEntryState.ManagerValidated)
            .OnEntry(() => stateMachine.Fire(TimeLogEntryTrigger.WorkflowComplete))
            .Permit(TimeLogEntryTrigger.WorkflowComplete, TimeLogEntryState.Validated)
            ;
    }
}

public class ProgressWithoutValidationWorkFlow(
    ILogger<BaseWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger>> logger) :
    BaseWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger>(logger)
{
    protected override void ConfigureTransitions(StateMachine<TimeLogEntryState, TimeLogEntryTrigger> stateMachine)
    {
        stateMachine.Configure(TimeLogEntryState.Undefined)
            .Permit(TimeLogEntryTrigger.Create, TimeLogEntryState.InProgress)
            .Permit(TimeLogEntryTrigger.Update, TimeLogEntryState.InProgress)
            .Permit(TimeLogEntryTrigger.Complete, TimeLogEntryState.Completed)
            .Permit(TimeLogEntryTrigger.Cancel, TimeLogEntryState.Canceled)
            ;

        stateMachine.Configure(TimeLogEntryState.InProgress)
            .PermitReentry(TimeLogEntryTrigger.Update)
            .Permit(TimeLogEntryTrigger.Complete, TimeLogEntryState.Completed)
            .Permit(TimeLogEntryTrigger.Cancel, TimeLogEntryState.Canceled)
            ;

        stateMachine.Configure(TimeLogEntryState.Completed)
            .OnEntry(() => stateMachine.Fire(TimeLogEntryTrigger.WorkflowComplete))
            .Permit(TimeLogEntryTrigger.WorkflowComplete, TimeLogEntryState.Validated)
            ;

        stateMachine.Configure(TimeLogEntryState.Validated)
            .PermitReentry(TimeLogEntryTrigger.Update)
            ;
    }
}

public class EntryWithoutValidationWorkFlow(
    ILogger<BaseWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger>> logger) :
    BaseWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger>(logger)
{
    protected override void ConfigureTransitions(StateMachine<TimeLogEntryState, TimeLogEntryTrigger> stateMachine)
    {
        stateMachine.Configure(TimeLogEntryState.Undefined)
            .Permit(TimeLogEntryTrigger.Create, TimeLogEntryState.Completed)
            .Permit(TimeLogEntryTrigger.Update, TimeLogEntryState.Completed)
            .Permit(TimeLogEntryTrigger.Complete, TimeLogEntryState.Completed)
            .Permit(TimeLogEntryTrigger.Cancel, TimeLogEntryState.Canceled)
            ;

        stateMachine.Configure(TimeLogEntryState.InProgress)
            .Permit(TimeLogEntryTrigger.Update, TimeLogEntryState.Completed)
            .Permit(TimeLogEntryTrigger.Complete, TimeLogEntryState.Completed)
            ;

        stateMachine.Configure(TimeLogEntryState.Completed)
            .OnEntry(() => stateMachine.Fire(TimeLogEntryTrigger.WorkflowComplete))
            .Permit(TimeLogEntryTrigger.WorkflowComplete, TimeLogEntryState.Validated)
            ;

        stateMachine.Configure(TimeLogEntryState.Validated)
            .PermitReentry(TimeLogEntryTrigger.Update)
            ;
    }
}