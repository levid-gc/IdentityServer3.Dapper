### Something to be noticed about the sqls we provided

Original sqls: https://github.com/IdentityServer/IdentityServer3.EntityFramework/tree/master/sql

In our project, we have modified the default type of `Id`s from `int` to `string`, so in our sqls, for example, `[Id] [int] NOT NULL IDENTITY` all have changed to `[Id] [nvarchar](128) NOT NULL`