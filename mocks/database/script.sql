IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'CdsApiDb')
BEGIN
    CREATE DATABASE CdsApiDb;
END
GO

USE [CdsApiDb];
GO

CREATE TABLE Courses (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Code NVARCHAR(50) NOT NULL,
    EffectiveDate DATETIME2 NOT NULL,
    ExpiryDate DATETIME2 NOT NULL,
    IsActive BIT NOT NULL,
    Description NVARCHAR(MAX) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL
);