USE [EasySoccer]
GO

/****** Object:  Table [dbo].[Company]    Script Date: 11/12/2019 00:57:45 ******/
DROP TABLE [dbo].[Company]
GO

/****** Object:  Table [dbo].[Company]    Script Date: 11/12/2019 00:57:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Company](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Description] [varchar](200) NULL,
	[CNPJ] [varchar](15) NULL,
	[Logo] [varchar](100) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[WorkOnHoliDays] [bit] NOT NULL,
	[Latitude] [decimal](12, 9) NULL,
	[Longitude] [decimal](12, 9) NULL,
	[City] [varchar](50) NULL,
	[CompleteAddress] [varchar](200) NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


