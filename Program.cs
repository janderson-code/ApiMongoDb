using ApiMongoDb.Models;
using ApiMongoDb.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.Configure<BookStoreDatabaseSettings>(
    builder.Configuration.GetSection("BookStoreDatabase"));

builder.Services.AddSingleton<BooksService>();

var app = builder.Build();

var mongoConnectionString = builder.Configuration.GetSection("BookStoreDatabase")
    .Get<BookStoreDatabaseSettings>();

var client = new MongoClient(mongoConnectionString!.ConnectionString);

var database = client.GetDatabase("BookStore");

await SeedDatabaseAsync(database);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var booksService = app.Services.GetRequiredService<BooksService>();

app.MapGet("/api/books", async () => { return await booksService.GetAsync(); });

app.MapGet("/api/books/{id:length(24)}", async (string id) =>
{
    var book = await booksService.GetAsync(id);
    return book is not null ? Results.Ok(book) : Results.NotFound();
});

app.MapPost("/api/books", async (Book newBook) =>
{
    await booksService.CreateAsync(newBook);
    return Results.Created($"/api/books/{newBook.Id}", newBook);
});

app.MapPut("/api/books/{id:length(24)}", async (string id, Book updatedBook) =>
{
    var book = await booksService.GetAsync(id);

    if (book is null)
    {
        return Results.NotFound();
    }

    updatedBook.Id = book.Id;

    await booksService.UpdateAsync(id, updatedBook);
    return Results.NoContent();
});

app.MapDelete("/api/books/{id:length(24)}", async (string id) =>
{
    var book = await booksService.GetAsync(id);

    if (book is null)
    {
        return Results.NotFound();
    }

    await booksService.RemoveAsync(id);
    return Results.NoContent();
});


app.Run();

async Task SeedDatabaseAsync(IMongoDatabase database)
{
    var collectionName = "Books";
    var collectionExists = (await database.ListCollectionNamesAsync())
        .ToList()
        .Contains(collectionName);

    if (!collectionExists)
    {
        await database.CreateCollectionAsync(collectionName);

        var booksCollection = database.GetCollection<Book>(collectionName);
        var books = new List<Book>
        {
            new Book { BookName = "Design Patterns", Price = 54.93M, Category = "Computers", Author = "Ralph Johnson" },
            new Book { BookName = "Clean Code", Price = 43.15M, Category = "Computers", Author = "Robert C. Martin" },
            new Book { BookName = "Livro de Teste", Price = 43.15M, Category = "Teste", Author = "Teste" }
        };

        await booksCollection.InsertManyAsync(books);
    }
}