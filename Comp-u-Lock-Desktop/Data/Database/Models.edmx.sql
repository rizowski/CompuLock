
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 02/11/2013 17:53:05
-- Generated from EDMX file: C:\Users\Rizowski\Documents\GitHub\CompuLock\Comp-u-Lock-Desktop\Data\Database\Models.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [settings];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserComputer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Computers] DROP CONSTRAINT [FK_UserComputer];
GO
IF OBJECT_ID(N'[dbo].[FK_ComputerAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Accounts] DROP CONSTRAINT [FK_ComputerAccount];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountProcess]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Processes] DROP CONSTRAINT [FK_AccountProcess];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountHistory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Histories] DROP CONSTRAINT [FK_AccountHistory];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountProgram]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Programs] DROP CONSTRAINT [FK_AccountProgram];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Accounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Accounts];
GO
IF OBJECT_ID(N'[dbo].[Computers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Computers];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Programs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Programs];
GO
IF OBJECT_ID(N'[dbo].[Histories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Histories];
GO
IF OBJECT_ID(N'[dbo].[Processes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Processes];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Accounts'
CREATE TABLE [dbo].[Accounts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Domain] nvarchar(2147483647)  NOT NULL,
    [UserName] nvarchar(2147483647)  NOT NULL,
    [Tracking] blob  NOT NULL,
    [AllottedTime] int  NOT NULL,
    [UsedTime] int  NOT NULL,
    [ComputerId] int  NOT NULL
);
GO

-- Creating table 'Computers'
CREATE TABLE [dbo].[Computers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(2147483647)  NOT NULL,
    [Enviroment] nvarchar(2147483647)  NOT NULL,
    [IpAddress] nvarchar(2147483647)  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(2147483647)  NOT NULL,
    [Email] nvarchar(2147483647)  NOT NULL
);
GO

-- Creating table 'Programs'
CREATE TABLE [dbo].[Programs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(2147483647)  NOT NULL,
    [LastRun] datetime  NOT NULL,
    [OpenCount] int  NOT NULL,
    [AccountId] int  NOT NULL
);
GO

-- Creating table 'Histories'
CREATE TABLE [dbo].[Histories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Domain] nvarchar(2147483647)  NOT NULL,
    [Url] nvarchar(2147483647)  NOT NULL,
    [LastVisited] datetime  NOT NULL,
    [VisitCount] int  NOT NULL,
    [AccountId] int  NOT NULL
);
GO

-- Creating table 'Processes'
CREATE TABLE [dbo].[Processes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(2147483647)  NOT NULL,
    [LastRun] datetime  NOT NULL,
    [AccountId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [PK_Accounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Computers'
ALTER TABLE [dbo].[Computers]
ADD CONSTRAINT [PK_Computers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Programs'
ALTER TABLE [dbo].[Programs]
ADD CONSTRAINT [PK_Programs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Histories'
ALTER TABLE [dbo].[Histories]
ADD CONSTRAINT [PK_Histories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Processes'
ALTER TABLE [dbo].[Processes]
ADD CONSTRAINT [PK_Processes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'Computers'
ALTER TABLE [dbo].[Computers]
ADD CONSTRAINT [FK_UserComputer]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserComputer'
CREATE INDEX [IX_FK_UserComputer]
ON [dbo].[Computers]
    ([UserId]);
GO

-- Creating foreign key on [ComputerId] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [FK_ComputerAccount]
    FOREIGN KEY ([ComputerId])
    REFERENCES [dbo].[Computers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ComputerAccount'
CREATE INDEX [IX_FK_ComputerAccount]
ON [dbo].[Accounts]
    ([ComputerId]);
GO

-- Creating foreign key on [AccountId] in table 'Processes'
ALTER TABLE [dbo].[Processes]
ADD CONSTRAINT [FK_AccountProcess]
    FOREIGN KEY ([AccountId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountProcess'
CREATE INDEX [IX_FK_AccountProcess]
ON [dbo].[Processes]
    ([AccountId]);
GO

-- Creating foreign key on [AccountId] in table 'Histories'
ALTER TABLE [dbo].[Histories]
ADD CONSTRAINT [FK_AccountHistory]
    FOREIGN KEY ([AccountId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountHistory'
CREATE INDEX [IX_FK_AccountHistory]
ON [dbo].[Histories]
    ([AccountId]);
GO

-- Creating foreign key on [AccountId] in table 'Programs'
ALTER TABLE [dbo].[Programs]
ADD CONSTRAINT [FK_AccountProgram]
    FOREIGN KEY ([AccountId])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountProgram'
CREATE INDEX [IX_FK_AccountProgram]
ON [dbo].[Programs]
    ([AccountId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------