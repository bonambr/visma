using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConsoleTables;
using Newtonsoft.Json;

namespace ConsoleApp3
{

    static class bookList
    {
        public static List<book> jsonList;

    }

    class rentedBook
    {
        public String bookname { get; set; }
        public String tilldate { get; set; }
        public String username { get; set; }
    }

    class book
    {
        public String name { get; set; }
        public String author { get; set; }
        public String language { get; set; }
        public String category { get; set; }
        public String publicationdate { get; set; }
        public String isbn { get; set; }



        public class Program
        {

            public static void printList()
            {

                string jsonString = File.ReadAllText("file.json");
                bookList.jsonList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<book>>(jsonString);

                var table = new ConsoleTable("Name", "Author", "Language", "Category", "publicationDate", "ISBN");
                foreach (var row in bookList.jsonList)
                {
                    table.AddRow(row.name, row.author, row.language, row.category, row.publicationdate, row.isbn);
                }
                table.Write();
                
                Console.WriteLine("Do you want to OrderBy? (Y/N)");
                var command = Console.ReadLine();

                if (command.ToLower().Equals("y"))
                {
                    Console.WriteLine("By which column do you want to OrderBy: Name, Author, Language, Category, publicationDate, ISBN?");
                    command = Console.ReadLine().ToLower();

                    if (command.Equals("name") ||
                        command.Equals("author") ||
                        command.Equals("language") ||
                        command.Equals("category") ||
                        command.Equals("publicationdate") ||
                        command.Equals("isbn"))
                    {
                        List<book> OrderedList = bookList.jsonList.OrderBy(p => p.GetType()
                            .GetProperty(command)
                            .GetValue(p, null)).ToList();

                        table = new ConsoleTable("Name", "Author", "Language", "Category", "publicationDate", "ISBN");
                        foreach (var row in OrderedList)
                        {
                            table.AddRow(row.name, row.author, row.language, row.category, row.publicationdate, row.isbn);
                        }
                        table.Write();
                    }
                    else
                    {
                        Console.WriteLine("Entered command is wrong");
                    }
                }
                
            }
            
            public static void addToTheLibrary()
            {
                Console.Write("Enter name: ");
                var name = Console.ReadLine();
                Console.Write("Enter author: ");
                var author = Console.ReadLine();
                Console.Write("Enter language: ");
                var language = Console.ReadLine();
                Console.Write("Enter category: ");
                var category = Console.ReadLine();
                Console.Write("Enter publicationDate: ");
                var publicationDate = Console.ReadLine();
                Console.Write("Enter ISBN: ");
                var isbn = Console.ReadLine();
                
                List<book> data = new List<book>();
                string jsonString = File.ReadAllText("file.json");
                data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<book>>(jsonString);
                data.Add(new book()
                {
                    name = name,
                    author = author,
                    language = language,
                    category = category,
                    publicationdate = publicationDate,
                    isbn = isbn
                });
                var convertedJson = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText("file.json", convertedJson);

            }
            
            public static void removeFromTheLibrary()
            {
                Console.Write("Enter the book ISBN you want to delete: ");
                var command = Console.ReadLine();
                string jsonString = File.ReadAllText("file.json");
                bookList.jsonList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<book>>(jsonString);
                
                bookList.jsonList.RemoveAll(x => x.isbn == command);
                
                var convertedJson = JsonConvert.SerializeObject(bookList.jsonList, Formatting.Indented);
                File.WriteAllText("file.json", convertedJson);
            
            }
            public static void userBookTaking()
            {
                string jsonString2 = File.ReadAllText("bookRenting.json");
                List<rentedBook> jsonList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<rentedBook>>(jsonString2);
                int rentedBookCount = 0;
                
                Console.Write("Enter your username: ");
                var username = Console.ReadLine();
                foreach (var book in jsonList)
                {
                    if (book.username.ToLower().Equals(username.ToLower()))
                    {
                        ++rentedBookCount;
                    }
                }

                if (rentedBookCount > 3)
                {
                    Console.WriteLine("Maximum number of rented books is reached");
                    return;
                }

                Console.Write("Enter the book name you want to rent: ");
                var command = Console.ReadLine();
                string jsonString = File.ReadAllText("file.json");
                bookList.jsonList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<book>>(jsonString);
                foreach (var book in bookList.jsonList)
                {
                    if (book.name.ToLower().Equals(command.ToLower()))
                    {
                        Console.Write("Till when you want to rent out a book?(MM/DD/YYYY) ");
                        var dateCommand = Console.ReadLine();
                        var endDate = Convert.ToDateTime(dateCommand);
                        double daysApart = (endDate - DateTime.Today ).TotalDays;

                        if (daysApart < 62)
                        {
                            
                            jsonList.Add(new rentedBook()
                            {
                                bookname = command,
                                tilldate = dateCommand,
                                username = username
                            });
                            var convertedJson = JsonConvert.SerializeObject(jsonList, Formatting.Indented);
                            File.WriteAllText("bookRenting.json", convertedJson);
                        }
                    }
                }

            }

            static void Main(string[] args)
            {
                
                Boolean dialogContinuity = true;

                while (dialogContinuity)
                {
                    Console.WriteLine("Choose what you want to do:");
                    Console.WriteLine("1. list,");
                    Console.WriteLine("2. add,");
                    Console.WriteLine("3. remove,");
                    Console.WriteLine("4. take,");
                    Console.WriteLine("5. return.");
                    Console.Write("Enter single command of what you want to do: ");
                    var command = Console.ReadLine();
                    
                    Boolean answerCheck = false;

                    while (!answerCheck)
                    {
                        if (command.ToLower().Equals("list"))
                        {
                            printList();
                            answerCheck = true;
                        }
                        else if (command.ToLower().Equals("add"))
                        {
                            addToTheLibrary();
                            answerCheck = true;
                        }
                        else if (command.ToLower().Equals("remove"))
                        {
                            removeFromTheLibrary();
                            answerCheck = true;
                        }
                        else if (command.ToLower().Equals("take"))
                        {
                            userBookTaking();
                            answerCheck = true;
                        }
                        else
                        {
                            Console.WriteLine("Enter the right command:");
                            answerCheck = false;
                        }
                    }

                }

                
                Console.Write("Baigta programa");

            }
        }
    }
}
