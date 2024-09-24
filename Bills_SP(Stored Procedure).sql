--Stored Procedure of Bills Table

--Select all--
--[dbo].[pr_Bills_select_all]
CREATE OR ALTER   PROCEDURE [dbo].[PR_Bills_SelectAll]
AS
BEGIN
    SELECT [dbo].[BILLS].[BillID],
	[dbo].[BILLS].[BillNumber],
	[dbo].[BILLS].[BillDate],
	[dbo].[ORDER].[OrderNumber],
	[dbo].[BILLS].[TotalAmount],
	[dbo].[BILLS].[Discount],
	[dbo].[BILLS].[NetAmount],
	[dbo].[User].[UserName]
	FROM [dbo].[BILLS]
	INNER JOIN [dbo].[User]
	ON [dbo].[BILLS].[UserID]=[dbo].[User].[UserID]
	INNER JOIN [DBO].[ORDER]
	ON [DBO].[ORDER].[OrderID]=[DBO].[BILLS].[OrderID]
	ORDER BY [dbo].[BILLS].[BillDate]
END

--Select by primary key--
--[dbo].[pr_Bills_select_by_pk] @BillID=1
CREATE OR ALTER PROCEDURE [dbo].[PR_Bills_SelectByPK]
(@BillID int)
AS
BEGIN
    SELECT [dbo].[BILLS].[BillID],
	[dbo].[BILLS].[BillNumber],
	[dbo].[BILLS].[BillDate],
	[dbo].[BILLS].[OrderID],
	[dbo].[BILLS].[TotalAmount],
	[dbo].[BILLS].[Discount],
	[dbo].[BILLS].[NetAmount],
	[dbo].[BILLS].[UserID]
	FROM [dbo].[BILLS]
	INNER JOIN [dbo].[User]
	ON [dbo].[BILLS].[UserID]=[dbo].[User].[UserID]
	where [dbo].[BILLS].[BillID]=@BillID
END

---Insert--
--[dbo].[pr_Bill_INSERT] @BillNumber='001',@BillDate='2024-07-30',@OrderID=3,@TotalAmount=1000,@Discount=100,@NetAmount=900,@UserID=2
CREATE OR ALTER PROCEDURE [dbo].[pr_Bill_INSERT]
(@BillNumber Varchar(100) ,@BillDate DateTime ,@OrderID Int ,@TotalAmount Decimal(10,2) ,@Discount Decimal(10,2),@NetAmount Decimal(10,2),@UserID Int)
AS
BEGIN
	INSERT INTO [dbo].[BILLS]([BillNumber],[BillDate],[OrderID],[TotalAmount],[Discount],[NetAmount],[UserID])
	VALUES(@BillNumber,@BillDate,@OrderID,@TotalAmount,@Discount,@NetAmount,@UserID)
END

EXEC [dbo].[pr_Bill_INSERT] @BillNumber='001',@BillDate='2024-07-30',@OrderID=4,@TotalAmount=1000,@Discount=100,@NetAmount=900,@UserID=2

EXEC [dbo].[pr_Bills_select_all]
EXEC [DBO].[pr_Bills_select_by_pk] @BillID=1

--Update
--[dbo].[pr_Bill_UPDATE] @BillID=1,@BillNumber='001',@BillDate='2024-07-30',@OrderID=4,@TotalAmount=5000,@Discount=1000,@NetAmount=4000,@UserID=2
CREATE OR ALTER PROCEDURE [dbo].[pr_Bill_UPDATE]
(@BillID Int ,@BillNumber Varchar(100) ,@BillDate DateTime ,@OrderID Int ,@TotalAmount Decimal(10,2) ,@Discount Decimal(10,2),@NetAmount Decimal(10,2),@UserID Int)
AS
BEGIN
	UPDATE [dbo].[BILLS] set 
	[BillNumber]=@BillNumber,
	[BillDate]=@BillDate,
	[OrderID]=@OrderID,
	[TotalAmount]=@TotalAmount,
	[Discount]=@Discount,
	[NetAmount]=@NetAmount,
	[UserID]=@UserID
	where [dbo].[BILLS].[BillID]=@BillID
END

EXEC [dbo].[pr_Bill_UPDATE] @BillID=1,@BillNumber='001',@BillDate='2024-07-30',@OrderID=4,@TotalAmount=5000,@Discount=1000,@NetAmount=4000,@UserID=2
EXEC [DBO].[pr_Bills_select_by_pk] @BillID=1

--Delete
--[dbo].[pr_Bill_DELETE] @BillID=1
CREATE OR ALTER PROCEDURE [dbo].[pr_Bill_DELETE]
(@BillID INT)
AS
BEGIN
	DELETE FROM [dbo].[BILLS] 
	WHERE [dbo].[BILLS].[BillID]=@BillID
END

EXEC [dbo].[pr_Bill_DELETE] @BillID=1
EXEC [dbo].[pr_Bills_select_all]
EXEC [DBO].[pr_Bills_select_by_pk] @BillID=1

