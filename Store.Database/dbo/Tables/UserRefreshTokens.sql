CREATE TABLE [dbo].[UserRefreshTokens] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (MAX) NULL,
    [RefreshToken] NVARCHAR (MAX) NULL,
    [Nickname]     NVARCHAR (MAX) NULL,
    [Sex]          NVARCHAR (MAX) NULL,
    [City]         NVARCHAR (MAX) NULL,
    [Country]      NVARCHAR (MAX) NULL,
    [Headimgurl]   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_UserRefreshTokens] PRIMARY KEY CLUSTERED ([Id] ASC)
);

