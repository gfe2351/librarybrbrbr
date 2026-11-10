using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using NationalLibraryApp.Models;

namespace NationalLibraryApp.Controllers
{
    public class ReaderController : Controller
    {
        private static readonly Library _library = Library.Instance;

        public IActionResult Index()
        {
            ViewBag.Institutions = InstitutionCatalog.Items;
            ViewBag.Readers = _library.ShowReaders();
            return View();
        }

        [HttpPost]
        public IActionResult Register(Reader reader, string customInstitution)
        {
            reader.Institution = ResolveInstitution(reader.Institution, customInstitution);

            var error = ValidateReader(reader);
            if (error != null)
            {
                SetMessage(error, true);
                return RedirectToAction(nameof(Index));
            }

            var result = _library.AddReader(reader);
            SetMessage(result.Message, !result.Success);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Profile(int id)
        {
            var reader = _library.GetReaderById(id);
            if (reader == null)
            {
                SetMessage("Читатель не найден.", true);
                return RedirectToAction(nameof(Index));
            }

            return View(reader);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var reader = _library.GetReaderById(id);
            if (reader == null)
            {
                SetMessage("Читатель не найден.", true);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Institutions = InstitutionCatalog.Items;
            return View(reader);
        }

        [HttpPost]
        public IActionResult Edit(Reader reader, string customInstitution)
        {
            reader.Institution = ResolveInstitution(reader.Institution, customInstitution);

            var error = ValidateReader(reader);
            if (error != null)
            {
                SetMessage(error, true);
                return RedirectToAction(nameof(Edit), new { id = reader.Id });
            }

            var result = _library.UpdateReader(reader);
            SetMessage(result.Message, !result.Success);
            return RedirectToAction(nameof(Profile), new { id = reader.Id });
        }

        private static string ResolveInstitution(string institution, string customInstitution)
        {
            if (institution == "custom")
                return customInstitution?.Trim() ?? "";

            return institution?.Trim() ?? "";
        }

        private static string? ValidateReader(Reader reader)
        {
            if (string.IsNullOrWhiteSpace(reader.FullName))
                return "Укажите ФИО читателя.";

            if (reader.DateOfBirth == default)
                return "Укажите дату рождения.";

            if (string.IsNullOrWhiteSpace(reader.Institution))
                return "Укажите место учёбы/работы.";

            if (!Regex.IsMatch(reader.IIN ?? "", @"^\d{12}$"))
                return "ИИН должен состоять из 12 цифр.";

            if (string.IsNullOrWhiteSpace(reader.Email) || !System.Net.Mail.MailAddress.TryCreate(reader.Email, out _))
                return "Укажите корректный email.";

            if (string.IsNullOrWhiteSpace(reader.Phone))
                return "Укажите номер телефона.";

            return null;
        }

        private void SetMessage(string message, bool isError)
        {
            TempData["Message"] = message;
            TempData["IsError"] = isError;
        }
    }
}
