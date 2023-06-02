CREATE TABLE [dbo].[ChatAttachments]
(
	[Id] INT NOT NULL IDENTITY,
	[SenderId] INT NOT NULL,
	[FileName] NVARCHAR(255) NOT NULL,
	[FileText] NVARCHAR(MAX) NULL, 
    [ChatId] INT NOT NULL, 
	PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ChatAttachments_Users] FOREIGN KEY ([SenderId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_ChatAttachments_Chats] FOREIGN KEY ([ChatId]) REFERENCES [Chats]([Id])
)
