using Microsoft.EntityFrameworkCore;
using TD.Interfaces;
using TD.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using TD.Areas.Identity.Data;

namespace TD.Repositories
{
    public class ToDoItemRepository : IToDoItemRepository
    {
        private readonly TDContext _context;

        public ToDoItemRepository(TDContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ToDoItem>> GetAllAsync(string userId)
        {
            return await _context.ToDoItems
                .Where(item => item.UserId == userId)
                .ToListAsync();
        }

        public async Task<ToDoItem> GetByIdAsync(int id)
        {
            return await _context.ToDoItems.FindAsync(id);
        }

        public async Task AddAsync(ToDoItem item)
        {
            await _context.ToDoItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ToDoItem item)
        {
            _context.ToDoItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.ToDoItems.FindAsync(id);
            if (item != null)
            {
                _context.ToDoItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
