using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DTO.Dtos
{
    [Dapper.Contrib.Extensions.Table("CustomNameColumn")]
    public class CustomColumn
    {
        public int Id { get; set; }
        [Column("TestDescription")]
        public string Description { get; set; }

        [Write(false)]
        public string TestWrite { get; set; }

        [Computed]
        public string TestComputed { get; set; }

        // it is for change name column
        public static void RegisterCustomColumn()
        {
                Dapper.SqlMapper.SetTypeMap(
                   typeof(CustomColumn),
                   new CustomPropertyTypeMap(
                      typeof(CustomColumn),
                       (type, columnName) =>
                       {
                           var data = type.GetProperties().FirstOrDefault(prop =>
                               prop.GetCustomAttributes(false)
                                   .OfType<ColumnAttribute>()
                                   .Any(attr => attr.Name == columnName));
                           if(data == null)
                           {
                               data = type.GetProperties().FirstOrDefault(prop => prop.Name == columnName);
                           }

                           return data;
                       }
                           )
               );
            
        }
    }

    public interface ITestCustomColumn
    {

    }
}
