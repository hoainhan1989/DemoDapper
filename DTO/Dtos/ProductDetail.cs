using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoDapper.Dtos
{
    [Table("ProductDetail")]
    public class ProductDetail
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
    }
}
