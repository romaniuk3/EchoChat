﻿CREATE TABLE [dbo].[Messages] (
    [Id]           INT      NOT NULL,
    [CreatedDate]  DATETIME NOT NULL,
    [ModifiedDate] DATETIME NULL,
    [ChatId]       INT      NOT NULL,
    [UserId]       INT      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Messages_Chats] FOREIGN KEY ([ChatId]) REFERENCES [dbo].[Chats] ([Id]),
    CONSTRAINT [FK_Messages_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

