using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;



var builder = WebApplication.CreateBuilder(args);
var movieDatabaseConfigSection = builder.Configuration.GetSection("DatabaseSettings");
builder.Services.Configure<DatabaseSettings>(movieDatabaseConfigSection);
var app = builder.Build();



app.MapGet("/", () => "Hello Worlds");

app.MapGet("/check", async (Microsoft.Extensions.Options.IOptions<DatabaseSettings> options) =>
{
    try
    {
        var connectionString = options.Value.ConnectionString;
        var mongoClient = new MongoClient(connectionString);
        var databaseNames = await mongoClient.ListDatabaseNames().ToListAsync();
        var response = new { Databases = databaseNames };
        return Results.Ok(response);
    }
    catch (Exception ex)
    {
        return Results.StatusCode(StatusCodes.Status500InternalServerError);
    }
});

// Insert Movie
// Wenn das übergebene Objekt eingefügt werden konnte,
// wird es mit Statuscode 200 zurückgegeben.
// Bei Fehler wird Statuscode 409 Conflict zurückgegeben.
app.MapPost("/api/movies", (Movie movie) =>
{
        var myMovie = new Movie()
        {
            Id = "1",
            Title = "Asterix und Obelix",
        };
        return Results.Ok(myMovie);
});
// Get all Movies
// Gibt alle vorhandenen Movie-Objekte mit Statuscode 200 OK zurück.
app.MapGet("api/movies", () =>
{
    throw new NotImplementedException();
});

app.MapGet("api/movies/{id}", (string id) =>
{
    if (id == "1")
    {
        var myMovie = new Movie()
        {
            Id = "1",
            Title = "Asterix und Obelix",
        };
        return Results.Ok(myMovie);
    }
    else
    {
        return Results.NotFound();
    }
});
// Update Movie
// Gibt das aktualisierte Movie-Objekt zurück.
// Bei ungültiger id wird Statuscode 404 not found zurückgegeben.
app.MapPut("/api/movies/{id}", (string id, Movie movie) =>
{
    if (id == "1")
    {
        return Results.Ok(movie);
    }
    else
    {
        return Results.NotFound();
    }
});
// Delete Movie
// Gibt bei erfolgreicher Löschung Statuscode 200 OK zurück.
// Bei ungültiger id wird Statuscode 404 not found zurückgegeben.
app.MapDelete("api/movies/{id}", (string id) =>
{
    if (id == "1")
    {
        return Results.Ok();
    }
    else
    {
        return Results.NotFound();
    }
});


app.Run();
