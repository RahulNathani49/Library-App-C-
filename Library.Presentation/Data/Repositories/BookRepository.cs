using Library.Core.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data.Repositories
{
    
    public class BookRepository
    {
        private readonly string connectionString;
        
        public BookRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["LibrarbyDb"].ConnectionString;
        }
        public bool BookExists(long id)
        {

            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select count(id) from books where id = " + "@id;";
            cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = id;
            int count = (int)cmd.ExecuteScalar();
            return count > 0;

        }
        public Book GetBook(long id)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select id,title,author,pagecount,TimesBorrowed,borrower,borrowdate,duedate,created,updated from books where id = " + "@id;";
            cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = id;
            SqlDataReader reader = cmd.ExecuteReader();
            Book book = GetBookFromReader(reader);
            return book;
        }
        public List<Book> GetAllBooks()
        {
            List<Book> BooksList = new List<Book>();
            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from books";
            SqlDataReader reader = cmd.ExecuteReader();
          
            while (reader.Read())
            {
                BooksList.Add(GetBookFromReader(reader));
            }
            return BooksList;
        }
        public List<Book> GetAvailableBooks()
        {
            List<Book> BooksList = new List<Book>();
            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from books where BorrowDate IS NULL";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                BooksList.Add(GetBookFromReader(reader));
            }
            return BooksList;
        }
        public List<Book> GetBorrowedBooks()
        {
            List<Book> BooksList = new List<Book>();
            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from books where borrowdate IS NOT NULL";
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                BooksList.Add(GetBookFromReader(reader));
            }
            return BooksList;
        }
        public List<Book> GetBooksByTitle(string titleFilter)
        {
            List<Book> BooksList = new List<Book>();
            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "select * from books where title like @titlefilter";
            cmd.Parameters.Add("@titlefilter", SqlDbType.NVarChar).Value = $"%{titleFilter}";
            return BooksList;
        }

        private Book GetBookFromReader(SqlDataReader reader)
        {
            return new Book(reader.GetInt64(0),    
                                 reader.GetString(1),
                                 reader.GetString(2),
                                 reader.GetInt32(3),
                                 reader.GetInt32(4),
                                 reader.GetString(5),
                                 reader.GetDateTime(6),
                                 reader.GetDateTime(7)
                                 );
        }

        public Book CreateBook(Book book)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO BOOKS(Title,Author,PageCount,TimesBorrowed,Borrower,BorrowDate,DueDate,Created,Updated) " +
                              "OUTPUT Inserted.Id " +
                              "VALUES(@Title, @Author, @PageCount, @TimesBorrowed, @Borrower, @BorrowDate, @DueDate, @Created, @Updated); ";
            cmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = book.Title;
            cmd.Parameters.Add("@Author", SqlDbType.NVarChar).Value = book.Author;
            cmd.Parameters.Add("@PageCount", SqlDbType.Int).Value = book.PageCount;
            cmd.Parameters.Add("@TimesBorrowed", SqlDbType.Int).Value = book.TimesBorrowed;
            cmd.Parameters.Add("@Borrower", SqlDbType.NVarChar).Value = book.Borrower != null ? book.Borrower : DBNull.Value;
            cmd.Parameters.Add("@BorrowDate", SqlDbType.DateTime2).Value = book.BorrowDate != null ? book.BorrowDate.Value : DBNull.Value;
            cmd.Parameters.Add("@DueDate", SqlDbType.DateTime2).Value = book.DueDate != null ? book.DueDate.Value : DBNull.Value;
            DateTime now = DateTime.UtcNow;
            cmd.Parameters.Add("@Created", SqlDbType.DateTime2).Value = now;
            cmd.Parameters.Add("@Updated", SqlDbType.DateTime2).Value = now;
            long id = (long)cmd.ExecuteScalar();
            return new Book(id, book);
        }
        public bool UpdateBook(Book book)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            using SqlCommand cmd = conn.CreateCommand();
            DateTime now = DateTime.UtcNow;
            cmd.CommandText= "update table books set Title = " + book.Title
                             + ",Author = " + book.Author
                             + ",PageCount = " + book.PageCount
                             + ",TimesBorrowed = " + book.TimesBorrowed
                             + ",Borrower = " + book.Borrower
                             + ",BorrowDate = " + book.BorrowDate
                             + ",DueDate = " + book.DueDate
                             + ",Updated = " +now;
            int i = cmd.ExecuteNonQuery();
            return i != 0;

        }
        public bool DeleteBook(Book book)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            using SqlCommand cmd = conn.CreateCommand();


            cmd.CommandText = "DELETE FROM BOOKS WHERE id = " +
                              "@id;";
            cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = book.Id;

            int i = cmd.ExecuteNonQuery();
            return i!=0;
        }
        public bool DeleteBook(long id)
        {
            using SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM BOOKS WHERE id = " +
                              "@id;";
            cmd.Parameters.Add("@id", SqlDbType.BigInt).Value = id;

            int i = cmd.ExecuteNonQuery();
            return i != 0;
        }

    }
}
