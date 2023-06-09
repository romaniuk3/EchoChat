CREATE TABLE [dbo].[ChatAttachments]
(
	[Id] INT NOT NULL IDENTITY,
	[SenderId] INT NOT NULL,
	[FileName] NVARCHAR(255) NOT NULL,
	[FileText] NVARCHAR(MAX) NULL, 
    [ChatId] INT NOT NULL, 
	[RequiresSignature] BIT NULL DEFAULT 0, 
    [ReceiverId] INT NULL, 
    [AttachmentUrl] NVARCHAR(MAX) NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ChatAttachments_Users_Sender] FOREIGN KEY ([SenderId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_ChatAttachments_Chats] FOREIGN KEY ([ChatId]) REFERENCES [Chats]([Id]), 
    CONSTRAINT [FK_ChatAttachments_Users_Receiver] FOREIGN KEY ([ReceiverId]) REFERENCES [AspNetUsers]([Id])
)
