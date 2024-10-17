using Microsoft.Extensions.Logging;
using Stateless;
using StateMachineDemo.Models;

namespace StateMachineDemo.Services;

public class TimeLogEntryManagerValidationWorkflowProvider(
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
            .PermitReentry(TimeLogEntryTrigger.Complete)
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