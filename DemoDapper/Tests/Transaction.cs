using Dapper;
using DemoDapper.Infarstructure;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;

namespace DemoDapper.Tests
{
    public class Transaction
    {
        public async Task InsertWithTransactionWithin()
        {
            string query = $"INSERT INTO Product(Name, Description) Values('Hung', 'Dung')";

            using (var connection = BaseConnection.CreateConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync(query, commandType: CommandType.Text, transaction: transaction);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
            
        }

        public async Task DeleteWithTransactionScope()
        {
           // Automatically rollback when having the error or exception
            using (var transaction = new TransactionScope())
            {
                string query = $"sp_delete_product";
                using (var connection = BaseConnection.CreateConnection())
                {
                    await connection.ExecuteAsync(query, new {id = 1 }, commandType: CommandType.StoredProcedure);
                }

                transaction.Complete();
            }
        }
    }
}
