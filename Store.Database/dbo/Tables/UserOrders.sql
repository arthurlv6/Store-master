CREATE TABLE [dbo].[UserOrders] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (MAX) NULL,
    [Phone]       NVARCHAR (MAX) NULL,
    [Address]     NVARCHAR (MAX) NULL,
    [CreatedDate] DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_UserOrders] PRIMARY KEY CLUSTERED ([Id] ASC)
);

