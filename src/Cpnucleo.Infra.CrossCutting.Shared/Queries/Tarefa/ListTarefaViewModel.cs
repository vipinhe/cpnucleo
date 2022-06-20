﻿namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Tarefa;

public record ListTarefaViewModel : BaseQuery
{
    public IEnumerable<TarefaDTO> Tarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
