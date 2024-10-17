using Autofac.Features.Indexed;
using StateMachineDemo.Models;

namespace StateMachineDemo.Services;

public class TimeLogEntryWorkflowProviderFactory(IIndex<WorkflowProviderImplementationEnum, IWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger>> workflowStrategy)
{
    public IWorkflowProvider<TimeLogEntryState, TimeLogEntryTrigger> Create(
        TimeLogEntryState initialState,
        WorkflowProviderImplementationEnum strategy)
    {
        return workflowStrategy[strategy].BuildStateMachine(initialState);
    }
}