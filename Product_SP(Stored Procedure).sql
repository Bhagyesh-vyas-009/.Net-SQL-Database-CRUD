--Stored Procedure of ProductTable

--Select all
--[dbo].[PR_Product_SelectAll]

CREATE OR ALTER   PROCEDURE [dbo].[PR_Product_SelectAll]
AS
BEGIN
    SELECT [dbo].[Product].[ProductID],
	[dbo].[Product].[ProductName],
	[dbo].[Product].[ProductPrice],
	[dbo].[Product].[ProductCode],
	[dbo].[Product].[Description],
	[dbo].[User].[UserName]
	FROM [dbo].[Product]
	INNER JOIN [dbo].[User]
	ON [dbo].[Product].[UserID]=[dbo].[User].[UserID]
	ORDER BY [dbo].[Product].[ProductName]
END

--Select by primary key
--[dbo].[PR_Product_SelectByPK] @productID=1
CREATE OR ALTER PROCEDURE [dbo].[PR_Product_SelectByPK]
(@ProductID int)
AS
BEGIN
    SELECT [dbo].[Product].[ProductID],
	[dbo].[Product].[ProductName],
	[dbo].[Product].[ProductPrice],
	[dbo].[Product].[ProductCode],
	[dbo].[Product].[Description],
	[dbo].[Product].[UserID]
	FROM [dbo].[Product]
	WHERE [dbo].[Product].[ProductID]=@ProductID
END

--Insert
--[dbo].[pr_Product_INSERT] @ProductName='Ball Pen',@ProductPrice=10,@ProductCode=01,@Description='Ball Pen',@UserID=2 

CREATE OR ALTER PROCEDURE [dbo].[pr_Product_INSERT]
(@ProductName Varchar(100) ,@ProductPrice Decimal(10,2) ,@ProductCode Varchar(100) ,@Description varchar(100) ,@UserID Int)
AS
BEGIN
	INSERT INTO [dbo].[Product] ([ProductName],[ProductPrice],[ProductCode],[Description],[UserID]) 
	VALUES(@ProductName,@ProductPrice,@ProductCode,@Description,@UserID)
END

EXEC [dbo].[pr_Product_INSERT] @ProductName='Ball Pen',@ProductPrice=10,@ProductCode=01,@Description='Ball Pen',@UserID=2 
EXEC [DBO].[pr_Product_select_all]
EXEC [DBO].[PR_Product_SelectByPK] @ProductID=1

--Update
--[dbo].[pr_Product_UPDATE] @ProductID=1,@ProductName='Ball Pen',@ProductPrice=10,@ProductCode=01,@Description='Ball Pen',@UserID=2 

CREATE OR ALTER PROCEDURE [dbo].[pr_Product_UPDATE]
(@ProductID int,@ProductName Varchar(100) ,@ProductPrice Decimal(10,2) ,@ProductCode Varchar(100) ,@Description varchar(100) ,@UserID Int)
AS
BEGIN
	UPDATE Product set 
	[ProductName]=@ProductName,
	[ProductPrice]=@ProductPrice,
	[ProductCode]=@ProductCode,
	[Description]=@Description,
	[UserID]=@UserID
	where [ProductID]=@ProductID
END

EXEC [dbo].[pr_Product_UPDATE] @ProductID=1,@ProductName='Ball Pen',@ProductPrice=10,@ProductCode=01,@Description='Ball Pen',@UserID=2 

--Delete
--[dbo].[pr_Product_DELETE] @ProductID=1

CREATE OR ALTER PROCEDURE [dbo].[pr_Product_DELETE]
(@ProductID INT)
AS
BEGIN
	DELETE FROM [dbo].[Product] WHERE [dbo].[Product].[ProductID]=@ProductID
END

EXEC [dbo].[pr_Product_DELETE] @ProductID=1
EXEC [dbo].[pr_Product_select_all]
EXEC [dbo].[PR_Product_SelectByPK] @ProductID=2


CREATE OR ALTER PROCEDURE [dbo].[PR_Product_DropDown]
AS
BEGIN
    SELECT
		[dbo].[Product].[ProductID],
        [dbo].[Product].[ProductName]
    FROM
        [dbo].[Product]
END


