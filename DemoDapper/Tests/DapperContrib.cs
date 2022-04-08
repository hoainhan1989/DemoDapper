using Dapper.Contrib.Extensions;
using DemoDapper.Dtos;
using DemoDapper.Infarstructure;
using DTO.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoDapper.Tests
{
    public class DapperContrib
    {
        public async Task GetProductById(int id)
        {
            using (var connection = BaseConnection.CreateConnection())
            {
                var data = await connection.GetAsync<Product>(id);
            }  
        }

        public async Task GetAllProducts()
        {
            using (var connection = BaseConnection.CreateConnection())
            {
                var data = await connection.GetAllAsync<Product>();
            }  
        }

        public async Task InsertManyProducts()
        {
            using (var connection = BaseConnection.CreateConnection())
            {
                var products = new List<Product> {
                new Product { Name = "DapperContrib", Description = "DapperContrib" } ,
                new Product { Name = "DapperContrib1", Description = "DapperContrib1" }
                };

                var data = await connection.InsertAsync(products);
            }
        }

        public async Task UpdateProduct(int id)
        {
            using (var connection = BaseConnection.CreateConnection())
            {
                var product = new Product { Id = id, Name = "Dapper contrib1", Description = "Dapper contrib1" };

                var data = await connection.UpdateAsync<Product>(product);
            }

        }

        //Custom Name Column
        public async Task TestCustomColumnAttribute()
        {
            using (var connection = BaseConnection.CreateConnection())
            {
                var data = await connection.GetAllAsync<CustomColumn>();
            }
        }

        public async Task TestWriteAttribute(int id)
        {
            using (var connection = BaseConnection.CreateConnection())
            {
                var data = await connection.GetAsync<CustomColumn>(id);

                data.TestComputed = "TestComputed";
                data.TestWrite = "TestWrite";

                var update = await connection.UpdateAsync<CustomColumn>(data);
            }
        }
       

        //Only support when saving one item
        public async Task DirtyTracking()
        {
            using (var connection = BaseConnection.CreateConnection())
            {
                IProduct product = await connection.GetAsync<IProduct>(52754748);

                var update1 = connection.Update(product);

                product.Description = product.Description;

                var update2 = connection.Update(product);
            }

        }
    }
}
