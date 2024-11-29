--Stored Procedure of OrderDetail Table

--Select all--
--[dbo].[PR_OrderDetail_SelectAll]

CREATE OR ALTER     PROCEDURE [dbo].[PR_OrderDetail_SelectAll]
AS
BEGIN
    SELECT [dbo].[ORDERDETAIL].[OrderDetailID],
	[dbo].[ORDER].[OrderNumber],
	[dbo].[Product].[ProductName],
	[dbo].[ORDERDETAIL].[Quantity],
	[dbo].[ORDERDETAIL].[Amount],
	[dbo].[ORDERDETAIL].[TotalAmount],
	[dbo].[User].[UserName]
	FROM [dbo].[ORDERDETAIL]
	INNER JOIN [dbo].[User]
	ON [dbo].[ORDERDETAIL].[UserID]=[dbo].[User].[UserID]
	INNER JOIN [dbo].[Product]
	ON [dbo].[ORDERDETAIL].[ProductID]=[dbo].[Product].[ProductID]
	INNER JOIN [DBO].[ORDER]
	ON [DBO].[ORDER].[OrderID]=[DBO].[ORDERDETAIL].[OrderID]
	ORDER BY [dbo].[ORDERDETAIL].[Amount]
END

--Select by primary key--
--[dbo].[PR_OrderDetail_SelectByPK] @OrderDetailID=1
CREATE OR ALTER PROCEDURE [dbo].[PR_OrderDetail_SelectByPK]
(@OrderDetailID int)
AS
BEGIN
    SELECT [dbo].[ORDERDETAIL].[OrderDetailID],
	[dbo].[ORDERDETAIL].[OrderID],
	[dbo].[ORDERDETAIL].[ProductID],
	[dbo].[ORDERDETAIL].[Quantity],
	[dbo].[ORDERDETAIL].[Amount],
	[dbo].[ORDERDETAIL].[TotalAmount],
	[dbo].[ORDERDETAIL].[UserID]
	FROM [dbo].[ORDERDETAIL]
	where [dbo].[ORDERDETAIL].[OrderDetailID]=@OrderDetailID
END

---Insert--
--[dbo].[pr_OrderDetail_INSERT] @OrderID=3,@ProductID=2,@Quantity=2,@Amount=500,@TotalAmount=1000,@UserID=2
CREATE OR ALTER PROCEDURE [dbo].[pr_OrderDetail_INSERT]
(@OrderID Int , @ProductID Int , @Quantity Int ,@Amount Decimal(10,2) ,@TotalAmount Decimal(10,2) ,@UserID Int)
AS
BEGIN
	INSERT INTO [dbo].[ORDERDETAIL] ([OrderID],[ProductID],[Quantity],[Amount],[TotalAmount],[UserID]) 
	VALUES(@OrderID,@ProductID,@Quantity,@Amount,@TotalAmount,@UserID)
END

EXEC [dbo].[pr_OrderDetail_INSERT] @OrderID=4,@ProductID=2,@Quantity=2,@Amount=500,@TotalAmount=1000,@UserID=2

EXEC [dbo].[pr_OrderDetail_select_all]
EXEC [DBO].[pr_OrderDetail_select_by_pk] @OrderDetailID=4

 
--Update
--[dbo].[pr_OrderDetail_UPDATE] @OrderDetailID=4,@OrderID=4,@ProductID=2,@Quantity=4,@Amount=500,@TotalAmount=2000,@UserID=2
CREATE OR ALTER PROCEDURE [dbo].[pr_OrderDetail_UPDATE]
(@OrderDetailID Int,@OrderID Int , @ProductID Int , @Quantity Int ,@Amount Decimal(10,2) ,@TotalAmount Decimal(10,2) ,@UserID Int)
AS
BEGIN
	UPDATE [dbo].[ORDERDETAIL] set 
	[OrderID]=@OrderID,
	[ProductID]=@ProductID,
	[Quantity]=@Quantity,
	[Amount]=@Amount,
	[TotalAmount]=@TotalAmount,
	[UserID]=@UserID
	where [dbo].[ORDERDETAIL].[OrderDetailID]=@OrderDetailID
END

EXEC [dbo].[pr_OrderDetail_UPDATE] @OrderDetailID=4,@OrderID=4,@ProductID=2,@Quantity=4,@Amount=500,@TotalAmount=2000,@UserID=2
EXEC [DBO].[pr_OrderDetail_select_by_pk] @OrderDetailID=4

--Delete
--[dbo].[pr_OrderDetail_DELETE] @OrderDetailID=4
CREATE OR ALTER PROCEDURE [dbo].[pr_OrderDetail_DELETE]
(@OrderDetailID INT)
AS
BEGIN
	DELETE FROM [dbo].[ORDERDETAIL]
	WHERE [dbo].[ORDERDETAIL].[OrderDetailID]=@OrderDetailID
END

EXEC [dbo].[pr_OrderDetail_DELETE] @OrderDetailID=4
EXEC [dbo].[pr_OrderDetail_select_all]
EXEC [DBO].[pr_OrderDetail_select_by_pk] @OrderDetailID=4

