using System;
using Stateless;

namespace StateMachineDemo.Services;

public interface IWorkflowProvider<TState, TTrigger>
    where TState : Enum
    where TTrigger : Enum 
{
    public event EventHandler<StateMachine<TState, TTrigger>.Transition> TransitionCompleted;

    public TState CurrentState { get; }

    public void Fire(TTrigger trigger);

    IWorkflowProvider<TState, TTrigger> BuildStateMachine(TState initialState);
}