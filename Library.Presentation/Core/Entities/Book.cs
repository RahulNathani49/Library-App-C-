using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Library.Core.Entities
{
    public class Book
    {
        public long Id { get; }
        private static int nextId = 1001;
        public string Title { get; set; }
        public string Author { get; set; }
        public int PageCount { get; set; }
        public int TimesBorrowed { get; set; }
        public string? Borrower { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? DueDate { get; set; }
        public bool Available
        {
            get
            {
                return String.IsNullOrEmpty(Borrower) ? true : false;
            }
        }
        public bool Borrowed
        {
            get
            {
                return !Available;
            }
        }
        public Book(long Id, string Title, string Author, int PageCount, int TimesBorrowed, string Borrower, DateTime BorrowDate, DateTime DueDate)
        {
            this.Id = Id;
            this.Title = Title;
            this.Author = Author;
            this.PageCount = PageCount;
            this.TimesBorrowed = TimesBorrowed;
            this.Borrower = Borrower;
            this.BorrowDate = BorrowDate;
            this.DueDate = DueDate;
        }
        public Book(string Title, string Author, int PageCount, int TimesBorrowed, string Borrower, DateTime BorrowDate, DateTime DueDate)
            : this(nextId++, Title, Author, PageCount, TimesBorrowed, Borrower, BorrowDate, DueDate)
        { }
        public Book(long Id, Book book)
            : this(Id, book.Title, book.Author, book.PageCount, book.TimesBorrowed, book.Borrower, (DateTime)book.BorrowDate, (DateTime)book.DueDate)
        { }
        public override string ToString()
        {
            return "Book " + Id + Title;
        }
    }
}