--Stored Procedure of OrderTable

--Select all--
--[dbo].[pr_Order_select_all]

CREATE OR ALTER   PROCEDURE [dbo].[PR_Order_SelectAll]
AS
BEGIN
    SELECT 
	[dbo].[Order].[OrderID],
	[dbo].[Order].[OrderDate],
	[dbo].[Order].[Ordernumber],
	[dbo].[Customer].[CustomerName],
	[dbo].[Order].[PaymentMode],
	[dbo].[Order].[TotalAmount],
	[dbo].[Order].[ShippingAddress],
	[dbo].[User].[UserName]
	FROM [dbo].[Order]
	INNER JOIN [DBO].[Customer]
	ON [DBO].[ORDER].[CustomerID]=[DBO].[Customer].[CustomerID]
	INNER JOIN [dbo].[User]
	ON [dbo].[Order].[UserID]=[dbo].[User].[UserID]
	ORDER BY [dbo].[Order].[OrderDate]
END

--Select by primary key--
--[dbo].[PR_Order_SelectByPK] @OrderID=1
CREATE OR ALTER PROCEDURE [dbo].[PR_Order_SelectByPK] 
(@OrderID int)
AS
BEGIN
    SELECT [dbo].[Order].[OrderID],
	[dbo].[Order].[OrderDate],
	[DBO].[ORDER].[OrderNumber],
	[dbo].[Order].[CustomerID],
	[dbo].[Order].[PaymentMode],
	[dbo].[Order].[TotalAmount],
	[dbo].[Order].[ShippingAddress],
	[dbo].[Order].[UserID]
	FROM [dbo].[Order] 
	where [dbo].[Order].[OrderID]=@OrderID
END
--[dbo].[PR_Order_DropDown]
CREATE OR ALTER   PROCEDURE [dbo].[PR_Order_DropDown]
AS
BEGIN
	SELECT [DBO].[ORDER].[OrderID],
	[DBO].[ORDER].[OrderNumber]
	FROM [DBO].[ORDER]
END
---Insert--
--[dbo].[pr_Order_INSERT] @OrderDate='2024-07-29', @CustomerID=1,@PaymentMode='Online',@TotalAmount=1000,@ShippingAddress='Rajkot',@UserID=2

CREATE OR ALTER PROCEDURE [dbo].[pr_Order_INSERT]
(@OrderDate DateTime ,@OrderNumber varchar(100),@CustomerID Int,@PaymentMode Varchar(100),@TotalAmount Decimal(10,2),@ShippingAddress Varchar(100) ,@UserID Int  )
AS
BEGIN
	INSERT INTO [dbo].[Order] ([OrderDate],[OrderNumber],[CustomerID],[PaymentMode],[TotalAmount],[ShippingAddress],[UserID])
	VALUES(@OrderDate,@OrderNumber,@CustomerID,@PaymentMode,@TotalAmount,@ShippingAddress,@UserID)
END

EXEC [dbo].[pr_Order_INSERT] @OrderDate='2024-07-29', @CustomerID=1,@PaymentMode='Online',@TotalAmount=1000,@ShippingAddress='Rajkot',@UserID=2

EXEC [dbo].[pr_Order_select_all]
EXEC [DBO].[PR_Order_SelectByPK] @OrderID=3

--Update--
--[dbo].[pr_Order_UPDATE] @OrderID=3,@OrderDate='2024-07-29', @CustomerID=1,@PaymentMode='Cash',@TotalAmount=3500,@ShippingAddress='Rajkot',@UserID=2

CREATE OR ALTER PROCEDURE [dbo].[pr_Order_UPDATE]
(@OrderID Int ,@OrderDate DateTime ,@OrderNumber varchar(100),@CustomerID Int,@PaymentMode Varchar(100),@TotalAmount Decimal(10,2),@ShippingAddress Varchar(100) ,@UserID Int  )
AS
BEGIN
	UPDATE [dbo].[Order] set 
	[OrderDate]=@OrderDate,
	[OrderNumber]=@OrderNumber,
	[CustomerID]=@CustomerID,
	[PaymentMode]=@PaymentMode,
	[TotalAmount]=@TotalAmount,
	[ShippingAddress]=@ShippingAddress,
	[UserID]=@UserID
	where [dbo].[Order].[OrderID]=@OrderID
END

EXEC [dbo].[pr_Order_UPDATE] @OrderID=3,@OrderDate='2024-07-29', @CustomerID=1,@PaymentMode='Cash',@TotalAmount=3500,@ShippingAddress='Rajkot',@UserID=2
EXEC [DBO].[PR_Order_SelectByPK] @OrderID=3
EXEC [DBO].[pr_Order_select_all]

--Delete--
CREATE OR ALTER PROCEDURE [dbo].[pr_Order_DELETE]
(@OrderID INT)
AS
BEGIN
	DELETE FROM [dbo].[Order] 
	WHERE [dbo].[Order].[OrderID]=@OrderID
END

EXEC [dbo].[pr_Order_DELETE] @OrderID=3
EXEC [dbo].[pr_Order_select_all]
EXEC [DBO].[PR_Order_SelectByPK] @OrderID=3

