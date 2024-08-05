using Microsoft.AspNetCore.Mvc;
using TD.Areas.Identity.Data; 
using TD.Models; 
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TD.Interfaces;

namespace TD.Controllers
{
    public class HomeController : Controller
    {
        private readonly IToDoItemRepository _repository;

        public HomeController(IToDoItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var items = await _repository.GetAllAsync(User.Identity.Name);
            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(x => x.Title.Contains(searchString));
            }
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ToDoItem item)
        {
            if (ModelState.IsValid)
            {
                item.UserId = User.Identity.Name; 
                await _repository.AddAsync(item);
                return RedirectToAction(nameof(Index));
            }
            return View("Index", await _repository.GetAllAsync(User.Identity.Name));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Complete(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item != null)
            {
                item.IsCompleted = true;
                await _repository.UpdateAsync(item);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
