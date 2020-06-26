using Firewatch.Application.Common.Mappings;
using Firewatch.Domain.Entities;

namespace Firewatch.Application.TodoLists.Queries.ExportTodos
{
    public class TodoItemRecord : IMapFrom<TodoItem>
    {
        public string Title { get; set; }

        public bool Done { get; set; }
    }
}
