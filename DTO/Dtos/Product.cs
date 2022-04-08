using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;
using Dapper;

namespace DemoDapper.Dtos
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Products")]
    [Dapper.Contrib.Extensions.Table("Products")] 
    public class Product
    {
        [System.ComponentModel.DataAnnotations.Key]
        [Dapper.Contrib.Extensions.Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Write(false)]
        public ProductDetail ProductDetail { get; set; } 
    }

    public interface IProduct
    {
    
        int Id { get; set; }
         string Name { get; set; }
        string Description { get; set; }
    }
}
