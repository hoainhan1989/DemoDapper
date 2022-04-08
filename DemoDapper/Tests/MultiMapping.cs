using Dapper;
using DemoDapper.Dtos;
using DemoDapper.Infarstructure;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DemoDapper.Tests
{
    public class MultiMapping
    {
        public async Task<List<Book>> MappingWithOneToOne_Book_Author()
        {
            string query = $"select top 10 * from dbo.Book book join dbo.Author author on book.AuthorId = author.Id";

            using (var connection = BaseConnection.CreateConnection())
            {
                var data = await connection.QueryAsync<Book, Author, Book>(query, (book, author) => {
                    book.Author = author;
                    return book;
                }, splitOn: "AuthorId");

                return data.ToList();
            }
        }

        public void MappingWithOneToMany_Author_Book()
        {
            var authorDictionary = new Dictionary<int, Author>();
            string query = $"select * from dbo.Book book join dbo.Author author on book.AuthorId = author.Id";

            using (var connection = BaseConnection.CreateConnection())
            {
                var data = (connection.Query<Book, Author, Author>(query, (book, author) => {

                    Author authorEntry = null;

                    if (!authorDictionary.TryGetValue(author.Id, out authorEntry))
                    {
                        authorEntry = author;
                        authorEntry.Books = new List<Book>();
                        authorDictionary.Add(authorEntry.Id, authorEntry);
                    }

                    authorEntry.Books.Add(book);
                    return authorEntry;

                }, splitOn: "Id")).Distinct().ToList();
            }
        }

        public List<Author> MappingWithOneToMany_Author_Book_Return()
        {
            var authorDictionary = new Dictionary<int, Author>();
            string query = $"select top 10 * from dbo.Book book join dbo.Author author on book.AuthorId = author.Id";
            var list = new List<Author>();

            using (var connection = BaseConnection.CreateConnection())
            {
                var data = (connection.Query<Book, Author, Author>(query, (book, author) => {

                    Author authorEntry = null;

                    if (!authorDictionary.TryGetValue(author.Id, out authorEntry))
                    {
                        authorEntry = author;
                        authorEntry.Books = new List<Book>();
                        authorDictionary.Add(authorEntry.Id, authorEntry);
                    }

                    book.AuthorId = author.Id;
                    authorEntry.Books.Add(book);
                    return authorEntry;

                }, splitOn: "Id")).Distinct();

                return data.ToList();
            }
        }

        public async Task MappingWithMultiResult()
        {
            string query = $"select top 10 * from dbo.Book ; select top 10 * from dbo.Author ";

            using (var connection = BaseConnection.CreateConnection())
            {
                var multi = await connection.QueryMultipleAsync(query);

                var book = multi.Read<Book>().ToList();
                var author = multi.Read<Author>().ToList();
            } 
        }

        //Must follow by order type
        public void MappingWithMoreThan7Types()
        {
            var sql = @"SELECT 
                                    1 AS Id, 'Daniel Dennett' AS Name, 1942 AS Born, 
                                    2 AS CountryId, 'United States of America' AS CountryName,
                                    3 AS BookId, 'Brainstorms' AS BookName,
                                    4 AS RegionId, 'Region' RegionName ,
                                    5 AS AddressId, 'Address' AddressName ,
                                    6 AS HobbyId, 'Hobby' HobbyName ,
                                    7 AS AreaId, 'Area' AreaName ,
                                    8 AS GenderId, 'Gender' GenderName ";
                      
                       
            var connection = BaseConnection.CreateConnection();
            var remainingHorsemen = new Dictionary<int, Person>();
            connection.Query<Person>(sql,
                new[]
                {
                    typeof(Person),
                    typeof(Country),
                    typeof(Books),
                    typeof(Region),
                    typeof(Address),
                    typeof(Hobby),
                    typeof(Area),
                    typeof(Gender)
                }
                , obj => {
                    Person person = obj[0] as Person;
                    Country country = obj[1] as Country;
                    Books book = obj[2] as Books;
                    Region region = obj[3] as Region;
                    Address address = obj[4] as Address;
                    Hobby hobby = obj[5] as Hobby;
                    Area area = obj[6] as Area;
                    Gender gender = obj[7] as Gender;

                   return person;
                        },
                    splitOn: "CountryId,BookId,RegionId,AddressId,HobbyId,AreaId,GenderId");
        }

        // Hierachy
        public async Task MappingWithMultiType()
        {
            var listContract = new List<Contract>();

            string query = $"select * from dbo.Contract";

            using (var connection = BaseConnection.CreateConnection())
            {
                var reader = await connection.ExecuteReaderAsync(query);

                var tvContractParser = reader.GetRowParser<TvContract>();
                var mobileContractParser = reader.GetRowParser<MobileContract>();

                while (reader.Read())
                {
                    var constractType = (ContractType)reader.GetInt32(reader.GetOrdinal(nameof(ContractType)));
                    switch (constractType)
                    {
                        case ContractType.TV:
                            listContract.Add(tvContractParser(reader));
                            break;

                        case ContractType.Mobile:
                            listContract.Add(mobileContractParser(reader));
                            break;
                    }
                }
            }
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public Country Residience { get; set; }
        public List<Books> Books { get; set; }
    }
    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }

    public class Books
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
    }

    public class Region
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; }
    }

    public class Area
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
    }

    public class Address
    {
        public int AddressId { get; set; }
        public string AddressName { get; set; }
    }

    public class Gender
    {
        public int GenderId { get; set; }
        public string GenderName { get; set; }
    }

    public class Hobby
    {
        public int HobbyId { get; set; }
        public string HobbyName { get; set; }
    }
}
