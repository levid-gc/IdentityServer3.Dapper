/* Uncomment it if you want drop these tables. */
/* 
IF object_id('[dbo].[ScopeClaims]') IS NOT NULL 
BEGIN
	DROP TABLE [dbo].[ScopeClaims]
END
IF object_id('[dbo].[ScopeSecrets]') IS NOT NULL 
BEGIN
	DROP TABLE [dbo].[ScopeSecrets]
END
IF object_id('[dbo].[Scopes]') IS NOT NULL 
BEGIN
	DROP TABLE [dbo].[Scopes]
END
*/

CREATE TABLE [dbo].[Scopes] (
    [Id] [nvarchar](128) NOT NULL,
    [Enabled] [bit] NOT NULL,
    [Name] [nvarchar](200) NOT NULL,
    [DisplayName] [nvarchar](200),
    [Description] [nvarchar](1000),
    [Required] [bit] NOT NULL,
    [Emphasize] [bit] NOT NULL,
    [Type] [int] NOT NULL,
    [IncludeAllClaimsForUser] [bit] NOT NULL,
    [ClaimsRule] [nvarchar](200),
    [ShowInDiscoveryDocument] [bit] NOT NULL,
    [AllowUnrestrictedIntrospection] [bit] NOT NULL,
    CONSTRAINT [PK_dbo.Scopes] PRIMARY KEY ([Id])
)

CREATE TABLE [dbo].[ScopeClaims] (
    [Id] [nvarchar](128) NOT NULL,
    [Name] [nvarchar](200) NOT NULL,
    [Description] [nvarchar](1000),
    [AlwaysIncludeInIdToken] [bit] NOT NULL,
    [Scope_Id] [nvarchar](128) NOT NULL,
    CONSTRAINT [PK_dbo.ScopeClaims] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Scope_Id] ON [dbo].[ScopeClaims]([Scope_Id])

CREATE TABLE [dbo].[ScopeSecrets] (
    [Id] [nvarchar](128) NOT NULL,
    [Description] [nvarchar](1000),
    [Expiration] [datetimeoffset](7),
    [Type] [nvarchar](250),
    [Value] [nvarchar](250) NOT NULL,
    [Scope_Id] [nvarchar](128) NOT NULL,
    CONSTRAINT [PK_dbo.ScopeSecrets] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_Scope_Id] ON [dbo].[ScopeSecrets]([Scope_Id])

ALTER TABLE [dbo].[ScopeClaims] ADD CONSTRAINT [FK_dbo.ScopeClaims_dbo.Scopes_Scope_Id] FOREIGN KEY ([Scope_Id]) REFERENCES [dbo].[Scopes] ([Id]) ON DELETE CASCADE
ALTER TABLE [dbo].[ScopeSecrets] ADD CONSTRAINT [FK_dbo.ScopeSecrets_dbo.Scopes_Scope_Id] FOREIGN KEY ([Scope_Id]) REFERENCES [dbo].[Scopes] ([Id]) ON DELETE CASCADE

ALTER TABLE [dbo].[Scopes] ADD DEFAULT ((0)) FOR [AllowUnrestrictedIntrospection]