USE [LeagueMaandag]
GO
/****** Object:  Table [dbo].[Speler]    Script Date: 6/05/2024 11:21:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Speler](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Naam] [nvarchar](250) NOT NULL,
	[Gewicht] [int] NULL,
	[Lengte] [int] NULL,
	[Rugnummer] [int] NULL,
	[Team_id] [int] NULL,
 CONSTRAINT [PK_Speler] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Team]    Script Date: 6/05/2024 11:21:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Team](
	[Stamnummer] [int] NOT NULL,
	[Naam] [nvarchar](250) NOT NULL,
	[Bijnaam] [nvarchar](250) NULL,
 CONSTRAINT [PK_Team] PRIMARY KEY CLUSTERED 
(
	[Stamnummer] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transfer]    Script Date: 6/05/2024 11:21:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transfer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Prijs] [int] NOT NULL,
	[speler_id] [int] NOT NULL,
	[oudteam_id] [int] NULL,
	[nieuwteam_id] [int] NULL,
 CONSTRAINT [PK_Transfer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Speler]  WITH CHECK ADD  CONSTRAINT [FK_Speler_Team] FOREIGN KEY([Team_id])
REFERENCES [dbo].[Team] ([Stamnummer])
GO
ALTER TABLE [dbo].[Speler] CHECK CONSTRAINT [FK_Speler_Team]
GO
ALTER TABLE [dbo].[Transfer]  WITH CHECK ADD  CONSTRAINT [FK_Transfer_Speler] FOREIGN KEY([Id])
REFERENCES [dbo].[Speler] ([Id])
GO
ALTER TABLE [dbo].[Transfer] CHECK CONSTRAINT [FK_Transfer_Speler]
GO
ALTER TABLE [dbo].[Transfer]  WITH CHECK ADD  CONSTRAINT [FK_Transfer_Team] FOREIGN KEY([Id])
REFERENCES [dbo].[Team] ([Stamnummer])
GO
ALTER TABLE [dbo].[Transfer] CHECK CONSTRAINT [FK_Transfer_Team]
GO
ALTER TABLE [dbo].[Transfer]  WITH CHECK ADD  CONSTRAINT [FK_Transfer_Team1] FOREIGN KEY([nieuwteam_id])
REFERENCES [dbo].[Team] ([Stamnummer])
GO
ALTER TABLE [dbo].[Transfer] CHECK CONSTRAINT [FK_Transfer_Team1]
GO
