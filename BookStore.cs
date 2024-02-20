using System;
using System.Collections.Generic;
using System.Linq;

namespace BookStore
{
    class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
    }

    class Program
    {
        static List<Book> books = new List<Book>();

        static void Main(string[] args)
        {
            SeedData();

            Console.WriteLine("Добро пожаловать в книжный магазин!");
            Console.WriteLine("Введите 'help' для получения списка команд.");

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;

                string[] commands = input.Split("--");
                List<string> options = commands.Skip(1).ToList();

                if (options.Count > 0)
                {
                    foreach (var option in options)
                    {
                        ProcessOption(option);
                    }
                }
                else
                {
                    string action = commands[0].Trim().ToLower();
                    switch (action)
                    {
                        case "help":
                            DisplayHelp();
                            break;
                        case "get":
                            DisplayBooks(books);
                            break;
                        case "buy":
                            if (commands.Length > 1)
                            {
                                BuyBook(commands[1]);
                            }
                            else
                            {
                                Console.WriteLine("Необходимо указать ID книги для покупки.");
                            }
                            break;
                        default:
                            Console.WriteLine("Неверная команда. Введите 'help' для получения списка команд.");
                            break;
                    }
                }
            }
        }

        static void SeedData()
        {
            books.Add(new Book { Id = 1, Title = "Война и мир", Author = "Толстой", Year = 1869 });
            books.Add(new Book { Id = 2, Title = "Преступление и наказание", Author = "Достоевский", Year = 1866 });
            books.Add(new Book { Id = 3, Title = "1984", Author = "Оруэлл", Year = 1949 });
            books.Add(new Book { Id = 4, Title = "Унесенные ветром", Author = "Митчелл", Year = 1936 });
            books.Add(new Book { Id = 5, Title = "Отцы и дети", Author = "Тургенев", Year = 1862 });
            books.Add(new Book { Id = 6, Title = "Муму", Author = "Тургенев", Year = 1854 });
            books.Add(new Book { Id = 7, Title = "Смерть Ивана Ильича", Author = "Толстой", Year = 1886 });
            books.Add(new Book { Id = 8, Title = "Евгений Онегин", Author = "Пушкин", Year = 1833 });
        }

        static void ProcessOption(string option)
        {
            string[] parts = option.Split('=');
            if (parts.Length != 2)
            {
                Console.WriteLine("Неверный формат флага.");
                return;
            }

            string key = parts[0].Trim().ToLower();
            string value = parts[1].Trim('\"');

            switch (key)
            {
                case "author":
                    DisplayBooks(books.FindAll(book => book.Author.ToLower().Contains(value.ToLower())));
                    break;
                case "title":
                    DisplayBooks(books.FindAll(book => book.Title.ToLower().Contains(value.ToLower())));
                    break;
                case "date":
                    if (int.TryParse(value, out int year))
                    {
                        DisplayBooks(books.FindAll(book => book.Year == year));
                    }
                    else
                    {
                        Console.WriteLine("Неверный формат года. Пожалуйста, введите целое число.");
                    }
                    break;
                case "order-by":
                    if (value.ToLower() == "date")
                    {
                        DisplayBooks(books.OrderBy(book => book.Year).ToList());
                    }
                    else
                    {
                        Console.WriteLine("Неверное значение для сортировки.");
                    }
                    break;
                case "id":
                    BuyBook(value);
                    break;
                default:
                    Console.WriteLine($"Неизвестный флаг: {key}");
                    break;
            }
        }

        static void DisplayHelp()
        {
            Console.WriteLine("Список команд:");
            Console.WriteLine("  get                              - получить все книги");
            Console.WriteLine("  --title=%%                       - поиск книги по названию");
            Console.WriteLine("  --author=%%                      - поиск книги по автору");
            Console.WriteLine("  --date=%%                        - поиск книги по году издания");
            Console.WriteLine("  --order-by=date                  - сортировка книг по дате издания");
            Console.WriteLine("  buy --id=%%                      - купить книгу по ID");
            Console.WriteLine("  help                             - отображение этого списка команд");
        }

        static void DisplayBooks(List<Book> books)
        {
            if (books.Count == 0)
            {
                Console.WriteLine("Книги не найдены.");
                return;
            }

            foreach (var book in books)
            {
                Console.WriteLine($"ID: {book.Id}, Название: {book.Title}, Автор: {book.Author}, Год издания: {book.Year}");
            }
        }

        static void BuyBook(string id)
        {
            if (!int.TryParse(id, out int bookId))
            {
                Console.WriteLine("Неверный формат ID книги.");
                return;
            }

            Book bookToRemove = books.Find(book => book.Id == bookId);
            if (bookToRemove != null)
            {
                books.Remove(bookToRemove);
                Console.WriteLine($"Книга '{bookToRemove.Title}' успешно куплена.");
            }
            else
            {
                Console.WriteLine($"Книга с ID {bookId} не найдена.");
            }
        }
    }
}
