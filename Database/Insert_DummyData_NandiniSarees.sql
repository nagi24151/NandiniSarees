-- Insert_DummyData_NandiniSarees.sql
-- Inserts sample/dummy data for NandiniSarees database tables
-- Run this script after the schema creation script. It is safe to re-run: inserts check for existence by unique keys.

SET NOCOUNT ON;

USE NandiniSareesDb;

BEGIN TRANSACTION;

-- ===== Users =====
DECLARE @UserAlice INT, @UserBob INT, @UserManager INT;

IF NOT EXISTS(SELECT 1 FROM dbo.Users WHERE Email = 'alice@example.com')
BEGIN
	INSERT INTO dbo.Users(FirstName, LastName, Email, PasswordHash, Phone)
	VALUES('Alice','Kumar','alice@example.com','hashedpwd1','+919000000001');
END
SELECT @UserAlice = Id FROM dbo.Users WHERE Email = 'alice@example.com';

IF NOT EXISTS(SELECT 1 FROM dbo.Users WHERE Email = 'bob@example.com')
BEGIN
	INSERT INTO dbo.Users(FirstName, LastName, Email, PasswordHash, Phone)
	VALUES('Bob','Sharma','bob@example.com','hashedpwd2','+919000000002');
END
SELECT @UserBob = Id FROM dbo.Users WHERE Email = 'bob@example.com';

IF NOT EXISTS(SELECT 1 FROM dbo.Users WHERE Email = 'manager@example.com')
BEGIN
	INSERT INTO dbo.Users(FirstName, LastName, Email, PasswordHash, Phone)
	VALUES('Manju','Patel','manager@example.com','hashedpwd3','+919000000003');
END
SELECT @UserManager = Id FROM dbo.Users WHERE Email = 'manager@example.com';

-- ===== Roles & UserRoles =====
DECLARE @RoleAdmin INT, @RoleCustomer INT;

IF NOT EXISTS(SELECT 1 FROM dbo.Roles WHERE Name = 'Admin')
	INSERT INTO dbo.Roles(Name) VALUES('Admin');
SELECT @RoleAdmin = Id FROM dbo.Roles WHERE Name = 'Admin';

IF NOT EXISTS(SELECT 1 FROM dbo.Roles WHERE Name = 'Customer')
	INSERT INTO dbo.Roles(Name) VALUES('Customer');
SELECT @RoleCustomer = Id FROM dbo.Roles WHERE Name = 'Customer';

IF NOT EXISTS(SELECT 1 FROM dbo.UserRoles WHERE UserId = @UserManager AND RoleId = @RoleAdmin)
	INSERT INTO dbo.UserRoles(UserId, RoleId) VALUES(@UserManager, @RoleAdmin);

IF NOT EXISTS(SELECT 1 FROM dbo.UserRoles WHERE UserId = @UserAlice AND RoleId = @RoleCustomer)
	INSERT INTO dbo.UserRoles(UserId, RoleId) VALUES(@UserAlice, @RoleCustomer);

IF NOT EXISTS(SELECT 1 FROM dbo.UserRoles WHERE UserId = @UserBob AND RoleId = @RoleCustomer)
	INSERT INTO dbo.UserRoles(UserId, RoleId) VALUES(@UserBob, @RoleCustomer);

-- ===== Categories =====
DECLARE @CatSarees INT, @CatSilk INT, @CatCotton INT, @CatBlouse INT;

IF NOT EXISTS(SELECT 1 FROM dbo.Categories WHERE Name = 'Sarees')
	INSERT INTO dbo.Categories(Name, Slug, Description, ParentCategoryId) VALUES('Sarees','sarees','All sarees',NULL);
SELECT @CatSarees = Id FROM dbo.Categories WHERE Name = 'Sarees';

IF NOT EXISTS(SELECT 1 FROM dbo.Categories WHERE Name = 'Silk Sarees')
	INSERT INTO dbo.Categories(Name, Slug, Description, ParentCategoryId) VALUES('Silk Sarees','silk-sarees','Silk sarees collection',@CatSarees);
SELECT @CatSilk = Id FROM dbo.Categories WHERE Name = 'Silk Sarees';

IF NOT EXISTS(SELECT 1 FROM dbo.Categories WHERE Name = 'Cotton Sarees')
	INSERT INTO dbo.Categories(Name, Slug, Description, ParentCategoryId) VALUES('Cotton Sarees','cotton-sarees','Lightweight cotton sarees',@CatSarees);
SELECT @CatCotton = Id FROM dbo.Categories WHERE Name = 'Cotton Sarees';

IF NOT EXISTS(SELECT 1 FROM dbo.Categories WHERE Name = 'Blouses')
	INSERT INTO dbo.Categories(Name, Slug, Description, ParentCategoryId) VALUES('Blouses','blouses','Blouse designs and pieces',@CatSarees);
SELECT @CatBlouse = Id FROM dbo.Categories WHERE Name = 'Blouses';

-- ===== Products =====
DECLARE @P1 INT, @P2 INT, @P3 INT, @P4 INT, @P5 INT;

IF NOT EXISTS(SELECT 1 FROM dbo.Products WHERE SKU = 'NS-SLK-001')
	INSERT INTO dbo.Products(Name, SKU, Description, CategoryId, Price, DiscountPrice, Stock)
	VALUES('Kanjivaram Silk - Red','NS-SLK-001','Traditional Kanjivaram silk saree in red',@CatSilk,12500.00,0,10);
SELECT @P1 = Id FROM dbo.Products WHERE SKU = 'NS-SLK-001';

IF NOT EXISTS(SELECT 1 FROM dbo.Products WHERE SKU = 'NS-SLK-002')
	INSERT INTO dbo.Products(Name, SKU, Description, CategoryId, Price, DiscountPrice, Stock)
	VALUES('Benarasi Silk - Gold','NS-SLK-002','Opulent Benarasi saree with gold zari',@CatSilk,15500.00,14000.00,5);
SELECT @P2 = Id FROM dbo.Products WHERE SKU = 'NS-SLK-002';

IF NOT EXISTS(SELECT 1 FROM dbo.Products WHERE SKU = 'NS-CTN-001')
	INSERT INTO dbo.Products(Name, SKU, Description, CategoryId, Price, Stock)
	VALUES('Mul Cotton - Pastel','NS-CTN-001','Soft mul cotton saree, everyday wear',@CatCotton,2200.00,25);
SELECT @P3 = Id FROM dbo.Products WHERE SKU = 'NS-CTN-001';

IF NOT EXISTS(SELECT 1 FROM dbo.Products WHERE SKU = 'NS-BLS-001')
	INSERT INTO dbo.Products(Name, SKU, Description, CategoryId, Price, Stock)
	VALUES('Embroidered Blouse - Maroon','NS-BLS-001','Matching blouse piece with embroidery',@CatBlouse,850.00,50);
SELECT @P4 = Id FROM dbo.Products WHERE SKU = 'NS-BLS-001';

IF NOT EXISTS(SELECT 1 FROM dbo.Products WHERE SKU = 'NS-SLK-003')
	INSERT INTO dbo.Products(Name, SKU, Description, CategoryId, Price, DiscountPrice, Stock)
	VALUES('Tussar Silk - Beige','NS-SLK-003','Lightweight tussar silk saree',@CatSilk,7600.00,7000.00,8);
SELECT @P5 = Id FROM dbo.Products WHERE SKU = 'NS-SLK-003';

-- ===== ProductImages =====
IF NOT EXISTS(SELECT 1 FROM dbo.ProductImages WHERE ProductId = @P1 AND Url LIKE '%kanjivaram-red%')
	INSERT INTO dbo.ProductImages(ProductId, Url, AltText, SortOrder, IsPrimary)
	VALUES(@P1,'/images/products/kanjivaram-red-1.jpg','Kanjivaram red front',0,1);

IF NOT EXISTS(SELECT 1 FROM dbo.ProductImages WHERE ProductId = @P2)
	INSERT INTO dbo.ProductImages(ProductId, Url, AltText, SortOrder, IsPrimary)
	VALUES(@P2,'/images/products/benarasi-gold-1.jpg','Benarasi front',0,1);

IF NOT EXISTS(SELECT 1 FROM dbo.ProductImages WHERE ProductId = @P3)
	INSERT INTO dbo.ProductImages(ProductId, Url, AltText, SortOrder, IsPrimary)
	VALUES(@P3,'/images/products/mulcotton-pastel-1.jpg','Mul cotton front',0,1);

IF NOT EXISTS(SELECT 1 FROM dbo.ProductImages WHERE ProductId = @P4)
	INSERT INTO dbo.ProductImages(ProductId, Url, AltText, SortOrder, IsPrimary)
	VALUES(@P4,'/images/products/blouse-maroon-1.jpg','Embroidered blouse',0,1);

IF NOT EXISTS(SELECT 1 FROM dbo.ProductImages WHERE ProductId = @P5)
	INSERT INTO dbo.ProductImages(ProductId, Url, AltText, SortOrder, IsPrimary)
	VALUES(@P5,'/images/products/tussar-beige-1.jpg','Tussar beige',0,1);

-- ===== Tags & ProductTags =====
DECLARE @TagTrad INT, @TagParty INT, @TagCotton INT, @TagSilk INT;

IF NOT EXISTS(SELECT 1 FROM dbo.Tags WHERE Name = 'Traditional') INSERT INTO dbo.Tags(Name) VALUES('Traditional');
SELECT @TagTrad = Id FROM dbo.Tags WHERE Name = 'Traditional';

IF NOT EXISTS(SELECT 1 FROM dbo.Tags WHERE Name = 'PartyWear') INSERT INTO dbo.Tags(Name) VALUES('PartyWear');
SELECT @TagParty = Id FROM dbo.Tags WHERE Name = 'PartyWear';

IF NOT EXISTS(SELECT 1 FROM dbo.Tags WHERE Name = 'Cotton') INSERT INTO dbo.Tags(Name) VALUES('Cotton');
SELECT @TagCotton = Id FROM dbo.Tags WHERE Name = 'Cotton';

IF NOT EXISTS(SELECT 1 FROM dbo.Tags WHERE Name = 'Silk') INSERT INTO dbo.Tags(Name) VALUES('Silk');
SELECT @TagSilk = Id FROM dbo.Tags WHERE Name = 'Silk';

-- Map tags to products
IF NOT EXISTS(SELECT 1 FROM dbo.ProductTags WHERE ProductId = @P1 AND TagId = @TagSilk) INSERT INTO dbo.ProductTags(ProductId, TagId) VALUES(@P1, @TagSilk);
IF NOT EXISTS(SELECT 1 FROM dbo.ProductTags WHERE ProductId = @P1 AND TagId = @TagTrad) INSERT INTO dbo.ProductTags(ProductId, TagId) VALUES(@P1, @TagTrad);
IF NOT EXISTS(SELECT 1 FROM dbo.ProductTags WHERE ProductId = @P2 AND TagId = @TagSilk) INSERT INTO dbo.ProductTags(ProductId, TagId) VALUES(@P2, @TagSilk);
IF NOT EXISTS(SELECT 1 FROM dbo.ProductTags WHERE ProductId = @P2 AND TagId = @TagParty) INSERT INTO dbo.ProductTags(ProductId, TagId) VALUES(@P2, @TagParty);
IF NOT EXISTS(SELECT 1 FROM dbo.ProductTags WHERE ProductId = @P3 AND TagId = @TagCotton) INSERT INTO dbo.ProductTags(ProductId, TagId) VALUES(@P3, @TagCotton);
IF NOT EXISTS(SELECT 1 FROM dbo.ProductTags WHERE ProductId = @P4 AND TagId = @TagTrad) INSERT INTO dbo.ProductTags(ProductId, TagId) VALUES(@P4, @TagTrad);
IF NOT EXISTS(SELECT 1 FROM dbo.ProductTags WHERE ProductId = @P5 AND TagId = @TagSilk) INSERT INTO dbo.ProductTags(ProductId, TagId) VALUES(@P5, @TagSilk);

-- ===== ShippingAddresses =====
DECLARE @Ship1 INT;
IF NOT EXISTS(SELECT 1 FROM dbo.ShippingAddresses WHERE FullName = 'Alice Kumar' AND Phone = '+919000000001')
	INSERT INTO dbo.ShippingAddresses(UserId, FullName, AddressLine1, City, State, PostalCode, Country, Phone)
	VALUES(@UserAlice, 'Alice Kumar', '12 MG Road', 'Bengaluru', 'Karnataka', '560001', 'India', '+919000000001');
SELECT @Ship1 = Id FROM dbo.ShippingAddresses WHERE UserId = @UserAlice;

-- ===== Orders & OrderItems =====
DECLARE @Order1 INT;

IF NOT EXISTS(SELECT 1 FROM dbo.Orders WHERE OrderNumber = 'NS-ORD-1001')
BEGIN
	INSERT INTO dbo.Orders(OrderNumber, UserId, Status, Subtotal, Shipping, Tax, Total, ShippingAddressId)
	VALUES('NS-ORD-1001', @UserAlice, 'Processing', 0.00, 50.00, 0.00, 0.00, @Ship1);
END
SELECT @Order1 = Id FROM dbo.Orders WHERE OrderNumber = 'NS-ORD-1001';

-- Add items to the order
IF NOT EXISTS(SELECT 1 FROM dbo.OrderItems WHERE OrderId = @Order1 AND ProductId = @P1)
	INSERT INTO dbo.OrderItems(OrderId, ProductId, Quantity, UnitPrice) VALUES(@Order1, @P1, 1, (SELECT Price FROM dbo.Products WHERE Id = @P1));

IF NOT EXISTS(SELECT 1 FROM dbo.OrderItems WHERE OrderId = @Order1 AND ProductId = @P4)
	INSERT INTO dbo.OrderItems(OrderId, ProductId, Quantity, UnitPrice) VALUES(@Order1, @P4, 2, (SELECT Price FROM dbo.Products WHERE Id = @P4));

-- Recalculate order totals (simple aggregation)
UPDATE dbo.Orders
SET Subtotal = ISNULL((SELECT SUM(Quantity * UnitPrice) FROM dbo.OrderItems WHERE OrderId = dbo.Orders.Id),0),
	Tax = ROUND(ISNULL((SELECT SUM(Quantity * UnitPrice) FROM dbo.OrderItems WHERE OrderId = dbo.Orders.Id) * 0.05,0),2),
	Total = ISNULL((SELECT SUM(Quantity * UnitPrice) FROM dbo.OrderItems WHERE OrderId = dbo.Orders.Id),0) + Shipping + ROUND(ISNULL((SELECT SUM(Quantity * UnitPrice) FROM dbo.OrderItems WHERE OrderId = dbo.Orders.Id) * 0.05,0),2)
WHERE Id = @Order1;

-- ===== Payments =====
DECLARE @Payment1 INT;
IF NOT EXISTS(SELECT 1 FROM dbo.Payments WHERE OrderId = @Order1 AND Amount = (SELECT Total FROM dbo.Orders WHERE Id = @Order1))
BEGIN
	INSERT INTO dbo.Payments(OrderId, UserId, PaymentMethod, Amount, PaidAt, Status)
	VALUES(@Order1, @UserAlice, 'CreditCard', (SELECT Total FROM dbo.Orders WHERE Id = @Order1), SYSUTCDATETIME(), 'Paid');
END
SELECT @Payment1 = Id FROM dbo.Payments WHERE OrderId = @Order1;

-- Link payment to order
UPDATE dbo.Orders SET PaymentId = @Payment1 WHERE Id = @Order1 AND PaymentId IS NULL;

-- ===== Reviews =====
IF NOT EXISTS(SELECT 1 FROM dbo.Reviews WHERE ProductId = @P1 AND UserId = @UserAlice)
	INSERT INTO dbo.Reviews(ProductId, UserId, Rating, Title, Body)
	VALUES(@P1, @UserAlice, 5, 'Beautiful Kanjivaram', 'The saree quality and finishing are excellent.');

IF NOT EXISTS(SELECT 1 FROM dbo.Reviews WHERE ProductId = @P3 AND UserId = @UserBob)
	INSERT INTO dbo.Reviews(ProductId, UserId, Rating, Title, Body)
	VALUES(@P3, @UserBob, 4, 'Comfortable for daily use', 'Lightweight and breathable cotton saree.');

-- ===== Carts & CartItems =====
DECLARE @CartAlice INT;
IF NOT EXISTS(SELECT 1 FROM dbo.Carts WHERE UserId = @UserAlice)
	INSERT INTO dbo.Carts(UserId) VALUES(@UserAlice);
SELECT @CartAlice = Id FROM dbo.Carts WHERE UserId = @UserAlice;

IF NOT EXISTS(SELECT 1 FROM dbo.CartItems WHERE CartId = @CartAlice AND ProductId = @P5)
	INSERT INTO dbo.CartItems(CartId, ProductId, Quantity, UnitPrice) VALUES(@CartAlice, @P5, 1, (SELECT Price FROM dbo.Products WHERE Id = @P5));

COMMIT TRANSACTION;

PRINT 'Dummy data insertion completed.';
