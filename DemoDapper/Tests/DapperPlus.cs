using DemoDapper.Dtos;
using DemoDapper.Infarstructure;
using DTO.Dtos;
using System;
using System.Collections.Generic;
using Z.Dapper.Plus;
using Dapper;
using System.Linq;

namespace DemoDapper.Tests
{
    public class DapperPlus
    {
        public void BulkInsert_OneToOne_Product()
        {
            DapperPlusManager.Entity<Product>().Table("Products").Identity(x => x.Id);
            DapperPlusManager.Entity<ProductDetail>().Table("ProductDetail").Identity(x => x.Id);

            var listData = new List<Product> { new Product { Name = "Bulk Name1", Description = "Bulk Description1", 
                ProductDetail = new ProductDetail { Name = "ProductDetail1"} } };

            using (var connection = BaseConnection.CreateConnection())
            {
                connection.BulkInsert(listData).ThenForEach(x => x.ProductDetail.ProductId = x.Id).ThenBulkInsert(x => x.ProductDetail);
            }
        }

        public void BulkInsert_OneToMany_Author()
        {
            DapperPlusManager.Entity<Author>().Table("Author").Identity(x => x.Id);
            DapperPlusManager.Entity<Book>().Table("Book").Identity(x => x.Id);

            var listData = new List<Author> { new Author { Name = "BulkInsert Author1",
                Books = new List<Book> {
                    new Book {Name = "BulkInsert Book 1" },
                    new Book {Name = "BulkInsert Book 2" }
                }
            }};

            using (var connection = BaseConnection.CreateConnection())
            {
                var result = connection.BulkInsert(listData).ThenForEach((x) => {
                    foreach (var item in x.Books)
                    {
                        item.AuthorId = x.Id;
                    }
                }).ThenBulkInsert(x => x.Books);
            }
        }

        public void BulkInsert_ManyProduct_Performance_WithIdentity()
        {
            DapperPlusManager.Entity<Product>().Table("Products").Identity(x => x.Id);

            var list = new List<Product>();
            for (int i = 1; i <= 2000; i++)
            {
                list.Add(new Product
                {
                    Name = "dapper BulkInsert Product1" + i,
                    Description = "dapper BulkInsert Product1" + i,
                });
            }

            using (var connection = BaseConnection.CreateConnection())
            {
              var result =  connection.BulkInsert(list);
            }
        }
        public void BulkInsert_TestPerformance()
        {
            //DapperPlusManager.Entity<Author>().Table("Test_Performance");

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

            using (var connection = BaseConnection.CreateConnection())
            {
                var result = connection.BulkInsert(list);
            }
        }


        //If you have column Identity, and when you save, you don't need to save relationship object , so, you should set AutoMapOutputDirection = false to improve performance
        //Because normal default is true , it will create temp table contain the column Identity
        public void BulkInsert_ManyAuthor_WIthNoReturnIdentity()
        {
            DapperPlusManager.Entity<Author>().Table("Author").Identity(x => x.Id);

            var listData = new List<Author> { new Author { Name = "BulkInsert Author2"
            }};

            using (var connection = BaseConnection.CreateConnection())
            {
               var result = connection.UseBulkOptions(options => options.AutoMapOutputDirection = false).BulkInsert(listData);
            }
        }

        // based on the primary key to check the item is exist or not, and then it will apply insert or update the info
        public void BulkMerge_Author(int id)
        {
            DapperPlusManager.Entity<Author>().Table("Author").Identity(x => x.Id);

            var listData = new List<Author> { new Author { Id = id,  Name = "BulkInsert Author5"
            }};

            using (var connection = BaseConnection.CreateConnection())
            {
                var result = connection.BulkMerge(listData);
            }
        }

        public void BulkUpdate_OneToMany_Author(List<Author> authors)
        {
            DapperPlusManager.Entity<Author>().Table("Author").Identity(x => x.Id);
            DapperPlusManager.Entity<Book>().Table("Book").Identity(x => x.Id);

            using (var connection = BaseConnection.CreateConnection())
            {
                var result = connection.BulkUpdate<Author>(authors, x => x.Books);
            }
        }

        public void BulkUpdate_ManyAuthor(List<Author> authors)
        {
            using (var connection = BaseConnection.CreateConnection())
            {
                connection.BulkUpdate<Author>(authors);
            }
        }

        public void BulkDeleteOneToMany()
        {
            DapperPlusManager.Entity<Author>().Table("Author").Identity(x => x.Id);
            DapperPlusManager.Entity<Book>().Table("Book").Identity(x => x.Id);


            var authors = new MultiMapping().MappingWithOneToMany_Author_Book_Return();
            using (var connection = BaseConnection.CreateConnection())
            {
                connection.BulkDelete(authors.SelectMany(x=> x.Books)).BulkDelete(authors);
            }
        }

        public void InitData()
        {
            DapperPlusManager.Entity<Product>().Table("Products").Identity(x => x.Id);

            DapperPlusManager.Entity<Author>().Table("Author").Identity(x => x.Id);
            DapperPlusManager.Entity<Book>().Table("Book").Identity(x => x.Id);

            //Delete Data
            var query = "delete from productdetail; delete from products;delete from book;delete from author";
            var affectRows = BaseConnection.CreateConnection().Execute(query, commandType: System.Data.CommandType.Text);

            var listProduct = new List<Product>();
            var listAuthor = new List<Author>();

            for (int i = 1; i <= 2000; i++)
            {
                listProduct.Add(new Product
                {
                    Name = "Init Product" + i,
                    Description = "Init Product" + i,
                    ProductDetail = new ProductDetail { Name = "Init ProductDetail" + i }
                });

                listAuthor.Add(new Author
                {
                    Name = "Init Author" + i,
                    Books = new List<Book>() { new Book { Name = "Init Book" + i}, new Book { Name = "Init Book1"+ i } }
                });
            }

            using (var connection = BaseConnection.CreateConnection())
            {
                //product
                var resultProduct = connection.BulkInsert(listProduct).ThenForEach(x=> {
                    x.ProductDetail.ProductId = x.Id;
                }).ThenBulkInsert(x=>x.ProductDetail);

                //author
                var resultAuthor = connection.BulkInsert(listAuthor).ThenForEach(x => {
                    foreach (var item in x.Books)
                    {
                        item.AuthorId = x.Id;
                    }
                }).ThenBulkInsert(x => x.Books);
            }
        }
    }
}
