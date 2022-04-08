using System;
using System.Collections.Generic;
using System.Text;

namespace DemoDapper.Dtos
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Book")]
    [Dapper.Contrib.Extensions.Table("Book")]
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }

    [System.ComponentModel.DataAnnotations.Schema.Table("Author")]
    [Dapper.Contrib.Extensions.Table("Author")]
    public class Author
    {
        public Author()
        {
            Books = new List<Book>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Book> Books { get; set; }
    }
}
