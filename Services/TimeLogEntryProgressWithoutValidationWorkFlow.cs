using Microsoft.Extensions.Logging;
using Stateless;
using StateMachineDemo.Models;

namespace StateMachineDemo.Services;

public class TimeLogEntryProgressWithoutValidationWorkFlow(
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
            .OnEntry(() => stateMachine.Fire(TimeLogEntryTrigger.WorkflowComplete))
            .Permit(TimeLogEntryTrigger.WorkflowComplete, TimeLogEntryState.Validated)
            ;

        stateMachine.Configure(TimeLogEntryState.Validated)
            .PermitReentry(TimeLogEntryTrigger.Update)
            .Permit(TimeLogEntryTrigger.Cancel, TimeLogEntryState.Canceled)
            ;
    }
}