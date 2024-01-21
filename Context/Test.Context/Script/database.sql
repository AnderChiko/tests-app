GO
IF OBJECT_ID(N'[dbo].[Task]', N'U') IS NOT NULL 
DROP TABLE [dbo].[Task];

GO
IF OBJECT_ID(N'[dbo].[User]', N'U') IS NOT NULL 
DROP TABLE [dbo].[User];

GO
CREATE TABLE [dbo].[User](
	[Id] [INT] IDENTITY(1,1) NOT NULL  PRIMARY KEY,
	[Username] [varchar](60) NULL,
	[Email] [varchar](70) NULL,
	[Password] [varchar](20) NULL,
	[Active] [bit] NOT NULL DEFAULT 0,
	[CreatedTime] DateTime default getdate()
	);
GO

CREATE TABLE [dbo].[Task](
	[Id] [INT] IDENTITY(1,1) NOT NULL  PRIMARY KEY,
	[Title] [varchar](60) NULL,
	[Description] [varchar](70) NULL,
	[Assignee] [INT] NULL FOREIGN KEY REFERENCES [User](Id),
	[DueDate] Date NULL
	);
GO

GO

INSERT INTO [dbo].[User] ([Username] ,[Email] ,[Password] ,[Active] ,[CreatedTime])
     VALUES ('string','string','string',	1 ,getdate())
GO