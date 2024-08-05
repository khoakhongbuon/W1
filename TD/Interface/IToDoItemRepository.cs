using System.Collections.Generic;
using System.Threading.Tasks;
using TD.Models;

namespace TD.Interfaces
{
    public interface IToDoItemRepository
    {
        Task<IEnumerable<ToDoItem>> GetAllAsync(string userId);
        Task<ToDoItem> GetByIdAsync(int id);
        Task AddAsync(ToDoItem item);
        Task UpdateAsync(ToDoItem item);
        Task DeleteAsync(int id);
    }
}
