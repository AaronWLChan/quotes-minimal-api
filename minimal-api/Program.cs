using Microsoft.EntityFrameworkCore;
using minimal_api.Models;
using minimal_api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MyDbContext>(opt => opt.UseInMemoryDatabase("MyDb"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/", () => "Running!");

//Get List
app.MapGet("/quotes", async (MyDbContext db) =>
    await db.Quotes.ToListAsync());

//Get random return 404 if there are no quotes
app.MapGet("/quotes/random", async (MyDbContext db) => {

    var count = await db.Quotes.CountAsync();

    if (count == 0) return Results.NotFound();

    Random random = new Random();

    var quotes = await db.Quotes.ToListAsync();

    var index = random.Next(quotes.Count);

    return Results.Ok(quotes[index]);

});

// Get By Id
app.MapGet("/quotes/{id}", async (int id, MyDbContext db) =>
    await db.Quotes.FindAsync(id)
        is Quote todo
            ? Results.Ok(todo)
            : Results.NotFound());

//Create
app.MapPost("/quotes", async (Quote quote, MyDbContext db) =>
{
    db.Quotes.Add(quote);
    await db.SaveChangesAsync();

    return Results.Created($"/quotes/{quote.Id}", quote);
});

//Update
app.MapPut("/quotes/{id}", async (int id, Quote newQuote, MyDbContext db) =>
{
    var quote = await db.Quotes.FindAsync(id);

    if (quote is null) return Results.NotFound();

    quote.Saying = newQuote.Saying;
    quote.SaidBy = newQuote.SaidBy;
    quote.TVShow = newQuote.TVShow;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

//Delete
app.MapDelete("/quotes/{id}", async (int id, MyDbContext db) =>
{
    if (await db.Quotes.FindAsync(id) is Quote quote)
    {
        db.Quotes.Remove(quote);
        await db.SaveChangesAsync();
        return Results.Ok(quote);
    }

    return Results.NotFound();
});

//Characters

//Get List
app.MapGet("/characters", async (MyDbContext db) => await db.Characters.ToListAsync());

// Get By Id
app.MapGet("/characters/{id}", async (int id, MyDbContext db) =>
    await db.Characters.FindAsync(id)
        is Character character
            ? Results.Ok(character)
            : Results.NotFound());

//Create
app.MapPost("/characters", async (Character character, MyDbContext db) =>
{
    db.Characters.Add(character);

    await db.SaveChangesAsync();

    return Results.Created($"/characters/{character.Id}", character);
});

//Update
app.MapPut("/characters/{id}", async (int id, Character newCharacter, MyDbContext db) => {

    if (await db.Characters.FindAsync(id) is Character character)
    {

        character.Name = newCharacter.Name;

        await db.SaveChangesAsync();

        return Results.NoContent();

    }

    return Results.NotFound();


});

// Delete
app.MapDelete("/characters/{id}", async (int id, MyDbContext db) => {

    if (await db.Characters.FindAsync(id) is Character character) {
        
        db.Characters.Remove(character);

        await db.SaveChangesAsync();

        return Results.Ok(character);

    }

    return Results.NotFound();

});


//TV Shows

//Get List
app.MapGet("/tv_shows", async (MyDbContext db) => await db.TVShows.ToListAsync());

//Get By Id
app.MapGet("/tv_shows/{id}", async(int id, MyDbContext db) => await db.TVShows.FindAsync(id)
        is TVShow tvShow
            ? Results.Ok(tvShow)
            : Results.NotFound());

//Create
app.MapPost("/tv_shows", async (TVShow tvShow, MyDbContext db) =>
{
    db.TVShows.Add(tvShow);

    await db.SaveChangesAsync();

    return Results.Created($"/tv_shows/{tvShow.Id}", tvShow);

});

//Update
app.MapPut("/tv_shows/{id}", async (int id, TVShow newTVShow, MyDbContext db) =>
{
    if (await db.TVShows.FindAsync(id) is TVShow tvShow) {

        tvShow.Name = newTVShow.Name;

        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    return Results.NotFound();

});

//Delete
app.MapDelete("/tv_shows/{id}", async (int id, MyDbContext db) =>
{
    if (await db.TVShows.FindAsync(id) is TVShow tvShow)
    {
        db.TVShows.Remove(tvShow);
        await db.SaveChangesAsync();
        return Results.Ok(tvShow);
    }

    return Results.NotFound();

});


app.Run();


