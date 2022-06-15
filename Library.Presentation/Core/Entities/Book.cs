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
        public int TimesBorrowed { get; private set; }
        public string? Borrower { get; private set; }
        public DateTime? BorrowDate { get; private set; }
        public DateTime? DueDate { get; private set; }

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
        public Book(long Id, string Title, string Author, int PageCount, int TimesBorrowed, string Borrower, DateTime? BorrowDate, DateTime? DueDate)
        {
            if (string.IsNullOrEmpty(Title))
                throw new ArgumentException("Title must not be null or empty.", nameof(Title));

            if (string.IsNullOrEmpty(Title))
                throw new ArgumentException("Author must not be null or empty.", nameof(Author));

            if (PageCount < 0)
                throw new ArgumentOutOfRangeException(nameof(PageCount), "PageCount must not be negative.");

            if (TimesBorrowed < 0)
                throw new ArgumentOutOfRangeException(nameof(TimesBorrowed), "Times Borrowed must not be negative.");

            if (Borrower == null)
            {
                if (BorrowDate != null)
                    throw new ArgumentException("BorrowDate must be null if borrower is null", nameof(BorrowDate));
                if (DueDate != null)
                    throw new ArgumentException("DueDate must be null if borrower is null", nameof(DueDate));
            }
            if (Borrower != null)
            {
                if (BorrowDate == null)
                    throw new ArgumentException("BorrowDate must not be null if borrower is not null", nameof(BorrowDate));
                if (DueDate == null)
                    throw new ArgumentException("DueDate must not be null if borrower is not null", nameof(DueDate));
            }
            if (BorrowDate != null && DueDate != null && DueDate < BorrowDate)
            {
                throw new ArgumentException("Duedate must be after the borrowdate");
            }
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
            : this(Id, book.Title, book.Author, book.PageCount, book.TimesBorrowed, book.Borrower, book.BorrowDate, book.DueDate)
        { }
        public bool IsOverdue()
        {
            return DateTime.UtcNow > DueDate;
        }
        public TimeSpan GetTimeOverdue()
        {
            if (IsOverdue())
                return DateTime.UtcNow - DueDate.Value;
            else
                return TimeSpan.Zero;
        }
        public decimal GetLateFee()
        {
            decimal fee = 0;
           TimeSpan time =  GetTimeOverdue();
            if (time != TimeSpan.Zero)
                return fee;

            if (time.Minutes<=5 && time.Minutes >= 1)
                fee = (time.Minutes * 10);

            if (fee>50)
                fee=50;

            return fee;
        }
        public void Borrow(string borrower)
        {
            if (!Borrowed)
            {
                TimesBorrowed += 1;
                BorrowDate = DateTime.UtcNow;
                Borrower = borrower;
                DueDate= DateTime.UtcNow.AddSeconds(30);

            }
        }
        public void Return(string borrower)
        {
            if (Borrowed && borrower!=null)
            {
                Borrower = null;
                BorrowDate = null;
                DueDate = null;
            }
        }
        public override string ToString()
        {
            return "Book " + Id + Title;
        }
    }
}