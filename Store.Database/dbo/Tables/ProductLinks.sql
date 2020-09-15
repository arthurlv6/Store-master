CREATE TABLE [dbo].[ProductLinks] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (MAX) NULL,
    [Type]        NVARCHAR (MAX) NULL,
    [Address]     NVARCHAR (MAX) NULL,
    [ProductId]   INT            NOT NULL,
    [CreatedDate] DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_ProductLinks] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProductLinks_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ProductLinks_ProductId]
    ON [dbo].[ProductLinks]([ProductId] ASC);

