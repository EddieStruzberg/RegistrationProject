USE [master]
GO
CREATE DATABASE [AppDB]
GO
USE [AppDB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Email] [varchar](100) NULL,
	[birthday] [varchar](50) NULL,
	[FullName] [varchar](100) NULL,
	[PhoneNumber] [varchar](50) NULL,
	[IsMale] [bit] NULL,
	[UserId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


