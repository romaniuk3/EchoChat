CREATE TABLE [dbo].[Chats] (
    [Id]   INT            NOT NULL IDENTITY,
    [Name] NVARCHAR (255) NOT NULL,
    [Description] NVARCHAR(MAX) NOT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

