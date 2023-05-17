using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args); 
var app = builder.Build();

app.MapGet("/", () => "Hello Worlds");

app.MapGet("/check", async () => { 
    try { 
            var connectionString = "mongodb://root:example@mongo:27017/"; 
            var mongoClient = new MongoClient(connectionString); 
            var databaseNames = await mongoClient.ListDatabaseNames().ToListAsync(); 
            var response = new { Databases = databaseNames }; 
            return Results.Ok(response); 
        } 
        catch (Exception ex) { 
            return Results.StatusCode(StatusCodes.Status500InternalServerError); 
            } 
        });

app.Run();
