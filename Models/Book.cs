// Models/Book.cs
namespace BookManagerCSharp.Models
{
    public enum BookStatus
        {
                Read = 0,
                        ToRead = 1,
                                ToBuy = 2
                                    }

      public class Book
      {
                public int Id { get; set; }
                public int UserId { get; set; }
                public string Title { get; set; } = string.Empty;
                public string Author { get; set; } = string.Empty;
                public string Genre { get; set; } = string.Empty;
                public BookStatus Status { get; set; }
                public int Rating { get; set; }   // 0 = sin valorar, 1-5
                public string Review { get; set; } = string.Empty;
                public DateTime AddedAt { get; set; }

                public Book()
                {
                              AddedAt = DateTime.Now;
                              Rating = 0;
                }

                public Book(int id, int userId, string title, string author,
                                                string genre, BookStatus status, int rating, string review)
                {
                              Id = id;
                              UserId = userId;
                              Title = title;
                              Author = author;
                              Genre = genre;
                              Status = status;
                              Rating = rating;
                              Review = review;
                              AddedAt = DateTime.Now;
                }

                public string StatusLabel() => Status switch
                {
                                BookStatus.Read   => "Leido",
                                BookStatus.ToRead => "Pendiente de leer",
                                BookStatus.ToBuy  => "Pendiente de comprar",
                                _                 => "Desconocido"
                    };

                public override string ToString() =>
                              $"{Title} - {Author} [{StatusLabel()}]";
      }
}
