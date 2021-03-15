using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Neo4j.Driver;


namespace DriverUsageAnalysis
{
    public class MovieRepositoryCompleted : IMovieRepository
    {
        public IDriver Driver {get; init;}
        
        public MovieRepositoryCompleted(IDriver driver)
        {
            Driver = driver;
        }  
       
        public async void SessionUsage()
        {
            var session = Driver.AsyncSession();            
            try
            {
                
            }
            finally
            {
                await session.CloseAsync();
            }   
        }

        public async Task<MovieRecord> FindByTitle(string title)
        {
            var cypher = $@"MATCH (movie:Movie {{title:""{title}""}})
                            RETURN movie.title AS title,
                                   movie.released AS released,
                                   movie.tagline AS tagline";

            var session = Driver.AsyncSession();            
            try
            {
                return await session.ReadTransactionAsync(async tx => 
                {
                    var cursor = await tx.RunAsync(cypher);

                    return await cursor.SingleAsync(record => new MovieRecord(Title: record["title"].As<string>(),
                                                                              Released: record["released"].As<int>(),
                                                                              Tagline: record["tagline"].As<string>(),
                                                                              Cast: new List<ActorRecord>()));                    
                });                
            }
            finally
            {
                await session.CloseAsync();
            }            
        }

        ///Get multiple results
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
                    var cursor = await tx.RunAsync(cypher);

                    return await cursor.ToListAsync(record => new MovieRecord(Title: record["title"].As<string>(),
                                                                              Released: record["released"].As<int>(),
                                                                              Tagline: record["tagline"].As<string>(),
                                                                              Cast: new List<ActorRecord>()));
                });
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        //Get 'nested' results
        public async Task<List<ActorRecord>> GetMovieCast(string title)
        {
            /*var session = Driver.AsyncSession();
            try
            {
                return await session.ReadTransactionAsync(async tx =>
                {
                    var cursor = await tx.RunAsync(@"
                        MATCH (movie:Movie {title:$title})<-[r]-(person:Person)
                        RETURN COLLECT({
                            name:person.name
                        }) as cast",
                        new {title}
                    );

                    return await cursor.SingleAsync(record => new ActorRecord(Name: record["name"].As<string>()));
                });
            }
            finally
            {
                await session.CloseAsync();
            }*/

            return await Task.FromResult<List<ActorRecord>>(new List<ActorRecord>());
        }
    }
}
