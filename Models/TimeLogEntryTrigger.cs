namespace StateMachineDemo.Models;

public enum TimeLogEntryTrigger
{
    Create,
    Update,
    Complete,
    Cancel,
    SubmitToManager,
    ManagerValidates,
    ManagerDeclines,
    SubmitToCustomer,
    CustomerValidates,
    CustomerDeclines,
    WorkflowComplete
}