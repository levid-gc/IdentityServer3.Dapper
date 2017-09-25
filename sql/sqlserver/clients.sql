/* Uncomment it if you want drop these tables. */
/* 
IF object_id('[dbo].[ClientClaims]') IS NOT NULL 
BEGIN
	DROP TABLE [dbo].[ClientClaims]
END
IF object_id('[dbo].[ClientSecrets]') IS NOT NULL 
BEGIN
	DROP TABLE [dbo].[ClientSecrets]
END
IF object_id('[dbo].[ClientCustomGrantTypes]') IS NOT NULL 
BEGIN
	DROP TABLE [dbo].[ClientCustomGrantTypes]
END
IF object_id('[dbo].[ClientIdPRestrictions]') IS NOT NULL 
BEGIN
	DROP TABLE [dbo].[ClientIdPRestrictions]
END
IF object_id('[dbo].[ClientPostLogoutRedirectUris]') IS NOT NULL 
BEGIN
	DROP TABLE [dbo].[ClientPostLogoutRedirectUris]
END
IF object_id('[dbo].[ClientRedirectUris]') IS NOT NULL 
BEGIN
	DROP TABLE [dbo].[ClientRedirectUris] 
END
IF object_id('[dbo].[ClientScopes]') IS NOT NULL 
BEGIN
	DROP TABLE [dbo].[ClientScopes] 
END
IF object_id('[dbo].[ClientCorsOrigins]') IS NOT NULL 
BEGIN
	DROP TABLE [dbo].[ClientCorsOrigins] 
END
IF object_id('[dbo].[Clients]') IS NOT NULL 
BEGIN
	DROP TABLE [dbo].[Clients]
END
*/

CREATE TABLE [dbo].[Clients] (
    [Id] [nvarchar](128) NOT NULL,
    [Enabled] [bit] NOT NULL,
    [ClientId] [nvarchar](200) NOT NULL,
    [ClientName] [nvarchar](200) NOT NULL,
    [ClientUri] [nvarchar](2000),
    [LogoUri] [nvarchar](max),
    [RequireConsent] [bit] NOT NULL,
    [AllowRememberConsent] [bit] NOT NULL,
    [Flow] [int] NOT NULL,
    [IdentityTokenLifetime] [int] NOT NULL,
    [AccessTokenLifetime] [int] NOT NULL,
    [AuthorizationCodeLifetime] [int] NOT NULL,
    [AbsoluteRefreshTokenLifetime] [int] NOT NULL,
    [SlidingRefreshTokenLifetime] [int] NOT NULL,
    [RefreshTokenUsage] [int] NOT NULL,
    [RefreshTokenExpiration] [int] NOT NULL,
    [AccessTokenType] [int] NOT NULL,
    [EnableLocalLogin] [bit] NOT NULL,
    [IncludeJwtId] [bit] NOT NULL,
    [AlwaysSendClientClaims] [bit] NOT NULL,
    [PrefixClientClaims] [bit] NOT NULL,
    [AllowAccessToAllScopes] [bit] NOT NULL,
    [AllowAccessToAllGrantTypes] [bit] NOT NULL,
    [AllowClientCredentialsOnly] [bit] NOT NULL,
    [UpdateAccessTokenOnRefresh] [bit] NOT NULL,
    [LogoutUri] [nvarchar](max) NULL,
    [LogoutSessionRequired] [bit] NOT NULL,
    [RequireSignOutPrompt] [bit] NOT NULL,
    [AllowAccessTokensViaBrowser] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.Clients] PRIMARY KEY ([Id])
)
CREATE UNIQUE INDEX [IX_ClientId] ON [dbo].[Clients]([ClientId])

CREATE TABLE [dbo].[ClientClaims] (
    [Id] [nvarchar](128) NOT NULL,
    [Type] [nvarchar](250) NOT NULL,
    [Value] [nvarchar](250) NOT NULL,
    [Client_Id] [nvarchar](128) NOT NULL,
    CONSTRAINT [PK_dbo.ClientClaims] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Client_Id] ON [dbo].[ClientClaims]([Client_Id])

CREATE TABLE [dbo].[ClientSecrets] (
    [Id] [nvarchar](128) NOT NULL,
    [Value] [nvarchar](250) NOT NULL,
    [Type] [nvarchar](250),
    [Description] [nvarchar](2000),
    [Expiration] [datetimeoffset](7),
    [Client_Id] [nvarchar](128) NOT NULL,
    CONSTRAINT [PK_dbo.ClientSecrets] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Client_Id] ON [dbo].[ClientSecrets]([Client_Id])

CREATE TABLE [dbo].[ClientCustomGrantTypes] (
    [Id] [nvarchar](128) NOT NULL,
    [GrantType] [nvarchar](250) NOT NULL,
    [Client_Id] [nvarchar](128) NOT NULL,
    CONSTRAINT [PK_dbo.ClientCustomGrantTypes] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Client_Id] ON [dbo].[ClientCustomGrantTypes]([Client_Id])

CREATE TABLE [dbo].[ClientIdPRestrictions] (
    [Id] [nvarchar](128) NOT NULL,
    [Provider] [nvarchar](200) NOT NULL,
    [Client_Id] [nvarchar](128) NOT NULL,
    CONSTRAINT [PK_dbo.ClientIdPRestrictions] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Client_Id] ON [dbo].[ClientIdPRestrictions]([Client_Id])

CREATE TABLE [dbo].[ClientPostLogoutRedirectUris] (
    [Id] [nvarchar](128) NOT NULL,
    [Uri] [nvarchar](2000) NOT NULL,
    [Client_Id] [nvarchar](128) NOT NULL,
    CONSTRAINT [PK_dbo.ClientPostLogoutRedirectUris] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Client_Id] ON [dbo].[ClientPostLogoutRedirectUris]([Client_Id])

CREATE TABLE [dbo].[ClientRedirectUris] (
    [Id] [nvarchar](128) NOT NULL,
    [Uri] [nvarchar](2000) NOT NULL,
    [Client_Id] [nvarchar](128) NOT NULL,
    CONSTRAINT [PK_dbo.ClientRedirectUris] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Client_Id] ON [dbo].[ClientRedirectUris]([Client_Id])

CREATE TABLE [dbo].[ClientScopes] (
    [Id] [nvarchar](128) NOT NULL,
    [Scope] [nvarchar](200) NOT NULL,
    [Client_Id] [nvarchar](128) NOT NULL,
    CONSTRAINT [PK_dbo.ClientScopes] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Client_Id] ON [dbo].[ClientScopes]([Client_Id])

CREATE TABLE [dbo].[ClientCorsOrigins] (
    [Id] [nvarchar](128) NOT NULL,
    [Origin] [nvarchar](150) NOT NULL,
    [Client_Id] [nvarchar](128) NOT NULL,
    CONSTRAINT [PK_dbo.ClientCorsOrigins] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Client_Id] ON [dbo].[ClientCorsOrigins]([Client_Id])

ALTER TABLE [dbo].[ClientClaims] ADD CONSTRAINT [FK_dbo.ClientClaims_dbo.Clients_Client_Id] FOREIGN KEY ([Client_Id]) REFERENCES [dbo].[Clients] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[ClientSecrets] ADD CONSTRAINT [FK_dbo.ClientSecrets_dbo.Clients_Client_Id] FOREIGN KEY ([Client_Id]) REFERENCES [dbo].[Clients] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[ClientCustomGrantTypes] ADD CONSTRAINT [FK_dbo.ClientCustomGrantTypes_dbo.Clients_Client_Id] FOREIGN KEY ([Client_Id]) REFERENCES [dbo].[Clients] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[ClientIdPRestrictions] ADD CONSTRAINT [FK_dbo.ClientIdPRestrictions_dbo.Clients_Client_Id] FOREIGN KEY ([Client_Id]) REFERENCES [dbo].[Clients] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[ClientPostLogoutRedirectUris] ADD CONSTRAINT [FK_dbo.ClientPostLogoutRedirectUris_dbo.Clients_Client_Id] FOREIGN KEY ([Client_Id]) REFERENCES [dbo].[Clients] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[ClientRedirectUris] ADD CONSTRAINT [FK_dbo.ClientRedirectUris_dbo.Clients_Client_Id] FOREIGN KEY ([Client_Id]) REFERENCES [dbo].[Clients] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[ClientScopes] ADD CONSTRAINT [FK_dbo.ClientScopes_dbo.Clients_Client_Id] FOREIGN KEY ([Client_Id]) REFERENCES [dbo].[Clients] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[ClientCorsOrigins] ADD CONSTRAINT [FK_dbo.ClientCorsOrigins_dbo.Clients_Client_Id] FOREIGN KEY ([Client_Id]) REFERENCES [dbo].[Clients] ([Id]) ON DELETE CASCADE

ALTER TABLE [dbo].[Clients] ADD DEFAULT ((0)) FOR [AllowAccessToAllScopes]
ALTER TABLE [dbo].[Clients] ADD DEFAULT ((0)) FOR [AllowAccessToAllGrantTypes]
ALTER TABLE [dbo].[Clients] ADD DEFAULT ((0)) FOR [AllowClientCredentialsOnly]
ALTER TABLE [dbo].[Clients] ADD DEFAULT ((0)) FOR [UpdateAccessTokenOnRefresh]
ALTER TABLE [dbo].[Clients] ADD DEFAULT ((0)) FOR [LogoutSessionRequired]
ALTER TABLE [dbo].[Clients] ADD DEFAULT ((0)) FOR [RequireSignOutPrompt]
ALTER TABLE [dbo].[Clients] ADD DEFAULT ((1)) FOR [AllowAccessTokensViaBrowser]