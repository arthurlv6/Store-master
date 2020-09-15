CREATE TABLE [dbo].[ProductCategories] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (MAX) NULL,
    [ShowOrder] INT            NOT NULL,
    [ParentId]  INT            NULL,
    CONSTRAINT [PK_ProductCategories] PRIMARY KEY CLUSTERED ([Id] ASC)
);

