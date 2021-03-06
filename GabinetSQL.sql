USE [master]
GO
/****** Object:  Database [Gabinet]    Script Date: 2015-09-15 17:09:36 ******/
CREATE DATABASE [Gabinet]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Gabinet', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Gabinet.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Gabinet_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\Gabinet_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Gabinet] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Gabinet].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Gabinet] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Gabinet] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Gabinet] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Gabinet] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Gabinet] SET ARITHABORT OFF 
GO
ALTER DATABASE [Gabinet] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Gabinet] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [Gabinet] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Gabinet] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Gabinet] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Gabinet] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Gabinet] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Gabinet] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Gabinet] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Gabinet] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Gabinet] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Gabinet] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Gabinet] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Gabinet] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Gabinet] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Gabinet] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Gabinet] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Gabinet] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Gabinet] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Gabinet] SET  MULTI_USER 
GO
ALTER DATABASE [Gabinet] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Gabinet] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Gabinet] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Gabinet] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [Gabinet]
GO
/****** Object:  Table [dbo].[godziny]    Script Date: 2015-09-15 17:09:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[godziny](
	[godziny_id] [int] IDENTITY(1,1) NOT NULL,
	[uzytkownik_id] [int] NOT NULL,
	[pon_od] [time](7) NULL,
	[pon_do] [time](7) NULL,
	[wt_od] [time](7) NULL,
	[wt_do] [time](7) NULL,
	[sr_od] [time](7) NULL,
	[sr_do] [time](7) NULL,
	[czw_od] [time](7) NULL,
	[czw_do] [time](7) NULL,
	[pt_od] [time](7) NULL,
	[pt_do] [time](7) NULL,
	[sb_od] [time](7) NULL,
	[sb_do] [time](7) NULL,
	[nd_od] [time](7) NULL,
	[nd_do] [time](7) NULL,
 CONSTRAINT [PK_godziny] PRIMARY KEY CLUSTERED 
(
	[godziny_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[klienci]    Script Date: 2015-09-15 17:09:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[klienci](
	[klienci_id] [int] IDENTITY(1,1) NOT NULL,
	[imie] [nvarchar](50) NULL,
	[nazwisko] [nvarchar](120) NULL,
	[email] [nvarchar](100) NULL,
	[telefon] [nvarchar](50) NULL,
	[ulica] [nvarchar](120) NULL,
	[numerDomu] [nvarchar](15) NULL,
	[miejscowosc] [nvarchar](120) NULL,
	[kodPocztowy] [nvarchar](50) NULL,
	[modyfikacja] [datetime] NULL,
 CONSTRAINT [PK_klienci] PRIMARY KEY CLUSTERED 
(
	[klienci_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[status]    Script Date: 2015-09-15 17:09:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[status](
	[status_id] [int] IDENTITY(1,1) NOT NULL,
	[nazwa] [nvarchar](50) NULL,
 CONSTRAINT [PK_status] PRIMARY KEY CLUSTERED 
(
	[status_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[uslugi]    Script Date: 2015-09-15 17:09:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[uslugi](
	[usluga_id] [int] IDENTITY(1,1) NOT NULL,
	[nazwa] [nvarchar](50) NULL,
	[cena] [money] NULL,
	[czas] [time](7) NULL,
	[opis] [text] NULL,
 CONSTRAINT [PK_uslugi] PRIMARY KEY CLUSTERED 
(
	[usluga_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[uzytkownik]    Script Date: 2015-09-15 17:09:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[uzytkownik](
	[uzytkownik_id] [int] IDENTITY(1,1) NOT NULL,
	[login] [nvarchar](50) NULL,
	[imie] [nvarchar](50) NULL,
	[nazwisko] [nvarchar](120) NULL,
	[haslo] [nvarchar](120) NULL,
	[pracownik] [bit] NULL,
 CONSTRAINT [PK_uzytkownik] PRIMARY KEY CLUSTERED 
(
	[uzytkownik_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[uzytkownik_usluga]    Script Date: 2015-09-15 17:09:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[uzytkownik_usluga](
	[uzytkownik_usluga_id] [int] IDENTITY(1,1) NOT NULL,
	[uzytkownik_id] [int] NOT NULL,
	[usluga_id] [int] NOT NULL,
	[modyfikacja] [timestamp] NULL,
 CONSTRAINT [PK_uzytkownik_usluga] PRIMARY KEY CLUSTERED 
(
	[uzytkownik_usluga_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[wizyty]    Script Date: 2015-09-15 17:09:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[wizyty](
	[wizyty_id] [int] IDENTITY(1,1) NOT NULL,
	[klienci_id] [int] NULL,
	[uslugi_id] [int] NULL,
	[uzytkownik_id] [int] NULL,
	[rezerwacja_od] [datetime] NULL,
	[rezerwacja_do] [datetime] NULL,
	[status_id] [int] NULL,
	[modyfikacja] [datetime] NULL,
 CONSTRAINT [PK_wizyty] PRIMARY KEY CLUSTERED 
(
	[wizyty_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[godziny]  WITH CHECK ADD  CONSTRAINT [FK_godziny_uzytkownik] FOREIGN KEY([uzytkownik_id])
REFERENCES [dbo].[uzytkownik] ([uzytkownik_id])
GO
ALTER TABLE [dbo].[godziny] CHECK CONSTRAINT [FK_godziny_uzytkownik]
GO
ALTER TABLE [dbo].[uzytkownik_usluga]  WITH CHECK ADD  CONSTRAINT [FK_uzytkownik_usluga_uslugi] FOREIGN KEY([usluga_id])
REFERENCES [dbo].[uslugi] ([usluga_id])
GO
ALTER TABLE [dbo].[uzytkownik_usluga] CHECK CONSTRAINT [FK_uzytkownik_usluga_uslugi]
GO
ALTER TABLE [dbo].[uzytkownik_usluga]  WITH CHECK ADD  CONSTRAINT [FK_uzytkownik_usluga_uzytkownik] FOREIGN KEY([uzytkownik_id])
REFERENCES [dbo].[uzytkownik] ([uzytkownik_id])
GO
ALTER TABLE [dbo].[uzytkownik_usluga] CHECK CONSTRAINT [FK_uzytkownik_usluga_uzytkownik]
GO
ALTER TABLE [dbo].[wizyty]  WITH CHECK ADD  CONSTRAINT [FK_wizyty_klienci] FOREIGN KEY([klienci_id])
REFERENCES [dbo].[klienci] ([klienci_id])
GO
ALTER TABLE [dbo].[wizyty] CHECK CONSTRAINT [FK_wizyty_klienci]
GO
ALTER TABLE [dbo].[wizyty]  WITH CHECK ADD  CONSTRAINT [FK_wizyty_status] FOREIGN KEY([status_id])
REFERENCES [dbo].[status] ([status_id])
GO
ALTER TABLE [dbo].[wizyty] CHECK CONSTRAINT [FK_wizyty_status]
GO
ALTER TABLE [dbo].[wizyty]  WITH CHECK ADD  CONSTRAINT [FK_wizyty_uslugi] FOREIGN KEY([uslugi_id])
REFERENCES [dbo].[uslugi] ([usluga_id])
GO
ALTER TABLE [dbo].[wizyty] CHECK CONSTRAINT [FK_wizyty_uslugi]
GO
ALTER TABLE [dbo].[wizyty]  WITH CHECK ADD  CONSTRAINT [FK_wizyty_uzytkownik] FOREIGN KEY([uzytkownik_id])
REFERENCES [dbo].[uzytkownik] ([uzytkownik_id])
GO
ALTER TABLE [dbo].[wizyty] CHECK CONSTRAINT [FK_wizyty_uzytkownik]
GO
USE [master]
GO
ALTER DATABASE [Gabinet] SET  READ_WRITE 
GO
