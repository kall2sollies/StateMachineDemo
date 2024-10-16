using System;
using Microsoft.Extensions.Logging;
using Stateless;
using StateMachineDemo.Models;

namespace StateMachineDemo.Services;

public interface ITimeLogEntryStateService
{
    public void Attach(IStateFieldAccessor timeLogEntry);
    public void Detach();
    public void Fire(TimeLogEntryTrigger trigger);
    public bool CanFire(TimeLogEntryTrigger trigger);
}

public class TimeLogEntryStateService :
    ITimeLogEntryStateService
{
    private IStateFieldAccessor _currentTimeLogEntry;
    private StateMachine<TimeLogEntryState, TimeLogEntryTrigger> _stateMachine;
    private readonly IWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger> _workflowProvider;
    private readonly ILogger<TimeLogEntryStateService> _logger;

    public TimeLogEntryStateService(
        ILogger<TimeLogEntryStateService> logger,
        IWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger> workflowProvider)
    {
        _logger = logger;
        _workflowProvider = workflowProvider;

        _workflowProvider.TransitionCompleted += (_, transition) =>
        {
            AddHistory(transition.Trigger, transition.Source, transition.Destination);
        };
    }

    public void Attach(IStateFieldAccessor timeLogEntry)
    {
        _currentTimeLogEntry = timeLogEntry;

        _stateMachine = _workflowProvider.BuildStateMachine(
            stateAccessor: () => _currentTimeLogEntry.State,
            stateMutator: s => _currentTimeLogEntry.State = s);
    }

    public bool CanFire(TimeLogEntryTrigger trigger) => _stateMachine.CanFire(trigger);

    public void Fire(TimeLogEntryTrigger trigger)
    {
        if (CanFire(trigger))
        {
            _stateMachine.Fire(trigger);
        }
        else
        {
            _logger.LogError($"Tentative de trigger {trigger} sur l'état {_currentTimeLogEntry.State}");
        }
    }

    private void AddHistory(TimeLogEntryTrigger trigger, TimeLogEntryState initial, TimeLogEntryState final)
    {
        _currentTimeLogEntry.History.Add(new TimeLogEntryHistoryViewModel(trigger, initial, final));
    }

    public void Detach()
    {
        _currentTimeLogEntry = null;
        _stateMachine = null;
    }
}