﻿namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.UpdateWorkflow
{
    [DataContract]
    public class UpdateWorkflowCommand
    {
        [DataMember(Order = 1)]
        public WorkflowViewModel Workflow { get; set; }
    }
}
