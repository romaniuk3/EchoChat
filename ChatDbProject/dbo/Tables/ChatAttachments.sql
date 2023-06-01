CREATE TABLE [dbo].[ChatAttachments]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[SenderId] INT NOT NULL,
	[FileName] NVARCHAR(255) NOT NULL,
	[FileText] NVARCHAR(MAX) NULL, 
    [ChatId] INT NOT NULL, 
    CONSTRAINT [FK_ChatAttachments_Users] FOREIGN KEY ([SenderId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_ChatAttachments_Chats] FOREIGN KEY ([ChatId]) REFERENCES [Chats]([Id])
)
