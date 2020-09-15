CREATE TABLE [dbo].[Products] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (MAX)  NULL,
    [Code]              NVARCHAR (MAX)  NULL,
    [Style]             NVARCHAR (MAX)  NULL,
    [Color]             NVARCHAR (MAX)  NULL,
    [Size]              NVARCHAR (MAX)  NULL,
    [Price]             DECIMAL (18, 2) NOT NULL,
    [Quatity]           INT             NOT NULL,
    [Description]       NVARCHAR (MAX)  NULL,
    [RRP]               DECIMAL (18, 2) NULL,
    [CreatedDate]       DATETIME2 (7)   NOT NULL,
    [ProductCategoryId] INT             NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Products_ProductCategories_ProductCategoryId] FOREIGN KEY ([ProductCategoryId]) REFERENCES [dbo].[ProductCategories] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Products_ProductCategoryId]
    ON [dbo].[Products]([ProductCategoryId] ASC);

