using Dapper;
using DemoDapper.Dtos;
using DemoDapper.Infarstructure;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DemoDapper.Tests
{
    public class Parameters
    {
      
        public async Task ParameterWithDynamicBag(int id)
        {
            string query = $"select * from Products where id = @id";
            var dictionary = new Dictionary<string, object> { 
                {
                    "@id", id
                } 
            };

            var param = new DynamicParameters(dictionary);

            using (var connection = BaseConnection.CreateConnection())
            {
                var data = await connection.QuerySingleAsync<Product>(query, param, commandType: CommandType.Text);
            }
        }

        public async Task ParameterWithDynamicAnonymousType(int id)
        {
            string query = $"select * from Products where id = @id";

            var param = new DynamicParameters(new { id = id});

            using (var connection = BaseConnection.CreateConnection())
            {
                var data = await connection.QuerySingleAsync<Product>(query, param, commandType: CommandType.Text);
            }
        }

        public async Task ParameterWithDynamicObjectType(int id)
        {
            string query = $"select * from Products where id = @id";

            var param = new DynamicParameters(new Product { Id = id});

            using (var connection = BaseConnection.CreateConnection())
            {
                var data = await connection.QuerySingleAsync<Product>(query, param, commandType: CommandType.Text);
            }
        }
        // IsFixedLength => convert to nchar (all context colum is the same length)
        public async Task ParameterWithString()
        {
            string query = $"select * from Products where Name = @Name";
            var param = new { Name = new DbString { Value = "Init Product1", IsFixedLength = true, Length = 50, IsAnsi = false }};

            using (var connection = BaseConnection.CreateConnection())
            {
                var data = await connection.QueryAsync<Product>(query, param, commandType: CommandType.Text);
            }
        }
        public async Task ParameterWithAnonymousType(int id)
        {
            string query = $"select * from Products where id = @id";
            var param = new { id = id };

            using (var connection = BaseConnection.CreateConnection())
            {
                var data = await connection.QuerySingleOrDefaultAsync<Product>(query, param, commandType: CommandType.Text);
            }
        }

        public async Task ParameterWithList(params int[] array)
        {
            string query = $"select * from Products where id IN @ids";
            var param = new { ids = array };

            using (var connection = BaseConnection.CreateConnection())
            {
                var data = await connection.QueryAsync<Product>(query,param, commandType: CommandType.Text);
            }
        }

        public async Task GetWithTableDefine()
        {
            var dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Description");

            for (int i = 0; i < 5; i++)
            {
                dt.Rows.Add(i, "Description_" + i);
            }

            using (var connection = BaseConnection.CreateConnection())
            {
                var data = await connection.QueryAsync("TestSPWithTableDefineValue", commandType: CommandType.StoredProcedure, param: new { tableDefine = dt.AsTableValuedParameter() });
            }
        }
    }
}
