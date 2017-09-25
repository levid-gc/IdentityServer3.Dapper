/* Uncomment it if you want drop these tables. */
/* 
IF object_id('[dbo].[Consents]') IS NOT NULL 
BEGIN
	DROP TABLE [dbo].[Consents]
END
IF object_id('[dbo].[Tokens]') IS NOT NULL 
BEGIN
	DROP TABLE [dbo].[Tokens]
END
*/

CREATE TABLE [dbo].[Consents] (
    [Subject] [nvarchar](200) NOT NULL,
    [ClientId] [nvarchar](200) NOT NULL,
    [Scopes] [nvarchar](2000) NOT NULL,
    CONSTRAINT [PK_dbo.Consents] PRIMARY KEY ([Subject], [ClientId])
)

CREATE TABLE [dbo].[Tokens] (
    [Key] [nvarchar](128) NOT NULL,
    [TokenType] [smallint] NOT NULL,
    [SubjectId] [nvarchar](200),
    [ClientId] [nvarchar](200) NOT NULL,
    [JsonCode] [nvarchar](max) NOT NULL,
    [Expiry] [datetimeoffset](7) NOT NULL,
    CONSTRAINT [PK_dbo.Tokens] PRIMARY KEY ([Key], [TokenType])
)