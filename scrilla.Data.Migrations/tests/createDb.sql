USE [master]
GO

/****** Object:  Database [scrilla.js.tests] ******/
CREATE DATABASE [scrilla.js.tests] ON  PRIMARY 
( NAME = N'scrilla.js.tests', FILENAME = N'D:\DATA\scrilla.js.tests.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'scrilla.js.tests_log', FILENAME = N'D:\DATA\scrilla.js.tests.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [scrilla.js.tests] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [scrilla.js.tests].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [scrilla.js.tests] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [scrilla.js.tests] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [scrilla.js.tests] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [scrilla.js.tests] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [scrilla.js.tests] SET ARITHABORT OFF 
GO

ALTER DATABASE [scrilla.js.tests] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [scrilla.js.tests] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [scrilla.js.tests] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [scrilla.js.tests] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [scrilla.js.tests] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [scrilla.js.tests] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [scrilla.js.tests] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [scrilla.js.tests] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [scrilla.js.tests] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [scrilla.js.tests] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [scrilla.js.tests] SET  DISABLE_BROKER 
GO

ALTER DATABASE [scrilla.js.tests] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [scrilla.js.tests] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [scrilla.js.tests] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [scrilla.js.tests] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [scrilla.js.tests] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [scrilla.js.tests] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [scrilla.js.tests] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [scrilla.js.tests] SET  READ_WRITE 
GO

ALTER DATABASE [scrilla.js.tests] SET RECOVERY FULL 
GO

ALTER DATABASE [scrilla.js.tests] SET  MULTI_USER 
GO

ALTER DATABASE [scrilla.js.tests] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [scrilla.js.tests] SET DB_CHAINING OFF 
GO

