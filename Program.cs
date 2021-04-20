using System;
using NLog.Web;
using System.IO;
using System.Linq;

namespace BlogsConsole
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");

            try
            {
                string choice;
                string secondChoice;
                do
                {
                    Console.WriteLine("Enter your selection:");
                    Console.WriteLine("1) Display all blogs");
                    Console.WriteLine("2) Add Blog");
                    Console.WriteLine("3) Create Post");
                    Console.WriteLine("4) Display Posts");
                    Console.WriteLine("Enter q to quit");
                    choice = Console.ReadLine();
                    Console.Clear();
                    logger.Info("Option {choice} selected", choice);

                    if (choice == "1")
                    {
                        // display blogs
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.Name);

                        Console.WriteLine($"{query.Count()} Blogs returned");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.Name);
                        }
                    }
                    else if (choice == "2")
                    {
                        // Add blog
                        Console.Write("Enter a name for a new Blog: ");
                        var blog = new Blog { Name = Console.ReadLine() };

                        var db = new BloggingContext();
                        db.AddBlog(blog);
                        logger.Info("Blog added - {name}", blog.Name);
                    }
                    else if (choice == "3")
                    {
                        Console.WriteLine("Select the blog you would post to:  ");
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.Name);

                        foreach (var item in query)
                        {
                            Console.WriteLine(item.BlogId + ") " + item.Name);
                        }

                        secondChoice = Console.ReadLine();

                        
                       // Console.WriteLine("Enter name of post");
                       // var post = new Post { Title = Console.ReadLine()};
                       // Console.WriteLine("Enter the Content");
                        
                    }
                    else if (choice == "4")
                    {
                        
                    }
                    Console.WriteLine();
                } while (choice.ToLower() != "q");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            logger.Info("Program ended");
        }
    }
}
