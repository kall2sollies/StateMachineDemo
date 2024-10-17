using System;
using System.Collections.Generic;
using System.Text;
using StateMachineDemo.Services;

namespace StateMachineDemo.Models;

public class TimeLogEntryViewModel
{
    public TimeLogEntryState State { get; set; }
    public List<TimeLogEntryHistoryViewModel> History { get; set; } = [];
    public string Name { get; set; }
    public WorkflowProviderImplementationEnum Strategy { get; set; }

    public TimeLogEntryViewModel()
    {
    }

    public TimeLogEntryViewModel(TimeLogEntryState initialState)
    {
        State = initialState;
    }

    public void AddHistory(TimeLogEntryTrigger trigger, TimeLogEntryState initial, TimeLogEntryState final)
    {
        History.Add(new TimeLogEntryHistoryViewModel(trigger, initial, final));
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine("\n---------------------------------------");
        sb.AppendLine($"Name: {Name}");
        sb.AppendLine($"Current State: {State}");
        sb.AppendLine($"Strategy: {Strategy}");

        if (History.Count > 0)
        {
            sb.AppendLine("---------------------------------------");
            History.ForEach(x => sb.AppendLine(x.ToString()));
        }

        sb.AppendLine("---------------------------------------");

        return sb.ToString();
    }
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