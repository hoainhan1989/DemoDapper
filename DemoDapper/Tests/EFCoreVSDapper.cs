using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using DemoDapper.Dtos;
using DemoDapper.EntityFramwork;
using System;
using System.Collections.Generic;
using System.Text;
namespace DemoDapper.Tests
{
    [SimpleJob(launchCount: 2, warmupCount: 2, targetCount: 2, invocationCount: 5)]
    public class EFCoreVSDapper
    {
        private readonly MultiMapping Dapper;
        private readonly EntityFrameworkCore EntityFramework;
        private readonly Operates Operates;
        private readonly DapperPlus OperatesPlus;

        List<Author> authors = new List<Author>();
        public EFCoreVSDapper()
        {
            Dapper = new MultiMapping();
            EntityFramework = new EntityFrameworkCore();
            Operates = new Operates();
            OperatesPlus = new DapperPlus();
           // authors = EntityFramework.GetAuthors_Return();
        }

        //[Benchmark]
        //public void UpdateAuthorsWithEntityFramework() => EntityFramework.Authors_BulkUpdate(authors);

        //[Benchmark]
        //public void UpdateAuthorsWithDapper() => OperatesPlus.BulkUpdate_ManyAuthor(authors);


        [Benchmark]
        public void GetAuthorsWithEntityFramework() => EntityFramework.GetAuthors();

        [Benchmark]
        public void GetAuthorsWithDapper() => Dapper.MappingWithOneToMany_Author_Book();

        //[Benchmark]
        //public void InsertProductWithEntityFramework() => EntityFramework.InsertProducts_BulkInsert();

        //[Benchmark]
        //public void InsertProductWithDapper() => OperatesPlus.InsertOneToMany_Product();
    }
}
