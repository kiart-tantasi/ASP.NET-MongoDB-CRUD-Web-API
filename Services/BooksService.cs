using WebAPIwithMongoDB.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace WebAPIwithMongoDB.Services
{
    public class BooksService
    {
        private readonly IMongoCollection<Book> _booksCollection; // from MongoDB.Driver

        // Configure mongodb url, database name and collection name
        public BooksService(IOptions<BookStoreDatabaseSetting> settings)
        // from /Models/BookStoreDatabaseSetting to link to appsettings.json
        {

            var mongoClient = new MongoClient(settings.Value.ConnectionString); // from MongoDB.Driver

            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);

            _booksCollection = mongoDatabase.GetCollection<Book>(settings.Value.BooksCollectionName);
        }

        public async Task<List<Book>> GetAsync()
        {
            return await _booksCollection.Find(x => true).ToListAsync();
        }

        public async Task<Book> GetAsync(string id)
        {
            return await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Book newBook) =>
            await _booksCollection.InsertOneAsync(newBook);

        public async Task UpdateSync(string id, Book updatedBook) =>
            await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _booksCollection.DeleteOneAsync(x => x.Id == id);
    }
}
