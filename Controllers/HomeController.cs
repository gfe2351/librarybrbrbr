using Microsoft.AspNetCore.Mvc;
using NationalLibraryApp.Models;

namespace NationalLibraryApp.Controllers
{
    public class HomeController : Controller
    {
        private static readonly Library _library = Library.Instance;

        public IActionResult Index(string searchTitle, string message, bool isError = false)
        {
            ViewBag.Message = message;
            ViewBag.IsError = isError;
            ViewBag.Welcome = _library.ShowWelcomeMessage();
            ViewBag.LibraryName = Library.ShowLibraryName();
            ViewBag.Stats = _library.ShowStatistics();

            var books = string.IsNullOrEmpty(searchTitle) 
                ? _library.ShowBooks() 
                : _library.SearchBook(searchTitle);

            return View(books);
        }

        [HttpPost]
        public IActionResult AddBook(string title, string author, int year)
        {
            if (!string.IsNullOrEmpty(title) && !string.IsNullOrEmpty(author) && year > 0)
            {
                _library.AddBook(title, author, year);
                return RedirectToAction("Index", new { message = $"Книга «{title}» успешно добавлена!" });
            }
            return RedirectToAction("Index", new { message = "Заполните корректно все поля книги.", isError = true });
        }

        [HttpPost]
        public IActionResult BorrowBook(string title, string readerName)
        {
            var res = _library.BorrowBook(title, readerName);
            return RedirectToAction("Index", new { message = res.Message, isError = !res.Success });
        }

        [HttpPost]
        public IActionResult ReturnBook(string title, int overdueDays)
        {
            var res = _library.ReturnBook(title, overdueDays);
            return RedirectToAction("Index", new { message = res.Message, isError = !res.Success });
        }

        public IActionResult SortByYear()
        {
            var sorted = _library.SortBooksByYear();
            ViewBag.Welcome = _library.ShowWelcomeMessage();
            ViewBag.LibraryName = Library.ShowLibraryName();
            ViewBag.Stats = _library.ShowStatistics();
            ViewBag.Message = "Книги отсортированы по году издания.";
            return View("Index", sorted);
        }
    }
}
