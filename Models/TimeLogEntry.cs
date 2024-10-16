using Stateless;
using Stateless.Graph;
using StateMachineDemo.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachineDemo.Models;

public class TimeLogEntryViewModel
{
    public TimeLogEntryState State { get; set; }
    public List<TimeLogEntryHistoryViewModel> History { get; set; } = [];

    private readonly StateMachine<TimeLogEntryState, TimeLogEntryTrigger> _stateMachine;

    public TimeLogEntryViewModel(IWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger> workflowProvider) :
        this(workflowProvider, TimeLogEntryState.InProgress)
    {

    }

    public TimeLogEntryViewModel(IWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger> workflowProvider, TimeLogEntryState initialState)
    {
        State = initialState;

        _stateMachine = workflowProvider.BuildStateMachine(
            stateAccessor: () => State,
            stateMutator: s => State = s);

        workflowProvider.TransitionCompleted += (_, transition) => AddHistory(transition.Trigger, transition.Source, transition.Destination);
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
}

public class TimeLogEntryHistoryViewModel(
    TimeLogEntryTrigger trigger,
    TimeLogEntryState initial,
    TimeLogEntryState final)
{
    public DateTime Date { get; set; } = DateTime.Now;
    public TimeLogEntryTrigger Trigger { get; set; } = trigger;
    public TimeLogEntryState InitialState { get; set; } = initial;
    public TimeLogEntryState FinalState { get; set; } = final;

    public override string ToString() => $"[{Date}] Action: {Trigger}, Transition: {InitialState} -> {FinalState}";
}