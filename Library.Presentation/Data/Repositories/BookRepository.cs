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
            return false;
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
            return false;
        }
        public bool DeleteBook(Book book)
        {
            return false;
        }
        public bool DeleteBook(long id)
        {
            return false;
        }


    }


}
