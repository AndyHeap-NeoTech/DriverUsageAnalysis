using System;
using System.Collections.Generic;

namespace DriverUsageAnalysis
{
    public record MovieRecord(string Title, int Released, string Tagline, List<ActorRecord> Cast)
    {
        public void WriteToConsole()
        {
            Console.WriteLine("Movie record:\n\t" + Title + "\n\t" + Released + "\n\t" + Tagline);          
        }
    }


    public record ActorRecord(string Name)
    {
        public void WriteToConsole()
        {
            Console.WriteLine("Actor record:\n\t" + Name);
        }
    }


    /* NOTE: The above immutable record type was introduced with C# 9.0. The below classes show what they basically implement.
    public class MovieRecord
    {
        public string Title { get; }
        public int Released { get; }
        public string Tagline { get; }
        public List<Actor> Cast { get; } = new();  //C# 9.0 version of new List<Actor>()

        public MovieRecord(string title, int released, string tagline, List<Actor> cast)
        {   
            Title = title;
            Released = released;
            Tagline = tagline;
            Cast = cast;
        }

        public override string ToString()
        {
            return "Movie record:\n\t" + Title + "\n\t" + Released + "\n\t" + Tagline;
        }
    }


    public class ActorRecord
    {
        public string Name { get; }

        public ActorRecord(string name)
        {
            Name = name;
        }
    }
    */
}