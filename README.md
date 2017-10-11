# IdentityServer3.Dapper

Dapper for IdentityServer3 including Clinet, Scope and Token etc.

This project is inspired by [IdentityServer3.EntityFramework](https://github.com/IdentityServer/IdentityServer3.EntityFramework), and We want to create a lib for people who counld not use EntityFramework.

We haven't make a throughly tests for our lib, so if you want use it, please be aware of some potential bugs.

### Supported Databases

We plan to support SQL Server, DB2, Oracle and MySQL, but at this stage, we just have complete adaption for [SQL Server](./IdentityServer3.Dapper.Tests/IntegrationTests/SQLServer/Sql) and [DB2](./IdentityServer3.Dapper.Tests/IntegrationTests/DB2/Sql) (may have bugs).

### Features

* Multi-Database support (currently, SQL Server and DB2)
* Table Name customization
* Table Schema customization

### Others

Our project is based on [Dapper](https://github.com/StackExchange/Dapper) and [Dapper-Extensions](https://github.com/tmsmith/Dapper-Extensions), and we also reference some code implementation in [IdentityServer3.EntityFramework](https://github.com/IdentityServer/IdentityServer3.EntityFramework), so if you want custom this project to support more features, we think these projects may give you some help.