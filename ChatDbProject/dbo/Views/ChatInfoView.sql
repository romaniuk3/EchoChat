CREATE VIEW [dbo].[ChatInfoView]
	AS SELECT c.Id, c.Name, c.Description, COUNT(uc.UserId) AS UsersCount
	FROM [dbo].[Chats] c
	LEFT JOIN [dbo].[UserChats] uc ON uc.ChatId = c.Id
	GROUP BY c.Id, c.Name, c.Description;
