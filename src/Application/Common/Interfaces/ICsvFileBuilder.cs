using Firewatch.Application.TodoLists.Queries.ExportTodos;
using System.Collections.Generic;

namespace Firewatch.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
    }
}
