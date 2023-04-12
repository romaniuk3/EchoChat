CREATE TABLE [dbo].[UserChats] (
    [UserId] INT NOT NULL,
    [ChatId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([ChatId] ASC, [UserId] ASC),
    CONSTRAINT [FK_UserChats_Chats] FOREIGN KEY ([ChatId]) REFERENCES [dbo].[Chats] ([Id]),
    CONSTRAINT [FK_UserChats_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

