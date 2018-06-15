using Kros.IO;
using Kros.KORM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace BulkUpdateTest
{
    class Tester
    {
        public static void Test()
        {
            using (var database = new Database(ConnectionString(),ProviderName()))
            {
                Database.Log = Console.WriteLine;

                database.ExecuteNonQuery("SELECT Name FROM Movies WHERE Id = @Id", new Kros.KORM.Query.CommandParameterCollection() { new Kros.KORM.Query.CommandParameter("Id", 4) });
                PopulateDb(database);
                BulkUpdate(database);
            }
        }

        private static void PopulateDb(Database database)
        {
            if (!database.Query<Movie>().Any())
            {
                var dbSet = database.Query<Movie>().AsDbSet();

                foreach (var movie in TestData())
                {
                    Console.WriteLine($"Added: {movie.Id}-{movie.Name}-{movie.Year}");
                    dbSet.Add(movie);
                }

                //dbSet.BulkInsert();
                Console.WriteLine("BulkInsert Complete.");
            }
        }

        private static void BulkUpdate(Database database)
        {
            var dbSet = database.Query<Movie>().AsDbSet();
            foreach (var movie in TestData())
            {
                movie.Name = "edited";
                Console.WriteLine($"Edited: {movie.Id}-{movie.Name}-{movie.Year}");
                dbSet.Edit(movie);
            }
            dbSet.BulkUpdate();
            Console.WriteLine("BulkUpdate Complete.");
        }

        private static ConnectionStringSettings GetConnectionStringSettings()
        {
            return new ConnectionStringSettings() { ConnectionString = ConnectionString(), ProviderName = ProviderName() };
        }

        private static string ConnectionString() =>
                "Data Source=(LocalDB)\\MSSQLLocalDB; " +
                $"AttachDbFilename={PathHelper.BuildPath(Environment.CurrentDirectory, "Db.mdf")};" +
                " Integrated Security=True; Connect Timeout=30; User ID=test; Password=test;";

        private static string ProviderName() => "System.Data.SqlClient";

        private static IEnumerable<Movie> TestData() =>
            Enumerable.Range(1, 10).Select(x => new Movie() { Id = x, Name = Guid.NewGuid().ToString("N"), Year = 2000 + x });
    }
}
