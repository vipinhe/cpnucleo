﻿namespace Cpnucleo.Shared.Queries.RecursoTarefa;

public sealed record GetRecursoTarefaByTarefaViewModel : BaseQuery
{
    public IEnumerable<RecursoTarefaDTO> RecursoTarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
