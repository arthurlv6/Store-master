CREATE TABLE [dbo].[UserOrderLines] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (MAX)  NULL,
    [UserOrderId] INT             NOT NULL,
    [ProductId]   INT             NOT NULL,
    [Code]        NVARCHAR (MAX)  NULL,
    [Style]       NVARCHAR (MAX)  NULL,
    [Color]       NVARCHAR (MAX)  NULL,
    [Size]        NVARCHAR (MAX)  NULL,
    [Price]       DECIMAL (18, 2) NOT NULL,
    [Quatity]     INT             NOT NULL,
    [Description] NVARCHAR (MAX)  NULL,
    [RRP]         DECIMAL (18, 2) NULL,
    CONSTRAINT [PK_UserOrderLines] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserOrderLines_UserOrders_UserOrderId] FOREIGN KEY ([UserOrderId]) REFERENCES [dbo].[UserOrders] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_UserOrderLines_UserOrderId]
    ON [dbo].[UserOrderLines]([UserOrderId] ASC);

