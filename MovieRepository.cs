using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Neo4j.Driver;


namespace DriverUsageAnalysis
{
    public interface IMovieRepository
    {
        Task<MovieRecord> FindByTitle(string title);
        public Task<List<MovieRecord>> Search(string search);        
    }


    public class MovieRepository : IMovieRepository
    {
        public IDriver Driver {get; init;}
        
        public MovieRepository(IDriver driver)
        {
            Driver = driver;
        }  
        

        //Instructions for Movies DB.

        /**********************************************************************************************************************

            Session object lifecycle.
            Aim:    To create and dispose of a Neo4j IAsyncSession demonstrating the objects typical usage/lifecycle
            Notes:  This is a contrived exercise to open discussion on the usage of sessions and to get your thoughts.
            
            Useful:

        **********************************************************************************************************************/
        public async void SessionUsage()
        {
            await Task.CompletedTask;   //Replace this placeholder line.
        }



        /**********************************************************************************************************************

            Use session to run a query against the database.
            Aim:    This method should use the supplied Cypher access the Neo4j DB to find the requested movie. We are interested
                    in the approach that is chosen and the driver methods used as a starting point for the discussion.
            Notes:  We are not going to focus on error handling here, for example if no result is found. This is the happy
                    path only. 
                    The processing of the results will be done in the next exercise so can be ignored for the time being.
                    The session lifecycle talked about in the previous exercise should be used here.
            
            Useful:

        **********************************************************************************************************************/
        public async Task<MovieRecord> FindByTitle(string title)
        {
            var cypher = $@"MATCH (movie:Movie {{title:""{title}""}})
                            RETURN movie.title AS title,
                                   movie.released AS released,
                                   movie.tagline AS tagline";

            return await Task.FromResult<MovieRecord>(new MovieRecord("Title", 0, "Tagline", null));   //Replace this placeholder line.
        }



        /**********************************************************************************************************************

            Get multiple results and parse them into objects.
            Aim:    This exercise expands on the previous one with the aim of observing and talking about the parsing of
                    the query results into a list of an application type, MovieRecord. 
            Notes:  As before the cypher is supplied and we are just implementing the happy path. 
                    We have also suplied an implementation of a transaction method on the session object. This may be what was used earlier or
                    you may have used the AutoCommit functionality. 
                    The Cypher query will return multiple results to be handled.      
            
            Useful: Manual Links?
                    Description of options/thoughts on what to use?
                    Thoughts on extension methods for result parsing?

        **********************************************************************************************************************/
        public async Task<List<MovieRecord>> Search(string search)
        {
            var cypher = $@"MATCH (movie:Movie)
                            WHERE TOLOWER(movie.title) CONTAINS TOLOWER(""{search}"")
                            RETURN movie.title AS title,
                                   movie.released AS released,
                                   movie.tagline AS tagline";

            var session = Driver.AsyncSession();
            try
            {
                return await session.ReadTransactionAsync(async tx =>
                {
                    return await Task.FromResult<List<MovieRecord>>(new List<MovieRecord>{new MovieRecord("Title", 0, "Tagline", null)});   //Replace this placeholder line with your code.
                });
            }
            finally
            {
                await session.CloseAsync();
            }
        }



        //Get 'nested' results
        //TODO: Maybe not needed?
        public async Task<List<ActorRecord>> GetMovieCast(string title)
        {
            return await Task.FromResult<List<ActorRecord>>(new List<ActorRecord>());       //Replace this placeholder line with your code.
        }
        
    }
}
