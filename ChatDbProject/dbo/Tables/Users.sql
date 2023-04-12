CREATE TABLE [dbo].[Users] (
    [Id]          INT            NOT NULL,
    [Email]       NVARCHAR (255) NOT NULL,
    [Password]    NVARCHAR (MAX) NOT NULL,
    [PhoneNumber] NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

