using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using DemoDapper.EntityFramwork;
using DemoDapper.Tests;
using DTO.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDapper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());
            var get = new GetData();
            var operate = new Operates();
            var parameter = new Parameters();
            var multi = new MultiMapping();
            var contrib = new DapperContrib();
            var dapperPlus = new DapperPlus();
            var EF = new EntityFrameworkCore();

             //dapperPlus.InitData();

            // await get.GetWithFirst_Product();
           // await get.GetWithReturnDynamic_Product();
           // get.GetWithNonBuffered_Product();
            //-----------------------------------------------------

            //await operate.Insert_Product();
            //await operate.InsertMultiItemWithStoreProcedure();
            //-----------------------------------------------------

            // await parameter.ParameterWithString();
            //await parameter.GetWithTableDefine();
            //-----------------------------------------------------

            //multi.MappingWithOneToMany_Author_Book(); 
            //await multi.MappingWithMultiResult();
            //multi.MappingWithMoreThan7Types();
            //-----------------------------------------------------

            //await contrib.InsertManyProducts();
            // await contrib.DirtyTracking();

            //CustomColumn.RegisterCustomColumn();
            //await contrib.TestCustomColumnAttribute();

            //-----------------------------------------------------

             dapperPlus.BulkInsert_ManyProduct_Performance_WithIdentity();

            //var authors = multi.MappingWithOneToMany_Author_Book_Return();
            //authors.First().Name = "Change Name Outside";
            //authors.First().Books.First().Name = "Change Name Outside";
            //dapperPlus.BulkUpdate_OneToMany_Author(authors); 

            //Need to run at release mode
             var summary = BenchmarkRunner.Run<EFCoreVSDapper>();

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
