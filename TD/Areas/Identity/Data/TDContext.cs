using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TD.Areas.Identity.Data;
using TD.Models;

namespace TD.Areas.Identity.Data
{
    public class TDContext : DbContext
    {
        public TDContext(DbContextOptions<TDContext> options)
            : base(options)
        {
        }

        
        public DbSet<ToDoItem> ToDoItems { get; set; }
    }
}
