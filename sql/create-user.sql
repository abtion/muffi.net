DECLARE @UserName NVARCHAR(200)
SET @UserName = 'care1@abtion.com'

DECLARE @Email NVARCHAR(200)
SET @Email = 'care1@abtion.com'

DECLARE @FullName NVARCHAR(200)
SET @FullName = 'Abtion Test User'

INSERT INTO [dbo].[AspNetUsers]
    ([Id], [UserName],[NormalizedUserName],[Email],[NormalizedEmail],[EmailConfirmed],[PasswordHash],[SecurityStamp],[ConcurrencyStamp],[PhoneNumber],[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEnd],[LockoutEnabled],[AccessFailedCount], FullName)
VALUES
    (NewID(), LOWER(@UserName), UPPER(@UserName), LOWER(@Email), UPPER(@Email), 1, 'AQAAAAEAACcQAAAAEH3IoJ1j7WL+7G9Jfqo4Jq8ureASrSvuvI8+vMVrfkz3NubbekK4YUEfKMDGLUMORg==', 'VB6G74SWYX2OS5NO7YN6SCUDFPCP4C6R', 'b0ede9c1-a7c8-4fd6-9139-d0fd52903972', NULL, 0, 0, NULL, 1, 0, @FullName)
