using DemoDapper.Dtos;
using DTO.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DemoDapper.EntityFramwork
{
    public class EntityFrameworkCore
    {
        public (int count, long time) GetProducts()
        {
            int count = 0;
            Stopwatch watch = new Stopwatch();
            
            using (ContextEfCore context = new ContextEfCore(Database.GetOptions()))
            {
                watch.Start();
                var data = context.Products.AsNoTracking();
                count = data.Count();
            }
            watch.Stop();
            var time = watch.ElapsedMilliseconds;

            return (count, time);
        }

        public void GetAuthors()
        {
           // var watch = Stopwatch.StartNew();
            using (ContextEfCore context = new ContextEfCore(Database.GetOptions()))
            {
                var data = context.Authors.Include(x => x.Books).AsNoTracking().ToList();

               // watch.Stop();
               // var time = watch.ElapsedMilliseconds;
            }
        }

        public List<Author> GetAuthors_Return()
        {
            using (ContextEfCore context = new ContextEfCore(Database.GetOptions()))
            {
                var data = context.Authors.AsNoTracking().ToList();
                return data;
            }
        }

        public void UpdateProduct()
        {
            using (ContextEfCore context = new ContextEfCore(Database.GetOptions()))
            {
                var item = context.Products.FirstOrDefault();

                context.Update(item);
                context.SaveChanges();
            }

        }

        public List<Author> GetAuthorsWithoutBooks()
        {
            var list = new List<Author>();
            using (ContextEfCore context = new ContextEfCore(Database.GetOptions()))
            {
                list = context.Authors.ToList();
            }

            return list;
        }

        public long InsertAuthors()
        {
            Stopwatch watch = new Stopwatch();

            var list = new List<Author>();
            for (int i = 1; i <= 2000; i++)
            {
                list.Add(new Author
                {
                    Name = "ET BulkInsert Author1" + i,
                    Books = new List<Book> {
                        new Book { Name = "ET BulkInsert Book " + i },
                        new Book { Name = "ET BulkInsert Book" + i }
                    }
                });
            }

            using (ContextEfCore context = new ContextEfCore(Database.GetOptions()))
            {
                context.Authors.AddRange(list);

                watch.Start();
                context.SaveChanges();
            }

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public long InsertProducts()
        {
            Stopwatch watch = new Stopwatch();

            var list = new List<Product>();
            for (int i = 1; i <= 2; i++)
            {
                list.Add(new Product
                {
                    Name = "ET BulkInsert Product" + i,
                    Description = "ET BulkInsert Product" + i
                });
            }

            using (ContextEfCore context = new ContextEfCore(Database.GetOptions()))
            {
                context.Products.AddRange(list);

                watch.Start();
                context.SaveChanges();
            }

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public void InsertProducts_BulkInsert()
        {
            var list = new List<Product>();
            for (int i = 1; i <= 2000; i++)
            {
                list.Add(new Product
                {
                    Name = "ET BulkInsert Product" + i,
                    Description = "ET BulkInsert Product" + i
                });
            }

            using (ContextEfCore context = new ContextEfCore(Database.GetOptions()))
            {
                context.BulkInsert(list);
            }
        }

        public void InsertTestPerformance_BulkInsert()
        {
            var list = new List<Test_Performance>();
            for (int i = 1; i <= 2000; i++)
            {
                list.Add(new Test_Performance
                {
                    Id = Guid.NewGuid(),
                    Name = "dapper Test_Performance" + i,
                    Description = "dapper Test_Performance" + i,
                    Type = "test",
                    HasChildren = true
                });
            }

            using (ContextEfCore context = new ContextEfCore(Database.GetOptions()))
            {
                context.BulkInsert(list);
            }
        }

        public void Authors_BulkUpdate()
        {
            using (ContextEfCore context = new ContextEfCore(Database.GetOptions()))
            {
                var authors = context.Authors.Include(x=> x.Books).Take(10).ToList();

                foreach (var item in authors)
                {
                    foreach (var child in item.Books)
                    {
                        child.Name = "changeName";
                    }
                }

                context.BulkUpdate(authors);
            }
        }

        public void Products_BulkUpdate()
        {
            using (ContextEfCore context = new ContextEfCore(Database.GetOptions()))
            {
                var products = context.Products.ToList();
                context.BulkUpdate(products);
            }
        }

        public void Authors_BulkUpdate(List<Author> authors)
        {
            using (ContextEfCore context = new ContextEfCore(Database.GetOptions()))
            {
                context.BulkUpdate(authors);
            }
        }

        public void Products_BulkDelete()
        {
            using (ContextEfCore context = new ContextEfCore(Database.GetOptions()))
            {
                var products = context.Products.ToList();
                context.BulkDelete(products);
            }
        }

        public void Products_BulkMerge()
        {
            using (ContextEfCore context = new ContextEfCore(Database.GetOptions()))
            {
                var products = context.Products.Take(10).ToList();
                foreach (var item in products)
                {
                    item.Name = "Hungd";
                }
                products.Add(new Product { Description = "Merge test1", Name = "Merge test1" });

                context.BulkMerge(products);
            }
        }
    }
}
