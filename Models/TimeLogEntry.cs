using Stateless;
using Stateless.Graph;
using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachineDemo.Models;

public class TimeLogEntryViewModel
{
    public TimeLogEntryState State { get; set; }
    public List<TimeLogEntryHistoryViewModel> History { get; set; } = [];

    private readonly StateMachine<TimeLogEntryState, TimeLogEntryTrigger> _stateMachine;

    public TimeLogEntryViewModel() : this(TimeLogEntryState.InProgress) {}
    public TimeLogEntryViewModel(TimeLogEntryState initialState)
    {
        State = initialState;

        _stateMachine = BuildStateMachine();
    }

    public void Fire(TimeLogEntryTrigger trigger)
    {
        _stateMachine.Fire(trigger);
    }

    public string GetGraph() => UmlDotGraph.Format(_stateMachine.GetInfo());

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine("---------------------------------------");
        sb.AppendLine($"Current State: {State}");

        if (History.Count > 0)
        {
            sb.AppendLine("---------------------------------------");
            History.ForEach(x => sb.AppendLine(x.ToString()));
        }

        sb.AppendLine("---------------------------------------\n\n\n");

        return sb.ToString();
    }

    private void AddHistory(TimeLogEntryTrigger trigger, TimeLogEntryState initial, TimeLogEntryState final) =>
        History.Add(new TimeLogEntryHistoryViewModel(trigger, initial, final));

    private StateMachine<TimeLogEntryState, TimeLogEntryTrigger> BuildStateMachine()
    {
        StateMachine<TimeLogEntryState, TimeLogEntryTrigger> stateMachine = new(
            stateAccessor: () => State,
            stateMutator: s => State = s);

        stateMachine.OnTransitionCompleted(transition => AddHistory(transition.Trigger, transition.Source, transition.Destination));

        stateMachine.Configure(TimeLogEntryState.Undefined)
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
            .Permit(TimeLogEntryTrigger.WorkflowComplete, TimeLogEntryState.Validated);

        return stateMachine;
    }
}

public class TimeLogEntryHistoryViewModel
{
    public DateTime Date { get; set; }
    public TimeLogEntryTrigger Trigger { get; set; }
    public TimeLogEntryState InitialState { get; set; }
    public TimeLogEntryState FinalState { get; set; }

    public TimeLogEntryHistoryViewModel(TimeLogEntryTrigger trigger, TimeLogEntryState initial, TimeLogEntryState final)
    {
        Date = DateTime.Now;
        Trigger = trigger;
        InitialState = initial;
        FinalState = final;
    }

    public override string ToString() => $"[{Date}] Action: {Trigger}, Transition: {InitialState} -> {FinalState}";
}