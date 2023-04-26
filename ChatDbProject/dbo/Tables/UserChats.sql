CREATE TABLE [dbo].[UserChats] (
    [UserId] INT NOT NULL,
    [ChatId] INT NOT NULL,
    [Id] INT NOT NULL IDENTITY, 
    CONSTRAINT [FK_UserChats_Chats] FOREIGN KEY ([ChatId]) REFERENCES [dbo].[Chats] ([Id]),
    CONSTRAINT [FK_UserChats_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]), 
    CONSTRAINT [PK_UserChats] PRIMARY KEY ([Id])
);

