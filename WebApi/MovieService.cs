using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

public class MovieService
{
    private readonly IMongoCollection<Movie> _moviesCollection;

    public MovieService(IOptions<DatabaseSettings> options)
    {
        var mongoDbConnectionString = options.Value.ConnectionString;
        var mongoClient = new MongoClient(mongoDbConnectionString);
        var database = mongoClient.GetDatabase("gbs");
        _moviesCollection = database.GetCollection<Movie>("movies");
    }

    public string Check()
    {
        if(_moviesCollection != null)
        {
            return _moviesCollection.Database.DatabaseNamespace.DatabaseName;
        }
        else
        {
            return "Not connected to MongoDB";
        }
    }

    public void Create(Movie movie)
    {
        _moviesCollection.InsertOne(movie);
    }

    public IEnumerable<Movie> Get()
    {
        return _moviesCollection.Find(new BsonDocument()).ToList();
    }

    public Movie Get(string id)
    {
        var filter = Builders<Movie>.Filter.Eq("Id", id);
        return _moviesCollection.Find(filter).FirstOrDefault();
    }

    public void Update(string id, Movie updatedMovie)
    {
        var filter = Builders<Movie>.Filter.Eq("Id", id);

        var update = Builders<Movie>.Update
            .Set("Title", updatedMovie.Title)
            .Set("Summary", updatedMovie.Summary)
            .Set("Year", updatedMovie.Year)
            .Set("Actors", updatedMovie.Actors);

        _moviesCollection.UpdateOne(filter, update);
    }

    public void Remove(string id)
    {
        var filter = Builders<Movie>.Filter.Eq("Id", id);
        _moviesCollection.DeleteOne(filter);
    }
}