--Stored Procedure of [dbo].[User] Table

--Select all
--[PR_User_SelectAll]

CREATE OR ALTER   PROCEDURE [dbo].[PR_User_SelectAll]
AS
BEGIN
    SELECT [dbo].[User].[UserID],
	[dbo].[User].[UserName],
	[dbo].[User].[Email],
	[dbo].[User].[Password],
	[dbo].[User].[MobileNo],
	[dbo].[User].[Address],
	[dbo].[User].[IsActive]
	FROM [dbo].[User]
	ORDER BY [dbo].[User].[UserName]
END

--Select by primary key
--[dbo].[pr_User_SelectByPK] @UserID=1

CREATE OR ALTER PROCEDURE [dbo].[pr_User_SelectByPK]
(@UserID int)
AS
BEGIN
    SELECT [dbo].[User].[UserID],
	[dbo].[User].[UserName],
	[dbo].[User].[Email],
	[dbo].[User].[Password],
	[dbo].[User].[MobileNo],
	[dbo].[User].[Address],
	[dbo].[User].[IsActive]
	from [dbo].[User]
	where [dbo].[User].[UserID]=@UserID
END


--Insert
--[dbo].[pr_User_INSERT] 'BHAGYESH VYAS','bhagyesh@gmail.com','fdgsh@','1234567890','Rajkot',1
CREATE OR ALTER PROCEDURE [dbo].[pr_User_INSERT]
(@UserName Varchar(100), @Email Varchar(100), @Password Varchar(100), @MobileNo Varchar(15), @Address Varchar(100), @IsActive Bit)
AS
BEGIN
	INSERT INTO [dbo].[User]([UserName],[Email],[Password],[MobileNo],[Address],[IsActive])
	VALUES(@UserName, @Email, @Password, @MobileNo, @Address, @IsActive)
END

EXEC [dbo].[pr_User_INSERT] 'BHAGYESH VYAS','bhagyesh@gmail.com','fdgsh@','1234567890','Rajkot',1
EXEC [DBO].[pr_User_select_all]
EXEC [DBO].[pr_User_SelectByPK] @UserID=2

--Update
--[dbo].[pr_User_UPDATE] @UserID=1,@UserName='BHAGYESH Y VYAS',@Email='bhagyesh@gmail.com',@Password='fdgsh@',@MobileNo='1234567890',@Address='Rajkot',@IsActive=1
CREATE OR ALTER PROCEDURE [dbo].[pr_User_UPDATE]
(@UserID INT,@UserName Varchar(100), @Email Varchar(100), @Password Varchar(100), @MobileNo Varchar(15), @Address Varchar(100), @IsActive Bit)
AS
BEGIN
	UPDATE [dbo].[User] set 
	[UserName]=@UserName,
	[Email]=@Email,
	[Password]=@Password,
	[MobileNo]=@MobileNo,
	[Address]=@Address,
	[IsActive]=@IsActive
	where [dbo].[User].[UserID]=@UserID
END

EXEC [dbo].[pr_User_UPDATE] @UserID=1,@UserName='BHAGYESH Y VYAS',@Email='bhagyesh@gmail.com',@Password='fdgsh@',@MobileNo='1234567890',@Address='Rajkot',@IsActive=1

--Delete
--[dbo].[pr_User_DELETE] @UserID=1
CREATE OR ALTER PROCEDURE [dbo].[pr_User_DELETE]
(@UserID INT)
AS
BEGIN
	DELETE FROM [dbo].[User] 
	WHERE [dbo].[User].[UserID]=@UserID
END

EXEC [dbo].[pr_User_DELETE] @UserID=1
EXEC [dbo].[pr_User_select_all]

CREATE OR ALTER PROCEDURE [dbo].[PR_User_DropDown]
AS
BEGIN
    SELECT
		[dbo].[User].[UserID],
        [dbo].[User].[UserName]
    FROM
        [dbo].[User]
END

--PR_User_Login

CREATE OR ALTER PROCEDURE [dbo].[PR_User_Login]
    @UserName NVARCHAR(50),
    @Password NVARCHAR(50)
AS
BEGIN
    SELECT 
        [dbo].[User].[UserID], 
        [dbo].[User].[UserName], 
        [dbo].[User].[MobileNo], 
        [dbo].[User].[Email], 
        [dbo].[User].[Password],
        [dbo].[User].[Address]
    FROM 
        [dbo].[User] 
    WHERE 
        [dbo].[User].[UserName] = @UserName 
        AND [dbo].[User].[Password] = @Password;
END

--PR_User_Register

CREATE OR ALTER     PROCEDURE [dbo].[PR_User_Register]
(@UserName Varchar(100), @Email Varchar(100), @Password Varchar(100), @MobileNo Varchar(15), @Address Varchar(100), @IsActive Bit)
AS
BEGIN
	INSERT INTO [dbo].[User]([UserName],[Email],[Password],[MobileNo],[Address],[IsActive])
	VALUES(@UserName, @Email, @Password, @MobileNo, @Address,@IsActive)
END