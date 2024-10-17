using Autofac.Features.Indexed;
using Microsoft.Extensions.Logging;
using Stateless;
using StateMachineDemo.Models;

namespace StateMachineDemo.Services;

public interface ITimeLogEntryStateService
{
    public void Attach(IStateFieldAccessor timeLogEntry, WorkflowProviderImplementationEnum workflow);
    public void Detach();
    public void Fire(TimeLogEntryTrigger trigger);
    public TimeLogEntryState CurrentState { get; }
}

public class TimeLogEntryStateService :
    ITimeLogEntryStateService
{
    private IStateFieldAccessor _currentTimeLogEntry;
    private StateMachine<TimeLogEntryState, TimeLogEntryTrigger> _stateMachine;
    private readonly ILogger<TimeLogEntryStateService> _logger;
    private readonly IIndex<WorkflowProviderImplementationEnum, IWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger>> _workflowStrategy;

    public TimeLogEntryStateService(
        IIndex<WorkflowProviderImplementationEnum, IWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger>> workflowStrategy,
        ILogger<TimeLogEntryStateService> logger)
    {
        _logger = logger;
        _workflowStrategy = workflowStrategy;
    }

    public TimeLogEntryState CurrentState => _stateMachine.State;

    public void Attach(IStateFieldAccessor timeLogEntry, WorkflowProviderImplementationEnum workflow)
    {
        _currentTimeLogEntry = timeLogEntry;

        IWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger> workflowProvider = _workflowStrategy[workflow];

        workflowProvider.TransitionCompleted += (_, transition) =>
        {
            AddHistory(transition.Trigger, transition.Source, transition.Destination);
        };

        _stateMachine = workflowProvider.BuildStateMachine(
            stateAccessor: () => _currentTimeLogEntry.State,
            stateMutator: s => _currentTimeLogEntry.State = s);
    }

    public bool CanFire(TimeLogEntryTrigger trigger) => _stateMachine.CanFire(trigger);

    public void Fire(TimeLogEntryTrigger trigger)
    {
        if (CanFire(trigger))
        {
            _logger.LogInformation($"\u26a1 {trigger}");
            _stateMachine.Fire(trigger);
        }
        else
        {
            _logger.LogWarning($"\u26d4 {trigger} on state {_currentTimeLogEntry.State}");
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