// Data/DatabaseManager.cs
using Microsoft.Data.Sqlite;
using BookManagerCSharp.Models;

namespace BookManagerCSharp.Data
{
    public static class DatabaseManager
    {
              private static readonly string _dbPath =
                            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BookManager.db");

              private static SqliteConnection CreateConnection() =>
                            new SqliteConnection($"Data Source={_dbPath}");

              // ─── Inicializar base de datos ────────────────────────────────────
              public static void Initialize()
              {
                            using var conn = CreateConnection();
                            conn.Open();

                            var cmd = conn.CreateCommand();
                            cmd.CommandText = @"
                                            CREATE TABLE IF NOT EXISTS Users (
                                                                Id       INTEGER PRIMARY KEY AUTOINCREMENT,
                                                                                    Username TEXT    NOT NULL UNIQUE,
                                                                                                        Password TEXT    NOT NULL,
                                                                                                                            Bio      TEXT    DEFAULT '',
                                                                                                                                                Avatar   TEXT    DEFAULT ''
                                                                                                                                                                );
                                                                                                                                                                                CREATE TABLE IF NOT EXISTS Books (
                                                                                                                                                                                                    Id       INTEGER PRIMARY KEY AUTOINCREMENT,
                                                                                                                                                                                                                        UserId   INTEGER NOT NULL,
                                                                                                                                                                                                                                            Title    TEXT    NOT NULL,
                                                                                                                                                                                                                                                                Author   TEXT    DEFAULT '',
                                                                                                                                                                                                                                                                                    Genre    TEXT    DEFAULT '',
                                                                                                                                                                                                                                                                                                        Status   INTEGER NOT NULL DEFAULT 0,
                                                                                                                                                                                                                                                                                                                            Rating   INTEGER DEFAULT 0,
                                                                                                                                                                                                                                                                                                                                                Review   TEXT    DEFAULT '',
                                                                                                                                                                                                                                                                                                                                                                    AddedAt  TEXT    DEFAULT (datetime('now'))
                                                                                                                                                                                                                                                                                                                                                                                    );
                                                                                                                                                                                                                                                                                                                                                                                                    CREATE TABLE IF NOT EXISTS Friendships (
                                                                                                                                                                                                                                                                                                                                                                                                                        Id       INTEGER PRIMARY KEY AUTOINCREMENT,
                                                                                                                                                                                                                                                                                                                                                                                                                                            UserId   INTEGER NOT NULL,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                FriendId INTEGER NOT NULL,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    Status   TEXT    DEFAULT 'pending'
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    );
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    CREATE TABLE IF NOT EXISTS Messages (
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        Id         INTEGER PRIMARY KEY AUTOINCREMENT,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            SenderId   INTEGER NOT NULL,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                ReceiverId INTEGER NOT NULL,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    Content    TEXT    NOT NULL,
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        SentAt     TEXT    DEFAULT (datetime('now'))
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        );";
                            cmd.ExecuteNonQuery();
              }

              // ─── Usuarios ─────────────────────────────────────────────────────
              public static bool RegisterUser(string username, string password)
              {
                            try
                            {
                                              using var conn = CreateConnection();
                                              conn.Open();
                                              var cmd = conn.CreateCommand();
                                              cmd.CommandText = "INSERT INTO Users (Username, Password) VALUES (@u, @p)";
                                              cmd.Parameters.AddWithValue("@u", username);
                                              cmd.Parameters.AddWithValue("@p", password);
                                              cmd.ExecuteNonQuery();
                                              return true;
                            }
                            catch (SqliteException)
                            {
                                              return false; // Username ya existe
                            }
              }

              public static User? LoginUser(string username, string password)
              {
                            using var conn = CreateConnection();
                            conn.Open();
                            var cmd = conn.CreateCommand();
                            cmd.CommandText = "SELECT * FROM Users WHERE Username=@u AND Password=@p";
                            cmd.Parameters.AddWithValue("@u", username);
                            cmd.Parameters.AddWithValue("@p", password);

                            using var reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                              return new User(
                                                                    reader.GetInt32(0),
                                                                    reader.GetString(1),
                                                                    reader.GetString(2),
                                                                    reader.GetString(3),
                                                                    reader.GetString(4));
                            }
                            return null;
              }

              // ─── Libros ───────────────────────────────────────────────────────
              public static List<Book> GetBooksByUser(int userId)
              {
                            var books = new List<Book>();
                            using var conn = CreateConnection();
                            conn.Open();
                            var cmd = conn.CreateCommand();
                            cmd.CommandText = "SELECT * FROM Books WHERE UserId=@uid ORDER BY AddedAt DESC";
                            cmd.Parameters.AddWithValue("@uid", userId);

                            using var reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                              books.Add(new Book(
                                                                    reader.GetInt32(0), reader.GetInt32(1),
                                                                    reader.GetString(2), reader.GetString(3),
                                                                    reader.GetString(4),
                                                                    (BookStatus)reader.GetInt32(5),
                                                                    reader.GetInt32(6), reader.GetString(7)));
                            }
                            return books;
              }

              public static bool AddBook(Book book)
              {
                            try
                            {
                                              using var conn = CreateConnection();
                                              conn.Open();
                                              var cmd = conn.CreateCommand();
                                              cmd.CommandText = @"INSERT INTO Books
                                                                  (UserId,Title,Author,Genre,Status,Rating,Review)
                                                                                      VALUES (@uid,@t,@a,@g,@s,@r,@rv)";
                                              cmd.Parameters.AddWithValue("@uid", book.UserId);
                                              cmd.Parameters.AddWithValue("@t",   book.Title);
                                              cmd.Parameters.AddWithValue("@a",   book.Author);
                                              cmd.Parameters.AddWithValue("@g",   book.Genre);
                                              cmd.Parameters.AddWithValue("@s",   (int)book.Status);
                                              cmd.Parameters.AddWithValue("@r",   book.Rating);
                                              cmd.Parameters.AddWithValue("@rv",  book.Review);
                                              cmd.ExecuteNonQuery();
                                              return true;
                            }
                            catch { return false; }
              }

              public static bool UpdateBook(Book book)
              {
                            try
                            {
                                              using var conn = CreateConnection();
                                              conn.Open();
                                              var cmd = conn.CreateCommand();
                                              cmd.CommandText = @"UPDATE Books SET
                                                                  Title=@t, Author=@a, Genre=@g,
                                                                                      Status=@s, Rating=@r, Review=@rv
                                                                                                          WHERE Id=@id";
                                              cmd.Parameters.AddWithValue("@id", book.Id);
                                              cmd.Parameters.AddWithValue("@t",  book.Title);
                                              cmd.Parameters.AddWithValue("@a",  book.Author);
                                              cmd.Parameters.AddWithValue("@g",  book.Genre);
                                              cmd.Parameters.AddWithValue("@s",  (int)book.Status);
                                              cmd.Parameters.AddWithValue("@r",  book.Rating);
                                              cmd.Parameters.AddWithValue("@rv", book.Review);
                                              cmd.ExecuteNonQuery();
                                              return true;
                            }
                            catch { return false; }
              }

              public static bool DeleteBook(int bookId)
              {
                            try
                            {
                                              using var conn = CreateConnection();
                                              conn.Open();
                                              var cmd = conn.CreateCommand();
                                              cmd.CommandText = "DELETE FROM Books WHERE Id=@id";
                                              cmd.Parameters.AddWithValue("@id", bookId);
                                              cmd.ExecuteNonQuery();
                                              return true;
                            }
                            catch { return false; }
              }

              // ─── Mensajes de chat ─────────────────────────────────────────────
              public static List<Message> GetMessages(int senderId, int receiverId)
              {
                            var msgs = new List<Message>();
                            using var conn = CreateConnection();
                            conn.Open();
                            var cmd = conn.CreateCommand();
                            cmd.CommandText = @"SELECT * FROM Messages
                                            WHERE (SenderId=@s AND ReceiverId=@r)
                                                               OR (SenderId=@r AND ReceiverId=@s)
                                                                               ORDER BY SentAt ASC";
                            cmd.Parameters.AddWithValue("@s", senderId);
                            cmd.Parameters.AddWithValue("@r", receiverId);

                            using var reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                              msgs.Add(new Message(
                                                                    reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2),
                                                                    reader.GetString(3), DateTime.Parse(reader.GetString(4))));
                            }
                            return msgs;
              }

              public static void SaveMessage(Message msg)
              {
                            using var conn = CreateConnection();
                            conn.Open();
                            var cmd = conn.CreateCommand();
                            cmd.CommandText = "INSERT INTO Messages (SenderId,ReceiverId,Content) VALUES (@s,@r,@c)";
                            cmd.Parameters.AddWithValue("@s", msg.SenderId);
                            cmd.Parameters.AddWithValue("@r", msg.ReceiverId);
                            cmd.Parameters.AddWithValue("@c", msg.Content);
                            cmd.ExecuteNonQuery();
              }
    }
}
