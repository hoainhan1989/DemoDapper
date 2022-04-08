using Dapper;
using DemoDapper.Infarstructure;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DemoDapper.Tests
{
    public class Operates
    {
        public async Task Insert_Product()
        {
            string query = @$"INSERT INTO Products(Name, Description) Values('Hung', 'Dung');
                            INSERT INTO Products(Name, Description) Values('Hung', 'Dung')";

            using (var connection = BaseConnection.CreateConnection())
            {
               var result = await connection.ExecuteAsync(query, commandType: CommandType.Text);
            }
               
        }

        public async Task Update_Product(int id)
        {
            string query = $"UPdate Products Set Name = 'Nana' where Id = {id}";

            using (var connection = BaseConnection.CreateConnection())
            {
                var result = await  connection.ExecuteAsync(query, commandType: CommandType.Text);
            }
        }

        public async Task Delete(int id)
        {
            string query = $"Delete from Products where Id = {id}";

            using (var connection = BaseConnection.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, commandType: CommandType.Text);
            }
        }

        public async Task DeleteSingleItemWithStoreProcedure()
        {
            string query = $"sp_delete_product";

            using (var connection = BaseConnection.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { id = 1 }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task InsertMultiItemWithStoreProcedure()
        {
            string query = $"sp_insert_product";

            using (var connection = BaseConnection.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new[]{
                new { name = "name 1", description = "description 1" },
                new { name = "name 2", description = "description 2" },
                new { name = "name 3", description = "description 3" },
                new { name = "name 4", description = "description 4" }
            }, commandType: CommandType.StoredProcedure);
            }
        }

        public void InsertMultiItems()
        {
            string query = $"INSERT INTO Products(Name, Description) Values(@Name, @Description)";

            var parameters = new List<DynamicParameters>();

            for (var i = 0; i < 10; i++)
            {
                var p = new DynamicParameters();
                p.Add("@Name", "Many_Insert_" + (i + 1));
                p.Add("@Description", "Many_Insert_" + (i + 1));
 
                parameters.Add(p);
            }

            using (var connection = BaseConnection.CreateConnection())
            {
               var result = connection.Execute(query, parameters, commandType: CommandType.Text);
            }
        }
    }
}
