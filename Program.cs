﻿using System;
using NLog.Web;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
                    Console.WriteLine("5) Delete Blog");
                    Console.WriteLine("6) Edit Blog");
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
                        var db = new BloggingContext();
                        Blog blog = InputBlog(db);
                        if (blog != null)
                        {
                            //blog.BlogId = BlogId;
                            db.AddBlog(blog);
                            logger.Info("Blog added - {name}", blog.Name);
                        }
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
                    else if (choice == "5")
                    {
                        // delete blog
                        Console.WriteLine("Choose the blog to delete:");
                        var db = new BloggingContext();
                        var blog = GetBlog(db);
                        if (blog != null)
                        {
                            // delete blog
                            db.DeleteBlog(blog);
                            logger.Info($"Blog (id: {blog.BlogId}) deleted");
                        }
                    }
                    else if (choice == "6")
                    {
                        // edit blog
                        Console.WriteLine("Choose the blog to edit:");
                        var db = new BloggingContext();
                        var blog = GetBlog(db);
                        if (blog != null)
                        {
                            // TODO: input blog
                        }
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

        public static Blog GetBlog(BloggingContext db)
        {
            // display all blogs
            var blogs = db.Blogs.OrderBy(b => b.BlogId);
            foreach (Blog b in blogs)
            {
                Console.WriteLine($"{b.BlogId}: {b.Name}");
            }
            if (int.TryParse(Console.ReadLine(), out int BlogId))
            {
                Blog blog = db.Blogs.FirstOrDefault(b => b.BlogId == BlogId);
                if (blog != null)
                {
                    return blog;
                }
            }
            logger.Error("Invalid Blog Id");
            return null;
        }

        public static Blog InputBlog(BloggingContext db)
        {
            Blog blog = new Blog();
            Console.WriteLine("Enter the Blog name");
            blog.Name = Console.ReadLine();

            ValidationContext context = new ValidationContext(blog, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(blog, context, results, true);
            if (isValid)
            {
                return blog;
            }
            else
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
            return null;
        }

    }
}
