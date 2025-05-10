CREATE UNIQUE INDEX UX_Books_ISBN
ON Books (ISBN);
GO;

CREATE UNIQUE INDEX UX_Customers_CustomerCode
ON Customers (CustomerCode);
GO;

ALTER TABLE Customers
ADD CONSTRAINT CK_Customers_TrustValue CHECK (TrustValue IN (1,2,3));
GO;

ALTER TABLE Customers
ADD CONSTRAINT DF_Customers_TrustValue DEFAULT 1 FOR TrustValue;
INSERT INTO Books (Title, Description, Author, Genre, ISBN, MaxQuantity, AvailableQuantity, NumberOfReturnDays, CreatedOn, CreatedBy)
VALUES ('Almanacul', 'Revista secolului!', 'Dorian Popa', 'Comedy', '100-100-100-100-0', 122, 100, 10, SYSDATETIME(), 'Admin');
GO;
INSERT INTO Books (Title, Description, Author, Genre, ISBN, MaxQuantity, AvailableQuantity, NumberOfReturnDays, CreatedOn, CreatedBy)
VALUES ('Baltagul', 'Virtutea Vitorei Lipan.', 'Mihail Sadoveanu', 'Crime-Novel', '100-100-100-101-0', 150, 103, 14, SYSDATETIME(), 'Admin');
GO;
INSERT INTO Books (Title, Description, Author, Genre, ISBN, MaxQuantity, AvailableQuantity, NumberOfReturnDays, CreatedOn, CreatedBy)
VALUES ('Motan Calator', 'Traducere de Raluca Nicolae', 'Hiro Arikawa', 'Comedy', '978-606-097-299-0', 133, 99, 7, SYSDATETIME(), 'Admin');
GO;
INSERT INTO Books (Title, Description, Author, Genre, ISBN, MaxQuantity, AvailableQuantity, NumberOfReturnDays, CreatedOn, CreatedBy)
VALUES ('Dune', 'Most well aclaimed Sci-Fi novel.', 'Frank Herbert', 'Sci-Fi', '978-606-097-299-1', 133, 99, 7, SYSDATETIME(), 'Admin');
GO;
INSERT INTO Books (Title, Description, Author, Genre, ISBN, MaxQuantity, AvailableQuantity, NumberOfReturnDays, CreatedOn, CreatedBy)
VALUES ('Razboiul Cipurilor', 'Lupta pentru cea mai importanta tehnologie!', 'Chris Miller', 'Science', '978-606-097-299-2', 133, 99, 7, SYSDATETIME(), 'Admin');
GO;

INSERT INTO Customers (CustomerCode, Name, Address, City, Email, CreatedOn, CreatedBy)
VALUES (1000, 'Gabriel Pasca', 'Pestera Ghetarului', 'Campeni', 'gabrielpasca@yahoo.com', SYSDATETIME(), 'Admin');
GO;

INSERT INTO Customers (CustomerCode, Name, Address, City, Email, CreatedOn, CreatedBy)
VALUES (1001, 'Teodor Gut', 'Donat 11', 'Cluj-Napoca', 'teodorgut@gmail.ro', SYSDATETIME(), 'Admin');
GO;

INSERT INTO Customers (CustomerCode, Name, Address, City, Email, CreatedOn, CreatedBy)
VALUES (1002, 'Lupu David', 'Buna Ziua', 'Cluj-Napoca', 'dslupu26@gmail.com', SYSDATETIME(), 'Admin');
GO;

INSERT INTO Rents (BookId, CustomerId , ReturnDate,CreatedOn, CreatedBy)
VALUES (1, 1, DATEADD(DAY, 7, SYSDATETIME()), SYSDATETIME(), 'Admin');
GO;

INSERT INTO Rents (BookId, CustomerId , ReturnDate,CreatedOn, CreatedBy)
VALUES (2, 1, DATEADD(DAY, -7, SYSDATETIME()), SYSDATETIME(), 'Admin');
GO;

INSERT INTO Rents (BookId, CustomerId , ReturnDate,CreatedOn, CreatedBy)
VALUES (3, 1, DATEADD(DAY, -2, SYSDATETIME()), SYSDATETIME(), 'Admin');
GO;

INSERT INTO Rents (BookId, CustomerId , ReturnDate,CreatedOn, CreatedBy)
VALUES (4, 2, DATEADD(DAY, 7, SYSDATETIME()), SYSDATETIME(), 'Admin');
GO;

INSERT INTO Rents (BookId, CustomerId , ReturnDate,CreatedOn, CreatedBy)
VALUES (5, 3, DATEADD(DAY, 7, SYSDATETIME()), SYSDATETIME(), 'Admin');
GO;