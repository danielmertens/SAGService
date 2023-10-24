-------------------
-- Album Example --
-------------------

CREATE TABLE MusicAlbums (
    id INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    artist VARCHAR(255) NOT NULL,
    numOfSongs INT,
    releaseYear INT
);

INSERT INTO MusicAlbums (name, artist, numOfSongs, releaseYear) VALUES
('Thriller', 'Michael Jackson', 9, 1982),
('The Dark Side of the Moon', 'Pink Floyd', 10, 1973),
('Abbey Road', 'The Beatles', 17, 1969),
('Rumours', 'Fleetwood Mac', 11, 1977),
('Back in Black', 'AC/DC', 10, 1980),
('Highway to Hell', 'AC/DC', 10, 1979),
('Born to Die', 'Lana Del Rey', 12, 2012),
('Hotel California', 'Eagles', 9, 1976),
('Led Zeppelin IV', 'Led Zeppelin', 8, 1971),
('21', 'Adele', 11, 2011),
('Sgt. Pepper''s Lonely Hearts Club Band', 'The Beatles', 13, 1967);

-------------------
-- Order example --
-------------------

-- Create the Users table
CREATE TABLE Users (
    UserID INT PRIMARY KEY,
    Username VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Address VARCHAR(255)
);

-- Create the Orders table without the TotalAmount column
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY,
    UserID INT,
    OrderDate DATE,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Create the Products table
CREATE TABLE Products (
    ProductID INT PRIMARY KEY,
    ProductName VARCHAR(100) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    Description TEXT
);

-- Create the OrderDetails table for the many-to-many relationship between Orders and Products
CREATE TABLE OrderDetails (
    OrderID INT,
    ProductID INT,
    PRIMARY KEY (OrderID, ProductID),
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- Insert sample data into the Users table
INSERT INTO Users (UserID, Username, Email, Address)
VALUES
    (1, 'JohnDoe', 'johndoe@example.com', '123 Main St'),
    (2, 'JaneSmith', 'janesmith@example.com', '456 Elm St'),
    (3, 'BobJohnson', 'bjohnson@example.com', '789 Oak St');

-- Insert sample data into the Products table
INSERT INTO Products (ProductID, ProductName, Price, Description)
VALUES
    (1, 'Widget A', 19.99, 'A high-quality widget'),
    (2, 'Widget B', 29.99, 'Another great widget'),
    (3, 'Gizmo X', 14.99, 'The latest gizmo'),
    (4, 'Thingamajig Y', 9.99, 'A useful thingamajig');

-- Insert sample data into the Orders table
INSERT INTO Orders (OrderID, UserID, OrderDate)
VALUES
    (1, 1, '2023-10-15'),
    (2, 1, '2023-10-16'),
    (3, 2, '2023-10-16'),
    (4, 3, '2023-10-17');

-- Insert sample data into the OrderDetails table (many-to-many relationship)
INSERT INTO OrderDetails (OrderID, ProductID)
VALUES
    (1, 1),
    (1, 2),
    (2, 3),
    (3, 2),
    (4, 4);
