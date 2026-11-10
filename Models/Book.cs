using System;

namespace NationalLibraryApp.Models
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public bool IsAvailable { get; set; } = true; // Флаг доступности (Задача 7)

        public Book(string title, string author, int year)
        {
            Title = title;
            Author = author;
            Year = year;
            IsAvailable = true;
        }

        // Задача 1: Вывод информации о книге
        public string DisplayInfo()
        {
            string status = IsAvailable ? "Доступна" : "Выдана";
            return $"«{Title}» — {Author} ({Year} г.) [{status}]";
        }

        // Задача 3: Изменение названия
        public void SetTitle(string title)
        {
            Title = title;
        }

        // Задача 8: Расчёт возраста книги
        public int CalculateBookAge()
        {
            return DateTime.Now.Year - Year;
        }
    }
}
