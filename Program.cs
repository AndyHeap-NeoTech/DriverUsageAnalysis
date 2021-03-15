using System;
using System.Threading.Tasks;
using Neo4j.Driver;


namespace DriverUsageAnalysis
{
    //Wrap the chosen logging framework, in this case NLog, in a Neo4j logging interface
    class Program
    {
        static private string Uri => Environment.GetEnvironmentVariable("NEO4J_URI") ?? "bolt://localhost:7687";           
        static private string User =>  Environment.GetEnvironmentVariable("NEO4J_USER") ?? "neo4j";
        static private string Password => Environment.GetEnvironmentVariable("NEO4J_PASSWORD") ?? "testingmovies";

        static async Task Main(string[] args)
        {   
            
            try
            {
                using var driver = GraphDatabase.Driver(Uri, AuthTokens.Basic(User, Password));

                var movieRepo = new MovieRepository(driver);

                var movie = await movieRepo.FindByTitle("Top Gun");   
                movie.WriteToConsole();   

                var movieList = await movieRepo.Search("Matrix");
                movieList.ForEach(m => m.WriteToConsole());

                var castList = await movieRepo.GetMovieCast("The Matrix");
                castList.ForEach(a => a.WriteToConsole());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
