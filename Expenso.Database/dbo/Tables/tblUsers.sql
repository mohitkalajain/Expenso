CREATE TABLE [dbo].[tblUsers] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (100) NULL,
    [Email]        NVARCHAR (150) NOT NULL,
    [PasswordHash] NVARCHAR (MAX) NOT NULL,
    [RoleId]       INT            NOT NULL,
    [Provider]     NVARCHAR (50)  NULL,
    [City]         NVARCHAR (100) NULL,
    [Country]      NVARCHAR (100) NULL,
    [DateOfBirth]  DATE           NULL,
    [Gender]       NVARCHAR (10)  NULL,
    [CreatedAt]    DATETIME2 (7)  DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([RoleId]) REFERENCES [dbo].[tblRoles] ([Id]),
    UNIQUE NONCLUSTERED ([Email] ASC)
);

