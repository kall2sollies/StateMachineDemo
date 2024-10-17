using Microsoft.Extensions.Logging;
using Stateless;
using StateMachineDemo.Models;

namespace StateMachineDemo.Services;

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
            .PermitReentry(TimeLogEntryTrigger.Update)
            .OnEntry(() => stateMachine.Fire(TimeLogEntryTrigger.WorkflowComplete))
            .Permit(TimeLogEntryTrigger.WorkflowComplete, TimeLogEntryState.Validated)
            ;

        stateMachine.Configure(TimeLogEntryState.Validated)
            .PermitReentry(TimeLogEntryTrigger.Update)
            .Permit(TimeLogEntryTrigger.Cancel, TimeLogEntryState.Canceled)
            ;
    }
}