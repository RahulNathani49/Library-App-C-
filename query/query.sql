CREATE DATABASE Library_Database
USE Library_Database
CREATE TABLE Books
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1001,1), 
    [Title] VARCHAR(50) NOT NULL, 
    [Author] VARCHAR(50) NOT NULL, 
    [PageCount] INT NOT NULL, 
    [TimesBorrowed] INT NOT NULL, 
    [Borrower] VARCHAR(50) NULL, 
    [BorrowDate] DATETIME NULL, 
    [DueDate] DATETIME NULL, 
    [Created] VARCHAR(50) NOT NULL, 
    [Updated] VARCHAR(50) NOT NULL
)
