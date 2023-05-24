using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var movieDatabaseConfigSection = builder.Configuration.GetSection("DatabaseSettings");
builder.Services.Configure<DatabaseSettings>(movieDatabaseConfigSection);

builder.Services.AddSingleton<MovieService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "Hello Worlds");

app.MapGet("/check", (MovieService movieService) =>
{
    return movieService.Check();
});

app.MapPost("/api/movies", (Movie movie, MovieService movieService) =>
{
    movieService.Create(movie);
    return Results.Ok(movie);
});

app.MapGet("/api/movies", (MovieService movieService) =>
{
    var movies = movieService.Get();
    return Results.Ok(movies);
});

app.MapGet("/api/movies/{id}", (string id, MovieService movieService) =>
{
    var movie = movieService.Get(id);

    if (movie != null)
    {
        return Results.Ok(movie);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapPut("/api/movies/{id}", (string id, Movie movie, MovieService movieService) =>
{
    movieService.Update(id, movie);
    return Results.Ok(movie);
});

app.MapDelete("/api/movies/{id}", (string id, MovieService movieService) =>
{
    movieService.Remove(id);
    return Results.Ok();
}).WithName("deleteMovie")
.WithOpenApi();


app.Run();
