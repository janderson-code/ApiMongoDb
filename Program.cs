using ApiMongoDb.Models;
using ApiMongoDb.Services;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:8080"); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<BookStoreDatabaseSettings>(
    builder.Configuration.GetSection("BookStoreDatabase"));
builder.Services.AddSingleton<BooksService>();

var app = builder.Build();

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