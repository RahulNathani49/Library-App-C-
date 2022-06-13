using Data.Repositories;
using Library.Core.Entities;

internal class program
{
    public static void Main()
    {
        BookRepository bookRepository = new BookRepository();
        Book book = new Book("Harry Potter", "Omkar", 100, 2, "Het", new DateTime(2022, 2, 21), new DateTime(2022, 3, 10));
        bookRepository.CreateBook(book);
    }
}