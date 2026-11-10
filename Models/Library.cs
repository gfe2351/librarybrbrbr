using System;
using System.Collections.Generic;
using System.Linq;

namespace NationalLibraryApp.Models
{
    public class Library
    {
        public static Library Instance { get; } = new Library();

        // Задача 15: Статический счётчик
        public static int TotalBooks { get; private set; } = 0;

        private int _nextReaderId = 1;

        public List<Book> Books { get; set; } = new List<Book>();
        public List<Reader> Readers { get; set; } = new List<Reader>();

        public Library()
        {
            // Начальное наполнение
            AddBook("Чистый код", "Роберт Мартин", 2008);
            AddBook("Совершенный код", "Стив Макконнелл", 2004);
            AddBook("Паттерны проектирования", "Эрик Фримен", 2020);

            AddReader(new Reader
            {
                FullName = "Алексей Иванов",
                DateOfBirth = new DateTime(2001, 5, 20),
                Institution = "Казахский национальный университет им. аль-Фараби",
                InstitutionType = InstitutionType.Study,
                IIN = "010520501234",
                Email = "alexey.ivanov@example.com",
                Phone = "+77011112233",
                Gender = Gender.Male
            });
        }

        // Задача 2: Приветственное сообщение
        public string ShowWelcomeMessage()
        {
            return "Добро пожаловать в Национальную Библиотеку!";
        }

        // Задача 11: Статический метод названия библиотеки
        public static string ShowLibraryName()
        {
            return "National Library";
        }

        // Задача 4 & 15: Добавление книги и увеличение счётчика
        public void AddBook(string title, string author, int year)
        {
            Books.Add(new Book(title, author, year));
            TotalBooks++;
        }

        // Задача 6: Количество книг
        public int GetBookCount()
        {
            return Books.Count;
        }

        // Задача 12: Вывод списка книг
        public List<Book> ShowBooks()
        {
            return Books;
        }

        // Задача 10: Проверка читателя
        public bool IsReaderRegistered(string name)
        {
            return Readers.Any(r => r.FullName.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        // Задача 13: Статический расчёт штрафа
        public static decimal CalculateFine(int overdueDays)
        {
            return overdueDays > 0 ? overdueDays * 100 : 0;
        }

        // Задача 14: Сравнение и доступные книги
        public static int GetMaxBooks(int first, int second) => Math.Max(first, second);
        public int GetAvailableBooksCount() => Books.Count(b => b.IsAvailable);

        // Задача 16: Поиск книг
        public List<Book> SearchBook(string title) => Books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
        public List<Book> SearchBookByAuthor(string author) => Books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)).ToList();
        public List<Book> SearchBookByYear(int year) => Books.Where(b => b.Year == year).ToList();

        // Задача 17: Добавление читателя (полный профиль)
        public (bool Success, string Message) AddReader(Reader reader)
        {
            if (Readers.Any(r => r.IIN == reader.IIN))
                return (false, "Читатель с таким ИИН уже зарегистрирован.");

            reader.Id = _nextReaderId++;
            Readers.Add(reader);
            return (true, $"Читатель «{reader.FullName}» успешно зарегистрирован!");
        }

        public List<Reader> ShowReaders() => Readers;

        public Reader? GetReaderById(int id) => Readers.FirstOrDefault(r => r.Id == id);

        public (bool Success, string Message) UpdateReader(Reader reader)
        {
            var existing = Readers.FirstOrDefault(r => r.Id == reader.Id);
            if (existing == null) return (false, "Читатель не найден.");

            if (Readers.Any(r => r.IIN == reader.IIN && r.Id != reader.Id))
                return (false, "Читатель с таким ИИН уже зарегистрирован.");

            existing.FullName = reader.FullName;
            existing.DateOfBirth = reader.DateOfBirth;
            existing.Institution = reader.Institution;
            existing.InstitutionType = reader.InstitutionType;
            existing.IIN = reader.IIN;
            existing.Email = reader.Email;
            existing.Phone = reader.Phone;
            existing.Gender = reader.Gender;

            return (true, $"Данные читателя «{reader.FullName}» обновлены.");
        }

        // Задача 18 & 22: Возврат книги и расчёт штрафа
        public (bool Success, string Message) ReturnBook(string title, int overdueDays = 0)
        {
            var book = Books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (book == null) return (false, "Книга не найдена.");
            if (book.IsAvailable) return (false, "Книга уже в библиотеке.");

            book.IsAvailable = true;
            decimal fine = CalculateFine(overdueDays);
            string fineMsg = fine > 0 ? $" Начислен штраф: {fine} ₸ за {overdueDays} дн. просрочки." : "";
            return (true, $"Книга «{book.Title}» успешно возвращена!{fineMsg}");
        }

        // Задача 19: Рекурсивный подсчёт книг
        public int CountBooksRecursive(int index = 0)
        {
            if (index >= Books.Count) return 0;
            return 1 + CountBooksRecursive(index + 1);
        }

        // Задача 20: Сортировка по году
        public List<Book> SortBooksByYear()
        {
            return Books.OrderBy(b => b.Year).ToList();
        }

        // Задача 21: Выдача книги
        public (bool Success, string Message) BorrowBook(string title, string readerName)
        {
            if (!IsReaderRegistered(readerName))
                return (false, $"Читатель '{readerName}' не зарегистрирован в системе!");

            var book = Books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
            if (book == null) return (false, "Книга не найдена.");
            if (!book.IsAvailable) return (false, "Книга уже выдана другим читателям.");

            book.IsAvailable = false;
            return (true, $"Книга «{book.Title}» успешно выдана читателю {readerName}.");
        }

       // Задача 23: Статистика
public LibraryStatistics ShowStatistics()
{
    int available = Books.Count(b => b.IsAvailable);
    int borrowed = Books.Count(b => !b.IsAvailable);
    
    return new LibraryStatistics
    {
        Total = Books.Count,
        Available = available,
        Borrowed = borrowed,
        ReadersCount = Readers.Count
    };
}
    }
}
