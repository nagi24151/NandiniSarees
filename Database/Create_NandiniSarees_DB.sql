/*
  SQL Server database creation script for NandiniSarees e-commerce
  - Creates database NandiniSarees (if not exists)
  - Creates core tables: Users, Roles, UserRoles, Categories, Products,
	ProductImages, Tags, ProductTags, Orders, OrderItems, ShippingAddresses,
	Payments, Reviews, Carts, CartItems

  Run this script in SQL Server Management Studio or via sqlcmd.
*/

SET NOCOUNT ON;

-- Create database if it doesn't exist
IF DB_ID(N'NandiniSarees') IS NULL
BEGIN
	PRINT 'Creating database NandiniSarees';
	CREATE DATABASE NandiniSarees;
END
GO

USE NandiniSareesDb;
GO

-- ==================================================
-- Users
-- ==================================================
IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.Users
	(
		Id INT IDENTITY(1,1) PRIMARY KEY,
		FirstName NVARCHAR(100) NOT NULL,
		LastName NVARCHAR(100) NULL,
		Email NVARCHAR(256) NOT NULL,
		PasswordHash NVARCHAR(512) NULL,
		Phone NVARCHAR(50) NULL,
		IsActive BIT NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT(1),
		CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT SYSUTCDATETIME(),
		UpdatedAt DATETIME2 NULL
	);
	CREATE UNIQUE INDEX UX_Users_Email ON dbo.Users(Email);
END
GO

-- ==================================================
-- Roles and UserRoles
-- ==================================================
IF OBJECT_ID(N'dbo.Roles', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.Roles
	(
		Id INT IDENTITY(1,1) PRIMARY KEY,
		Name NVARCHAR(100) NOT NULL
	);
	CREATE UNIQUE INDEX UX_Roles_Name ON dbo.Roles(Name);
END
GO

IF OBJECT_ID(N'dbo.UserRoles', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.UserRoles
	(
		UserId INT NOT NULL,
		RoleId INT NOT NULL,
		AssignedAt DATETIME2 NOT NULL CONSTRAINT DF_UserRoles_AssignedAt DEFAULT SYSUTCDATETIME(),
		CONSTRAINT PK_UserRoles PRIMARY KEY(UserId, RoleId),
		CONSTRAINT FK_UserRoles_User FOREIGN KEY(UserId) REFERENCES dbo.Users(Id) ON DELETE CASCADE,
		CONSTRAINT FK_UserRoles_Role FOREIGN KEY(RoleId) REFERENCES dbo.Roles(Id) ON DELETE CASCADE
	);
END
GO

-- ==================================================
-- Categories
-- ==================================================
IF OBJECT_ID(N'dbo.Categories', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.Categories
	(
		Id INT IDENTITY(1,1) PRIMARY KEY,
		Name NVARCHAR(200) NOT NULL,
		Slug NVARCHAR(200) NULL,
		Description NVARCHAR(1000) NULL,
		ParentCategoryId INT NULL,
		IsActive BIT NOT NULL CONSTRAINT DF_Categories_IsActive DEFAULT(1)
	);
	CREATE INDEX IX_Categories_Parent ON dbo.Categories(ParentCategoryId);
	ALTER TABLE dbo.Categories ADD CONSTRAINT FK_Categories_Parent FOREIGN KEY(ParentCategoryId) REFERENCES dbo.Categories(Id);
END
GO

-- ==================================================
-- Products
-- ==================================================
IF OBJECT_ID(N'dbo.Products', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.Products
	(
		Id INT IDENTITY(1,1) PRIMARY KEY,
		Name NVARCHAR(300) NOT NULL,
		SKU NVARCHAR(100) NULL,
		Description NVARCHAR(MAX) NULL,
		CategoryId INT NULL,
		Price DECIMAL(18,2) NOT NULL CONSTRAINT DF_Products_Price DEFAULT(0.00),
		DiscountPrice DECIMAL(18,2) NULL,
		Stock INT NOT NULL CONSTRAINT DF_Products_Stock DEFAULT(0),
		IsActive BIT NOT NULL CONSTRAINT DF_Products_IsActive DEFAULT(1),
		CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Products_CreatedAt DEFAULT SYSUTCDATETIME(),
		UpdatedAt DATETIME2 NULL
	);
	CREATE INDEX IX_Products_Category ON dbo.Products(CategoryId);
	CREATE UNIQUE INDEX UX_Products_SKU ON dbo.Products(SKU) WHERE SKU IS NOT NULL;
	ALTER TABLE dbo.Products ADD CONSTRAINT FK_Products_Category FOREIGN KEY(CategoryId) REFERENCES dbo.Categories(Id) ON DELETE SET NULL;
END
GO

-- ==================================================
-- ProductImages
-- ==================================================
IF OBJECT_ID(N'dbo.ProductImages', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.ProductImages
	(
		Id INT IDENTITY(1,1) PRIMARY KEY,
		ProductId INT NOT NULL,
		Url NVARCHAR(1000) NOT NULL,
		AltText NVARCHAR(500) NULL,
		SortOrder INT NOT NULL DEFAULT(0),
		IsPrimary BIT NOT NULL DEFAULT(0)
	);
	CREATE INDEX IX_ProductImages_Product ON dbo.ProductImages(ProductId);
	ALTER TABLE dbo.ProductImages ADD CONSTRAINT FK_ProductImages_Product FOREIGN KEY(ProductId) REFERENCES dbo.Products(Id) ON DELETE CASCADE;
END
GO

-- ==================================================
-- Tags and ProductTags
-- ==================================================
IF OBJECT_ID(N'dbo.Tags', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.Tags
	(
		Id INT IDENTITY(1,1) PRIMARY KEY,
		Name NVARCHAR(200) NOT NULL
	);
	CREATE UNIQUE INDEX UX_Tags_Name ON dbo.Tags(Name);
END
GO

IF OBJECT_ID(N'dbo.ProductTags', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.ProductTags
	(
		ProductId INT NOT NULL,
		TagId INT NOT NULL,
		CONSTRAINT PK_ProductTags PRIMARY KEY(ProductId, TagId),
		CONSTRAINT FK_ProductTags_Product FOREIGN KEY(ProductId) REFERENCES dbo.Products(Id) ON DELETE CASCADE,
		CONSTRAINT FK_ProductTags_Tag FOREIGN KEY(TagId) REFERENCES dbo.Tags(Id) ON DELETE CASCADE
	);
END
GO

-- ==================================================
-- Orders and OrderItems
-- ==================================================
IF OBJECT_ID(N'dbo.Orders', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.Orders
	(
		Id INT IDENTITY(1,1) PRIMARY KEY,
		OrderNumber NVARCHAR(50) NOT NULL,
		UserId INT NULL,
		OrderDate DATETIME2 NOT NULL CONSTRAINT DF_Orders_OrderDate DEFAULT SYSUTCDATETIME(),
		Status NVARCHAR(50) NOT NULL DEFAULT('Pending'),
		Subtotal DECIMAL(18,2) NOT NULL DEFAULT(0.00),
		Shipping DECIMAL(18,2) NOT NULL DEFAULT(0.00),
		Tax DECIMAL(18,2) NOT NULL DEFAULT(0.00),
		Total DECIMAL(18,2) NOT NULL DEFAULT(0.00),
		ShippingAddressId INT NULL,
		PaymentId INT NULL
	);
	CREATE UNIQUE INDEX UX_Orders_OrderNumber ON dbo.Orders(OrderNumber);
	CREATE INDEX IX_Orders_User ON dbo.Orders(UserId);
	ALTER TABLE dbo.Orders ADD CONSTRAINT FK_Orders_User FOREIGN KEY(UserId) REFERENCES dbo.Users(Id) ON DELETE SET NULL;
END
GO

IF OBJECT_ID(N'dbo.OrderItems', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.OrderItems
	(
		Id INT IDENTITY(1,1) PRIMARY KEY,
		OrderId INT NOT NULL,
		ProductId INT NOT NULL,
		Quantity INT NOT NULL,
		UnitPrice DECIMAL(18,2) NOT NULL,
		TotalPrice AS (Quantity * UnitPrice) PERSISTED
	);
	CREATE INDEX IX_OrderItems_Order ON dbo.OrderItems(OrderId);
	ALTER TABLE dbo.OrderItems ADD CONSTRAINT FK_OrderItems_Order FOREIGN KEY(OrderId) REFERENCES dbo.Orders(Id) ON DELETE CASCADE;
	ALTER TABLE dbo.OrderItems ADD CONSTRAINT FK_OrderItems_Product FOREIGN KEY(ProductId) REFERENCES dbo.Products(Id) ON DELETE NO ACTION;
END
GO

-- ==================================================
-- ShippingAddresses
-- ==================================================
IF OBJECT_ID(N'dbo.ShippingAddresses', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.ShippingAddresses
	(
		Id INT IDENTITY(1,1) PRIMARY KEY,
		UserId INT NULL,
		FullName NVARCHAR(200) NOT NULL,
		AddressLine1 NVARCHAR(500) NOT NULL,
		AddressLine2 NVARCHAR(500) NULL,
		City NVARCHAR(200) NOT NULL,
		State NVARCHAR(200) NULL,
		PostalCode NVARCHAR(50) NULL,
		Country NVARCHAR(200) NOT NULL,
		Phone NVARCHAR(50) NULL
	);
	CREATE INDEX IX_ShippingAddresses_User ON dbo.ShippingAddresses(UserId);
	ALTER TABLE dbo.ShippingAddresses ADD CONSTRAINT FK_ShippingAddresses_User FOREIGN KEY(UserId) REFERENCES dbo.Users(Id) ON DELETE SET NULL;
END
GO

-- ==================================================
-- Payments
-- ==================================================
IF OBJECT_ID(N'dbo.Payments', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.Payments
	(
		Id INT IDENTITY(1,1) PRIMARY KEY,
		OrderId INT NULL,
		UserId INT NULL,
		PaymentMethod NVARCHAR(100) NULL,
		Amount DECIMAL(18,2) NOT NULL,
		PaidAt DATETIME2 NULL,
		Status NVARCHAR(50) NULL
	);
	CREATE INDEX IX_Payments_Order ON dbo.Payments(OrderId);
	ALTER TABLE dbo.Payments ADD CONSTRAINT FK_Payments_Order FOREIGN KEY(OrderId) REFERENCES dbo.Orders(Id) ON DELETE SET NULL;
	ALTER TABLE dbo.Payments ADD CONSTRAINT FK_Payments_User FOREIGN KEY(UserId) REFERENCES dbo.Users(Id) ON DELETE SET NULL;
END
GO

-- ==================================================
-- Reviews
-- ==================================================
IF OBJECT_ID(N'dbo.Reviews', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.Reviews
	(
		Id INT IDENTITY(1,1) PRIMARY KEY,
		ProductId INT NOT NULL,
		UserId INT NULL,
		Rating TINYINT NOT NULL CHECK (Rating >= 1 AND Rating <= 5),
		Title NVARCHAR(200) NULL,
		Body NVARCHAR(MAX) NULL,
		CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Reviews_CreatedAt DEFAULT SYSUTCDATETIME()
	);
	CREATE INDEX IX_Reviews_Product ON dbo.Reviews(ProductId);
	ALTER TABLE dbo.Reviews ADD CONSTRAINT FK_Reviews_Product FOREIGN KEY(ProductId) REFERENCES dbo.Products(Id) ON DELETE CASCADE;
	ALTER TABLE dbo.Reviews ADD CONSTRAINT FK_Reviews_User FOREIGN KEY(UserId) REFERENCES dbo.Users(Id) ON DELETE SET NULL;
END
GO

-- ==================================================
-- Carts and CartItems (optional temporary carts)
-- ==================================================
IF OBJECT_ID(N'dbo.Carts', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.Carts
	(
		Id INT IDENTITY(1,1) PRIMARY KEY,
		UserId INT NULL,
		CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Carts_CreatedAt DEFAULT SYSUTCDATETIME()
	);
	ALTER TABLE dbo.Carts ADD CONSTRAINT FK_Carts_User FOREIGN KEY(UserId) REFERENCES dbo.Users(Id) ON DELETE SET NULL;
END
GO

IF OBJECT_ID(N'dbo.CartItems', N'U') IS NULL
BEGIN
	CREATE TABLE dbo.CartItems
	(
		Id INT IDENTITY(1,1) PRIMARY KEY,
		CartId INT NOT NULL,
		ProductId INT NOT NULL,
		Quantity INT NOT NULL,
		UnitPrice DECIMAL(18,2) NOT NULL
	);
	CREATE INDEX IX_CartItems_Cart ON dbo.CartItems(CartId);
	ALTER TABLE dbo.CartItems ADD CONSTRAINT FK_CartItems_Cart FOREIGN KEY(CartId) REFERENCES dbo.Carts(Id) ON DELETE CASCADE;
	ALTER TABLE dbo.CartItems ADD CONSTRAINT FK_CartItems_Product FOREIGN KEY(ProductId) REFERENCES dbo.Products(Id) ON DELETE NO ACTION;
END
GO

PRINT 'NandiniSarees database schema creation completed.';
