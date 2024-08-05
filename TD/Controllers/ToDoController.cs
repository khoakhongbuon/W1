using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TD.Interfaces;
using TD.Models;
using System.Threading.Tasks;
using TD.Areas.Identity.Data;

namespace TD.Controllers
{
    [Authorize]
    public class ToDoController : Controller
    {
        private readonly IToDoItemRepository _toDoItemRepository;
        private readonly UserManager<TDUser> _userManager;

        public ToDoController(IToDoItemRepository toDoItemRepository, UserManager<TDUser> userManager)
        {
            _toDoItemRepository = toDoItemRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var userId = _userManager.GetUserId(User);
            var items = await _toDoItemRepository.GetAllAsync(userId);

            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Title.Contains(searchString)).ToList();
            }

            ViewData["CurrentFilter"] = searchString;

            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ToDoItem item)
        {
            if (ModelState.IsValid)
            {
                item.UserId = _userManager.GetUserId(User);
                await _toDoItemRepository.AddAsync(item);
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _toDoItemRepository.GetByIdAsync(id);
            if (item == null || item.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ToDoItem item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _toDoItemRepository.UpdateAsync(item);
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _toDoItemRepository.GetByIdAsync(id);
            if (item == null || item.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _toDoItemRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
