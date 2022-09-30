﻿namespace Cpnucleo.Shared.Queries.Workflow;

public sealed record ListWorkflowViewModel : BaseQuery
{
    public IEnumerable<WorkflowDTO> Workflows { get; set; }
    public OperationResult OperationResult { get; set; }
}
