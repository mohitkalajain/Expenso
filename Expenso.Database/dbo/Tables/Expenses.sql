CREATE TABLE [dbo].[Expenses] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [Amount]        DECIMAL (18, 2) NOT NULL,
    [Date]          DATETIME2 (7)   NOT NULL,
    [PaymentMethod] NVARCHAR (MAX)  NOT NULL,
    [Note]          NVARCHAR (MAX)  NOT NULL,
    [CategoryId]    INT             NOT NULL,
    CONSTRAINT [PK_Expenses] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Expenses_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Expenses_CategoryId]
    ON [dbo].[Expenses]([CategoryId] ASC);

