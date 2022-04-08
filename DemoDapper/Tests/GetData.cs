using Dapper;
using DemoDapper.Dtos;
using DemoDapper.Infarstructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDapper.Tests
{
    public class GetData
    {
        //Only return first column, It is suit for getting count, or only Id or Name
        public async Task GetWithScalar_Product()
        {
            string query = $"select top 2 * from Products";

            var dbConnection = BaseConnection.CreateConnection();
            using (var connection = dbConnection)
            {
                var data = await connection.ExecuteScalarAsync<int>(query, commandType: CommandType.Text);
            }
        }

        //It will execute the query, and base on the result to get single, if the result has more than one record, it throws the error
        public async Task GetWithSingleOrDefault_Product()
        {
            string query = $"select top 1 * from Products";

            var dbConnection = BaseConnection.CreateConnection();

            using (var connection = dbConnection)
            {
                var data = await connection.QuerySingleOrDefaultAsync<Product>(query, commandType: CommandType.Text);
            }
        }

        // you need to map by manually
        public async Task GetWithDataReader_Product()
        {
            var list = new List<Product>();
            string query = $"select top 2 * from Products";
            IDataReader reader;

            using (var connection = BaseConnection.CreateConnection())
            {
                reader = await connection.ExecuteReaderAsync(query, commandType: CommandType.Text);
            }

            DataTable table = new DataTable();
            table.Load(reader);

            foreach (DataRow row in table.Rows)
            {
                int id = Convert.ToInt32(row["Id"].ToString());
                string name = row["Name"].ToString();
                string description = row["Description"].ToString();

                list.Add(new Product() { Description = description, Name = name, Id = id });
            }
        }

        //It will execute the query, and base on the result to get first, if the result has more than one record, it throws the error
        public async Task GetWithFirst_Product()
        {
            string query = $"select top 10 * from Products ";

            using (var connection = BaseConnection.CreateConnection())
            {
                var data = await connection.QueryFirstAsync<Product>(query, commandType: CommandType.Text);
            }
        }

        public async Task<Product> GetWithQueryAsync_Product(int id)
        {
            string query = $"select * from Products where id = @id";
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add("@id", id);

            using (var connection = BaseConnection.CreateConnection())
            {
                var data = await connection.QueryAsync<Product>(query, dynamicParams, commandType: CommandType.Text);

                return data.FirstOrDefault();
            }   
        }
        // it can run multi queries at the same time and avoid to use it, it is unnecessary, cause it has performance issue
        public async Task GetWithQueryMultiple_Author_Book()
        {
            string query = $"select top 10 * from dbo.Book ; select top 10 * from dbo.Author ";

            using (var connection = BaseConnection.CreateConnection())
            {
                var multi = await connection.QueryMultipleAsync(query);

                var author = multi.Read<Author>().ToList();
                var book = multi.Read<Book>().ToList();
            }
        }

        public List<Product> GetWithNonBuffered_Product()
        {
             string query = $"select top 10 * from Products";

            using (var connection = BaseConnection.CreateConnection())
            {
                var data = connection.Query<Product>(query, commandType: CommandType.Text, buffered: false);

                data = data.Where(x => x.Id == 49803014);
                var result = data.ToList();
                return result;
            }
        }
       
        public async Task<List<Product>> GetWithReturnDynamic_Product()
        {
            string query = $"select top 10 * from Products";
            var result = new List<Product>();

            using (var connection = BaseConnection.CreateConnection())
            {
                var data = await connection.QueryAsync(query, commandType: CommandType.Text);
                foreach (var item in data)
                {
                    result.Add(new Product { Name = item.Name, Id = item.Id });
                }

                return result.ToList();
            }
        }
    }
}
