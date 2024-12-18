namespace ApiMongoDb.Models;

public record BookStoreDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DataBaseName { get; set; } = null!;

    public string BooksCollectionName { get; set; } = null!;
}

   
