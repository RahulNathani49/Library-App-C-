using Data.Repositories;
using Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
internal class program
{
    private static void Main()
    {
        Book book = new Book("Harry Potter","Omkar",100,2,"Het",new DateTime(2022,3,3),new DateTime(2022,5,4));
        BookRepository bookRepository = new BookRepository();
        bookRepository.CreateBook(book);
    }
}