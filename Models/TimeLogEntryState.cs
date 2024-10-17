namespace StateMachineDemo.Models;

public enum TimeLogEntryState
{
    Undefined = 0,

    Canceled = 50,
    InProgress = 100,
    Completed = 200,

    DeclinedByManager = 225,
    AwaitingManagerValidation = 250,
    ManagerValidated = 300,

    DeclinedByCustomer = 325,
    AwaitingCustomerApproval = 350,
    CustomerValidated = 400,

    Validated = 500
}