--Stored Procedure of CustomerTable

--Select all--
--[dbo].[PR_Customer_SelectAll]
CREATE OR ALTER   PROCEDURE [dbo].[PR_Customer_SelectAll]
AS
BEGIN
    SELECT [dbo].[Customer].[CustomerID],
	[dbo].[Customer].[CustomerName],
	[dbo].[Customer].[HomeAddress],
	[dbo].[Customer].[Email],
	[dbo].[Customer].[MobileNo],
	[dbo].[Customer].[GSTNO],
	[dbo].[Customer].[CityName],
	[dbo].[Customer].[PinCode],
	[dbo].[Customer].[NetAmount],
	[dbo].[User].[UserName]
	FROM [dbo].[Customer]
	INNER JOIN [dbo].[User]
	ON [dbo].[Customer].[UserID]=[dbo].[User].[UserID]
	ORDER BY [dbo].[Customer].[CustomerName]
END

--Select by primary key--
--[dbo].[PR_Customer_SelectByPK] @CustomerID=1
CREATE OR ALTER PROCEDURE [dbo].[PR_Customer_SelectByPK]
(@CustomerID int)
AS
BEGIN
   SELECT [dbo].[Customer].[CustomerID],
	[dbo].[Customer].[CustomerName],
	[dbo].[Customer].[HomeAddress],
	[dbo].[Customer].[Email],
	[dbo].[Customer].[MobileNo],
	[dbo].[Customer].[GSTNO],
	[dbo].[Customer].[CityName],
	[dbo].[Customer].[PinCode],
	[dbo].[Customer].[NetAmount],
	[dbo].[Customer].[UserID]
	FROM [dbo].[Customer]
	where [dbo].[Customer].[CustomerID]=@CustomerID
END

---Insert--
--[dbo].[pr_Customer_INSERT] @CustomerName='Bhagyesh',@HomeAddress='Rajkot',@Email='abc@gmail.com',@MobileNo='1234567890',@GSTNO='0111',@CityName='Rajkot',@PinCode='360007',@NetAmount=1000,@UserID=2
CREATE OR ALTER PROCEDURE [dbo].[pr_Customer_INSERT]
(@CustomerName Varchar(100) ,@HomeAddress Varchar(100) ,@Email Varchar(100) ,@MobileNo Varchar(15) ,@GSTNO Varchar(15) ,@CityName Varchar(100) ,@PinCode Varchar(15) ,@NetAmount Decimal(10,2) ,@UserID Int )
AS
BEGIN
	INSERT INTO [dbo].[Customer]([CustomerName],[HomeAddress],[Email],[MobileNo],[GSTNO],[CityName],[PinCode],[NetAmount],[UserID])
	VALUES(@CustomerName,@HomeAddress,@Email,@MobileNo,@GSTNO,@CityName,@PinCode,@NetAmount,@UserID)
END

EXEC [dbo].[pr_Customer_INSERT] @CustomerName='Bhagyesh',@HomeAddress='Rajkot',@Email='abc@gmail.com',@MobileNo='1234567890',@GSTNO='0111',@CityName='Rajkot',@PinCode='360007',@NetAmount=1000,@UserID=2

EXEC [dbo].[pr_Customer_select_all]
EXEC [DBO].[PR_Customer_SelectByPK] @CustomerID=1

--Update
--[dbo].[pr_Customer_UPDATE] @CustomerID=1,@CustomerName='Bhagyesh',@HomeAddress='Rajkot',@Email='abc@gmail.com',@MobileNo='1234567890',@GSTNO='0111',@CityName='Rajkot',@PinCode='360007',@NetAmount=5000,@UserID=2
CREATE OR ALTER PROCEDURE [dbo].[pr_Customer_UPDATE]
(@CustomerID Int,@CustomerName Varchar(100) ,@HomeAddress Varchar(100) ,@Email Varchar(100) ,@MobileNo Varchar(15) ,@GSTNO Varchar(15) ,@CityName Varchar(100) ,@PinCode Varchar(15) ,@NetAmount Decimal(10,2) ,@UserID Int )
AS
BEGIN
	UPDATE [dbo].[Customer] set 
	[CustomerName]=@CustomerName,
	[HomeAddress]=@HomeAddress,
	[Email]=@Email,
	[MobileNo]=@MobileNo,
	[GSTNO]=@GSTNO,
	[CityName]=@CityName,
	[PinCode]=@PinCode,
	[NetAmount]=@NetAmount,
	[UserID]=@UserID
	where [dbo].[Customer].[CustomerID]=@CustomerID
END

EXEC [dbo].[pr_Customer_UPDATE] @CustomerID=1,@CustomerName='Bhagyesh',@HomeAddress='Rajkot',@Email='abc@gmail.com',@MobileNo='1234567890',@GSTNO='0111',@CityName='Rajkot',@PinCode='360007',@NetAmount=5000,@UserID=2
EXEC [DBO].[PR_Customer_SelectByPK] @CustomerID=1

--Delete
--[dbo].[pr_Customer_DELETE] @CustomerID=3
CREATE OR ALTER PROCEDURE [dbo].[pr_Customer_DELETE]
(@CustomerID INT)
AS
BEGIN
	DELETE FROM [dbo].[Customer]
	WHERE [dbo].[Customer].[CustomerID]=@CustomerID
END

EXEC [dbo].[pr_Customer_DELETE] @CustomerID=3
EXEC [dbo].[pr_Customer_select_all]
EXEC [DBO].[PR_Customer_SelectByPK] @CustomerID=3


CREATE OR ALTER PROCEDURE [dbo].[PR_Customer_DropDown]
AS
BEGIN
    SELECT
		[dbo].[Customer].[CustomerID],
        [dbo].[Customer].[CustomerName]
    FROM
        [dbo].[Customer]
END

