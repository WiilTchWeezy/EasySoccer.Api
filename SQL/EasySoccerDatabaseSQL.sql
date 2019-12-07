/****** Object:  Table [dbo].[Company]    Script Date: 06/12/2019 21:58:59 ******/
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
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CompanySchedule]    Script Date: 06/12/2019 21:58:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanySchedule](
	[Id] [bigint] NOT NULL,
	[Day] [int] NOT NULL,
	[StartHour] [bigint] NOT NULL,
	[FinalHour] [bigint] NOT NULL,
	[CompanyId] [bigint] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_CompanySchedule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CompanyUser]    Script Date: 06/12/2019 21:58:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyUser](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Email] [varchar](100) NULL,
	[Phone] [varchar](50) NULL,
	[Password] [varchar](200) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CompanyId] [bigint] NOT NULL,
 CONSTRAINT [PK_CompanyUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SoccerPitch]    Script Date: 06/12/2019 21:58:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SoccerPitch](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](300) NULL,
	[HasRoof] [bit] NOT NULL,
	[NumberOfPlayers] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CompanyId] [bigint] NOT NULL,
	[Active] [bit] NOT NULL,
	[ActiveDate] [datetime] NULL,
	[InactiveDate] [datetime] NULL,
 CONSTRAINT [PK_SoccerPitch] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SoccerPitchPictures]    Script Date: 06/12/2019 21:58:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SoccerPitchPictures](
	[Id] [uniqueidentifier] NOT NULL,
	[SoccerPitchId] [bigint] NOT NULL,
 CONSTRAINT [PK_SoccerPitchPictures] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SoccerPitchPlan]    Script Date: 06/12/2019 21:58:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SoccerPitchPlan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Value] [decimal](18, 2) NOT NULL,
	[CompanyId] [bigint] NOT NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_SoccerPitchPlan] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SoccerPitchReservation]    Script Date: 06/12/2019 21:58:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SoccerPitchReservation](
	[Id] [uniqueidentifier] NOT NULL,
	[SoccerPitchId] [bigint] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[SelectedDate] [datetime] NOT NULL,
	[SelectedHourStart] [time](0) NOT NULL,
	[SelectedHourEnd] [time](0) NOT NULL,
	[Status] [int] NOT NULL,
	[StatusChangedUserId] [bigint] NULL,
	[Note] [varchar](200) NULL,
	[OringinReservationId] [uniqueidentifier] NULL,
	[SoccerPitchSoccerPitchPlanId] [bigint] NOT NULL,
 CONSTRAINT [PK_SoccerPitchReservation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SoccerPitchSoccerPitchPlan]    Script Date: 06/12/2019 21:58:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SoccerPitchSoccerPitchPlan](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SoccerPitchId] [bigint] NOT NULL,
	[SoccerPitchPlanId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_SoccerPitchSoccerPitchPlan] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 06/12/2019 21:58:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Phone] [varchar](50) NULL,
	[SocialMediaId] [varchar](100) NULL,
	[Email] [varchar](100) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[Password] [varchar](200) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CompanySchedule]  WITH CHECK ADD  CONSTRAINT [FK_CompanySchedule_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[CompanySchedule] CHECK CONSTRAINT [FK_CompanySchedule_Company]
GO
ALTER TABLE [dbo].[CompanyUser]  WITH CHECK ADD  CONSTRAINT [FK_CompanyUser_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[CompanyUser] CHECK CONSTRAINT [FK_CompanyUser_Company]
GO
ALTER TABLE [dbo].[SoccerPitch]  WITH CHECK ADD  CONSTRAINT [FK_SoccerPitch_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([Id])
GO
ALTER TABLE [dbo].[SoccerPitch] CHECK CONSTRAINT [FK_SoccerPitch_Company]
GO
ALTER TABLE [dbo].[SoccerPitchPictures]  WITH CHECK ADD  CONSTRAINT [FK_SoccerPitchPictures_SoccerPitch] FOREIGN KEY([SoccerPitchId])
REFERENCES [dbo].[SoccerPitch] ([Id])
GO
ALTER TABLE [dbo].[SoccerPitchPictures] CHECK CONSTRAINT [FK_SoccerPitchPictures_SoccerPitch]
GO
ALTER TABLE [dbo].[SoccerPitchReservation]  WITH CHECK ADD  CONSTRAINT [FK_SoccerPitchReservation_CompanyUser] FOREIGN KEY([StatusChangedUserId])
REFERENCES [dbo].[CompanyUser] ([Id])
GO
ALTER TABLE [dbo].[SoccerPitchReservation] CHECK CONSTRAINT [FK_SoccerPitchReservation_CompanyUser]
GO
ALTER TABLE [dbo].[SoccerPitchReservation]  WITH CHECK ADD  CONSTRAINT [FK_SoccerPitchReservation_SoccerPitch] FOREIGN KEY([SoccerPitchId])
REFERENCES [dbo].[SoccerPitch] ([Id])
GO
ALTER TABLE [dbo].[SoccerPitchReservation] CHECK CONSTRAINT [FK_SoccerPitchReservation_SoccerPitch]
GO
ALTER TABLE [dbo].[SoccerPitchReservation]  WITH CHECK ADD  CONSTRAINT [FK_SoccerPitchReservation_SoccerPitchReservation1] FOREIGN KEY([OringinReservationId])
REFERENCES [dbo].[SoccerPitchReservation] ([Id])
GO
ALTER TABLE [dbo].[SoccerPitchReservation] CHECK CONSTRAINT [FK_SoccerPitchReservation_SoccerPitchReservation1]
GO
ALTER TABLE [dbo].[SoccerPitchReservation]  WITH CHECK ADD  CONSTRAINT [FK_SoccerPitchReservation_SoccerPitchSoccerPitchPlan] FOREIGN KEY([SoccerPitchSoccerPitchPlanId])
REFERENCES [dbo].[SoccerPitchSoccerPitchPlan] ([Id])
GO
ALTER TABLE [dbo].[SoccerPitchReservation] CHECK CONSTRAINT [FK_SoccerPitchReservation_SoccerPitchSoccerPitchPlan]
GO
ALTER TABLE [dbo].[SoccerPitchReservation]  WITH CHECK ADD  CONSTRAINT [FK_SoccerPitchReservation_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[SoccerPitchReservation] CHECK CONSTRAINT [FK_SoccerPitchReservation_User]
GO
ALTER TABLE [dbo].[SoccerPitchSoccerPitchPlan]  WITH CHECK ADD  CONSTRAINT [FK_SoccerPitchSoccerPitchPlan_SoccerPitch] FOREIGN KEY([SoccerPitchId])
REFERENCES [dbo].[SoccerPitch] ([Id])
GO
ALTER TABLE [dbo].[SoccerPitchSoccerPitchPlan] CHECK CONSTRAINT [FK_SoccerPitchSoccerPitchPlan_SoccerPitch]
GO
ALTER TABLE [dbo].[SoccerPitchSoccerPitchPlan]  WITH CHECK ADD  CONSTRAINT [FK_SoccerPitchSoccerPitchPlan_SoccerPitchPlan] FOREIGN KEY([SoccerPitchPlanId])
REFERENCES [dbo].[SoccerPitchPlan] ([Id])
GO
ALTER TABLE [dbo].[SoccerPitchSoccerPitchPlan] CHECK CONSTRAINT [FK_SoccerPitchSoccerPitchPlan_SoccerPitchPlan]
GO
