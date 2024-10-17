using System;
using Microsoft.Extensions.Logging;
using Stateless;

namespace StateMachineDemo.Services;

public abstract class BaseWorkflowProvider<TState, TTrigger>(
    ILogger<BaseWorkflowProvider<TState, TTrigger>> logger) :
    IWorkflowProvider<TState, TTrigger>
    where TState : Enum
    where TTrigger : Enum
{
    private StateMachine<TState, TTrigger> _stateMachine;

    public event EventHandler<StateMachine<TState, TTrigger>.Transition> TransitionCompleted;

    public TState CurrentState => _stateMachine.State;

    public IWorkflowProvider<TState, TTrigger> BuildStateMachine(TState initialState)
    {
        _stateMachine = new(initialState);

        _stateMachine.OnTransitionCompleted(transition =>
        {
            logger.LogInformation($"\ud83d\udd17 Action: {transition.Trigger}, Transition: {transition.Source} -> {transition.Destination}");
            
            TransitionCompleted?.Invoke(this, transition);
        });

        ConfigureTransitions(_stateMachine);

        return this;
    }

    public void Fire(TTrigger trigger)
    {
        if (_stateMachine.CanFire(trigger))
        {
            logger.LogInformation($"\u26a1 {trigger}");
            _stateMachine.Fire(trigger);
        }
        else
        {
            logger.LogWarning($"\u26d4 {trigger} on state {CurrentState}");
        }
    }

    protected abstract void ConfigureTransitions(StateMachine<TState, TTrigger> stateMachine);
}