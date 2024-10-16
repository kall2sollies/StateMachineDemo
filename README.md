# What's this

This project is a sandbox project to play with Stateless within dependency injection context.
Check out Stateless : https://github.com/dotnet-state-machine/stateless

# Results
````log
[2024-10-16 23:50:27.632][INF] 
---------------------------------------
entryWillBeCanceled
InitialState=Undefined
Workflow=ManagerValidationWorkflowProvider
---------------------------------------
 
[2024-10-16 23:50:27.668][INF] ⚡ Create 
[2024-10-16 23:50:27.670][INF] 🔗 Action: Create, Transition: Undefined -> InProgress 
[2024-10-16 23:50:27.671][INF] ⚡ Cancel 
[2024-10-16 23:50:27.671][INF] 🔗 Action: Cancel, Transition: InProgress -> Canceled 
[2024-10-16 23:50:27.674][INF] 
---------------------------------------
Current State: Canceled
---------------------------------------
[16/10/2024 23:50:27] Action: Create, Transition: Undefined -> InProgress
[16/10/2024 23:50:27] Action: Cancel, Transition: InProgress -> Canceled
---------------------------------------



 
[2024-10-16 23:50:27.675][INF] 
---------------------------------------
entryWillCompleteWorkflow
InitialState=Undefined
Workflow=ManagerValidationWorkflowProvider
---------------------------------------
 
[2024-10-16 23:50:27.675][INF] ⚡ Create 
[2024-10-16 23:50:27.675][INF] 🔗 Action: Create, Transition: Undefined -> InProgress 
[2024-10-16 23:50:27.675][INF] ⚡ Update 
[2024-10-16 23:50:27.676][INF] 🔗 Action: Update, Transition: InProgress -> InProgress 
[2024-10-16 23:50:27.676][INF] ⚡ Complete 
[2024-10-16 23:50:27.676][INF] 🔗 Action: Complete, Transition: InProgress -> Completed 
[2024-10-16 23:50:27.676][INF] ⚡ Update 
[2024-10-16 23:50:27.676][INF] 🔗 Action: Update, Transition: Completed -> Completed 
[2024-10-16 23:50:27.676][INF] ⚡ SubmitToManager 
[2024-10-16 23:50:27.676][INF] 🔗 Action: SubmitToManager, Transition: Completed -> AwaitingManagerValidation 
[2024-10-16 23:50:27.676][INF] ⚡ ManagerDeclines 
[2024-10-16 23:50:27.676][INF] 🔗 Action: ManagerDeclines, Transition: AwaitingManagerValidation -> DeclinedByManager 
[2024-10-16 23:50:27.676][INF] ⚡ Update 
[2024-10-16 23:50:27.676][INF] 🔗 Action: Update, Transition: DeclinedByManager -> DeclinedByManager 
[2024-10-16 23:50:27.676][INF] ⚡ SubmitToManager 
[2024-10-16 23:50:27.677][INF] 🔗 Action: SubmitToManager, Transition: DeclinedByManager -> AwaitingManagerValidation 
[2024-10-16 23:50:27.677][INF] ⚡ ManagerValidates 
[2024-10-16 23:50:27.677][INF] 🔗 Action: ManagerValidates, Transition: AwaitingManagerValidation -> ManagerValidated 
[2024-10-16 23:50:27.677][INF] 🔗 Action: WorkflowComplete, Transition: ManagerValidated -> Validated 
[2024-10-16 23:50:27.677][INF] 
---------------------------------------
Current State: Validated
---------------------------------------
[16/10/2024 23:50:27] Action: Create, Transition: Undefined -> InProgress
[16/10/2024 23:50:27] Action: Update, Transition: InProgress -> InProgress
[16/10/2024 23:50:27] Action: Complete, Transition: InProgress -> Completed
[16/10/2024 23:50:27] Action: Update, Transition: Completed -> Completed
[16/10/2024 23:50:27] Action: SubmitToManager, Transition: Completed -> AwaitingManagerValidation
[16/10/2024 23:50:27] Action: ManagerDeclines, Transition: AwaitingManagerValidation -> DeclinedByManager
[16/10/2024 23:50:27] Action: Update, Transition: DeclinedByManager -> DeclinedByManager
[16/10/2024 23:50:27] Action: SubmitToManager, Transition: DeclinedByManager -> AwaitingManagerValidation
[16/10/2024 23:50:27] Action: ManagerValidates, Transition: AwaitingManagerValidation -> ManagerValidated
[16/10/2024 23:50:27] Action: WorkflowComplete, Transition: ManagerValidated -> Validated
---------------------------------------



 
[2024-10-16 23:50:27.677][INF] 
---------------------------------------
entryWillCompleteWorkflowFromIntermediaryState
InitialState=Completed
Workflow=ManagerValidationWorkflowProvider
---------------------------------------
 
[2024-10-16 23:50:27.677][INF] ⚡ Update 
[2024-10-16 23:50:27.677][INF] 🔗 Action: Update, Transition: Completed -> Completed 
[2024-10-16 23:50:27.677][INF] ⚡ Complete 
[2024-10-16 23:50:27.677][INF] 🔗 Action: Complete, Transition: Completed -> Completed 
[2024-10-16 23:50:27.677][INF] ⚡ SubmitToManager 
[2024-10-16 23:50:27.677][INF] 🔗 Action: SubmitToManager, Transition: Completed -> AwaitingManagerValidation 
[2024-10-16 23:50:27.678][INF] ⚡ ManagerDeclines 
[2024-10-16 23:50:27.678][INF] 🔗 Action: ManagerDeclines, Transition: AwaitingManagerValidation -> DeclinedByManager 
[2024-10-16 23:50:27.678][INF] ⚡ Update 
[2024-10-16 23:50:27.678][INF] 🔗 Action: Update, Transition: DeclinedByManager -> DeclinedByManager 
[2024-10-16 23:50:27.678][INF] ⚡ SubmitToManager 
[2024-10-16 23:50:27.678][INF] 🔗 Action: SubmitToManager, Transition: DeclinedByManager -> AwaitingManagerValidation 
[2024-10-16 23:50:27.678][INF] ⚡ ManagerValidates 
[2024-10-16 23:50:27.678][INF] 🔗 Action: ManagerValidates, Transition: AwaitingManagerValidation -> ManagerValidated 
[2024-10-16 23:50:27.678][INF] 🔗 Action: WorkflowComplete, Transition: ManagerValidated -> Validated 
[2024-10-16 23:50:27.678][INF] 
---------------------------------------
Current State: Validated
---------------------------------------
[16/10/2024 23:50:27] Action: Update, Transition: Completed -> Completed
[16/10/2024 23:50:27] Action: Complete, Transition: Completed -> Completed
[16/10/2024 23:50:27] Action: SubmitToManager, Transition: Completed -> AwaitingManagerValidation
[16/10/2024 23:50:27] Action: ManagerDeclines, Transition: AwaitingManagerValidation -> DeclinedByManager
[16/10/2024 23:50:27] Action: Update, Transition: DeclinedByManager -> DeclinedByManager
[16/10/2024 23:50:27] Action: SubmitToManager, Transition: DeclinedByManager -> AwaitingManagerValidation
[16/10/2024 23:50:27] Action: ManagerValidates, Transition: AwaitingManagerValidation -> ManagerValidated
[16/10/2024 23:50:27] Action: WorkflowComplete, Transition: ManagerValidated -> Validated
---------------------------------------



 
[2024-10-16 23:50:27.678][INF] --------------------------------------------------------------------------------------------------------------------- 
[2024-10-16 23:50:27.678][INF] 
---------------------------------------
entryWillBeCanceled
InitialState=Undefined
Workflow=ProgressWithoutValidationWorkFlow
---------------------------------------
 
[2024-10-16 23:50:27.678][INF] ⚡ Create 
[2024-10-16 23:50:27.679][INF] 🔗 Action: Create, Transition: Undefined -> InProgress 
[2024-10-16 23:50:27.679][INF] ⚡ Cancel 
[2024-10-16 23:50:27.679][INF] 🔗 Action: Cancel, Transition: InProgress -> Canceled 
[2024-10-16 23:50:27.679][INF] 
---------------------------------------
Current State: Canceled
---------------------------------------
[16/10/2024 23:50:27] Action: Create, Transition: Undefined -> InProgress
[16/10/2024 23:50:27] Action: Cancel, Transition: InProgress -> Canceled
---------------------------------------



 
[2024-10-16 23:50:27.679][INF] 
---------------------------------------
entryWillCompleteWorkflow
InitialState=Undefined
Workflow=ProgressWithoutValidationWorkFlow
---------------------------------------
 
[2024-10-16 23:50:27.679][INF] ⚡ Create 
[2024-10-16 23:50:27.679][INF] 🔗 Action: Create, Transition: Undefined -> InProgress 
[2024-10-16 23:50:27.679][INF] ⚡ Update 
[2024-10-16 23:50:27.679][INF] 🔗 Action: Update, Transition: InProgress -> InProgress 
[2024-10-16 23:50:27.679][INF] ⚡ Complete 
[2024-10-16 23:50:27.679][INF] 🔗 Action: Complete, Transition: InProgress -> Completed 
[2024-10-16 23:50:27.679][INF] 🔗 Action: WorkflowComplete, Transition: Completed -> Validated 
[2024-10-16 23:50:27.679][INF] ⚡ Update 
[2024-10-16 23:50:27.679][INF] 🔗 Action: Update, Transition: Validated -> Validated 
[2024-10-16 23:50:27.680][WRN] ⛔ SubmitToManager on state Validated 
[2024-10-16 23:50:27.680][WRN] ⛔ ManagerDeclines on state Validated 
[2024-10-16 23:50:27.680][INF] ⚡ Update 
[2024-10-16 23:50:27.680][INF] 🔗 Action: Update, Transition: Validated -> Validated 
[2024-10-16 23:50:27.680][WRN] ⛔ SubmitToManager on state Validated 
[2024-10-16 23:50:27.680][WRN] ⛔ ManagerValidates on state Validated 
[2024-10-16 23:50:27.680][INF] 
---------------------------------------
Current State: Validated
---------------------------------------
[16/10/2024 23:50:27] Action: Create, Transition: Undefined -> InProgress
[16/10/2024 23:50:27] Action: Update, Transition: InProgress -> InProgress
[16/10/2024 23:50:27] Action: Complete, Transition: InProgress -> Completed
[16/10/2024 23:50:27] Action: WorkflowComplete, Transition: Completed -> Validated
[16/10/2024 23:50:27] Action: Update, Transition: Validated -> Validated
[16/10/2024 23:50:27] Action: Update, Transition: Validated -> Validated
---------------------------------------



 
[2024-10-16 23:50:27.680][INF] 
---------------------------------------
entryWillCompleteWorkflowFromIntermediaryState
InitialState=Completed
Workflow=ProgressWithoutValidationWorkFlow
---------------------------------------
 
[2024-10-16 23:50:27.680][INF] ⚡ Update 
[2024-10-16 23:50:27.680][INF] 🔗 Action: Update, Transition: Completed -> Completed 
[2024-10-16 23:50:27.680][INF] 🔗 Action: WorkflowComplete, Transition: Completed -> Validated 
[2024-10-16 23:50:27.680][WRN] ⛔ Complete on state Validated 
[2024-10-16 23:50:27.680][WRN] ⛔ SubmitToManager on state Validated 
[2024-10-16 23:50:27.681][WRN] ⛔ ManagerDeclines on state Validated 
[2024-10-16 23:50:27.681][INF] ⚡ Update 
[2024-10-16 23:50:27.681][INF] 🔗 Action: Update, Transition: Validated -> Validated 
[2024-10-16 23:50:27.681][WRN] ⛔ SubmitToManager on state Validated 
[2024-10-16 23:50:27.681][WRN] ⛔ ManagerValidates on state Validated 
[2024-10-16 23:50:27.681][INF] 
---------------------------------------
Current State: Validated
---------------------------------------
[16/10/2024 23:50:27] Action: Update, Transition: Completed -> Completed
[16/10/2024 23:50:27] Action: WorkflowComplete, Transition: Completed -> Validated
[16/10/2024 23:50:27] Action: Update, Transition: Validated -> Validated
---------------------------------------



 
[2024-10-16 23:50:27.681][INF] --------------------------------------------------------------------------------------------------------------------- 
[2024-10-16 23:50:27.681][INF] 
---------------------------------------
entryWillBeCanceled
InitialState=Undefined
Workflow=EntryWithoutValidationWorkFlow
---------------------------------------
 
[2024-10-16 23:50:27.681][INF] ⚡ Create 
[2024-10-16 23:50:27.681][INF] 🔗 Action: Create, Transition: Undefined -> Completed 
[2024-10-16 23:50:27.681][INF] 🔗 Action: WorkflowComplete, Transition: Completed -> Validated 
[2024-10-16 23:50:27.681][INF] ⚡ Cancel 
[2024-10-16 23:50:27.682][INF] 🔗 Action: Cancel, Transition: Validated -> Canceled 
[2024-10-16 23:50:27.682][INF] 
---------------------------------------
Current State: Canceled
---------------------------------------
[16/10/2024 23:50:27] Action: Create, Transition: Undefined -> Completed
[16/10/2024 23:50:27] Action: WorkflowComplete, Transition: Completed -> Validated
[16/10/2024 23:50:27] Action: Cancel, Transition: Validated -> Canceled
---------------------------------------



 
[2024-10-16 23:50:27.682][INF] 
---------------------------------------
entryWillCompleteWorkflow
InitialState=Undefined
Workflow=EntryWithoutValidationWorkFlow
---------------------------------------
 
[2024-10-16 23:50:27.682][INF] ⚡ Create 
[2024-10-16 23:50:27.682][INF] 🔗 Action: Create, Transition: Undefined -> Completed 
[2024-10-16 23:50:27.682][INF] 🔗 Action: WorkflowComplete, Transition: Completed -> Validated 
[2024-10-16 23:50:27.683][INF] ⚡ Update 
[2024-10-16 23:50:27.683][INF] 🔗 Action: Update, Transition: Validated -> Validated 
[2024-10-16 23:50:27.683][WRN] ⛔ Complete on state Validated 
[2024-10-16 23:50:27.683][INF] ⚡ Update 
[2024-10-16 23:50:27.683][INF] 🔗 Action: Update, Transition: Validated -> Validated 
[2024-10-16 23:50:27.683][WRN] ⛔ SubmitToManager on state Validated 
[2024-10-16 23:50:27.683][WRN] ⛔ ManagerDeclines on state Validated 
[2024-10-16 23:50:27.683][INF] ⚡ Update 
[2024-10-16 23:50:27.683][INF] 🔗 Action: Update, Transition: Validated -> Validated 
[2024-10-16 23:50:27.683][WRN] ⛔ SubmitToManager on state Validated 
[2024-10-16 23:50:27.683][WRN] ⛔ ManagerValidates on state Validated 
[2024-10-16 23:50:27.684][INF] 
---------------------------------------
Current State: Validated
---------------------------------------
[16/10/2024 23:50:27] Action: Create, Transition: Undefined -> Completed
[16/10/2024 23:50:27] Action: WorkflowComplete, Transition: Completed -> Validated
[16/10/2024 23:50:27] Action: Update, Transition: Validated -> Validated
[16/10/2024 23:50:27] Action: Update, Transition: Validated -> Validated
[16/10/2024 23:50:27] Action: Update, Transition: Validated -> Validated
---------------------------------------



 
[2024-10-16 23:50:27.684][INF] 
---------------------------------------
entryWillCompleteWorkflowFromIntermediaryState
InitialState=Completed
Workflow=EntryWithoutValidationWorkFlow
---------------------------------------
 
[2024-10-16 23:50:27.684][INF] ⚡ Update 
[2024-10-16 23:50:27.684][INF] 🔗 Action: Update, Transition: Completed -> Completed 
[2024-10-16 23:50:27.684][INF] 🔗 Action: WorkflowComplete, Transition: Completed -> Validated 
[2024-10-16 23:50:27.684][WRN] ⛔ Complete on state Validated 
[2024-10-16 23:50:27.684][WRN] ⛔ SubmitToManager on state Validated 
[2024-10-16 23:50:27.684][WRN] ⛔ ManagerDeclines on state Validated 
[2024-10-16 23:50:27.684][INF] ⚡ Update 
[2024-10-16 23:50:27.684][INF] 🔗 Action: Update, Transition: Validated -> Validated 
[2024-10-16 23:50:27.684][WRN] ⛔ SubmitToManager on state Validated 
[2024-10-16 23:50:27.684][WRN] ⛔ ManagerValidates on state Validated 
[2024-10-16 23:50:27.684][INF] 
---------------------------------------
Current State: Validated
---------------------------------------
[16/10/2024 23:50:27] Action: Update, Transition: Completed -> Completed
[16/10/2024 23:50:27] Action: WorkflowComplete, Transition: Completed -> Validated
[16/10/2024 23:50:27] Action: Update, Transition: Validated -> Validated
---------------------------------------



 
[2024-10-16 23:50:27.684][INF] --------------------------------------------------------------------------------------------------------------------- 

````