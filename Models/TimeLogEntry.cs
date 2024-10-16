using System;
using System.Collections.Generic;
using System.Text;

namespace StateMachineDemo.Models;

public interface IStateAccessor
{
    public TimeLogEntryState State { get; set; }
    public List<TimeLogEntryHistoryViewModel> History { get; set; }
}

public class TimeLogEntryViewModel : IStateAccessor
{
    public TimeLogEntryState State { get; set; }
    public List<TimeLogEntryHistoryViewModel> History { get; set; } = [];

    public TimeLogEntryViewModel()
    {
    }

    public TimeLogEntryViewModel(TimeLogEntryState initialState)
    {
        State = initialState;
    }

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