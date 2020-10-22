# SecureWebApp

Opposite of the [Unsecure WebApp](https://github.com/TomaszKandula/UnsecureWebApp) that has been written for the sole purpose of the article titled [SQL Injection](https://medium.com/&#64;tomasz.kandula/sql-injection-1bde8bb76ebc) being an extension of another article [I said goodbye to Stored Procedures](https://medium.com/swlh/i-said-goodbye-to-stored-procedures-539d56350486).

The purpose of this demo application is to present an example of an application that is in compliance with some security best practices describded in the article. Therefore:

1. User input is validated on the front-end and on the back-end.
1. We mostly use AJAX to perform asynchronous calls to Web API, secured by Anti-Forgery Token. 
1. Instead of ADO.NET with custom SQL string, we use OR/M (Entity Framework Core) and LINQ.
1. User password is hashed and salted with [BCrypt](https://auth0.com/blog/hashing-in-action-understanding-bcrypt/). Please note that we do not use SHA2_512 and GUID for hashing and salting on the server-side via SQL stored procedures (this would be the alternative approach, most likely implemented by DBA). Using BCrypt (or SCrypt) is much preferable.

## Tech-stack

### Front-end

1. Bulma CSS framework v.0.9.0 via CDN.
1. Bulma divider 0.2.0 via CDN.
1. Vanilla JavaScript with AJAX.
1. WebPack module bundler.

### Back-end

1. NET Core 3.1, C# language, Razor Pages, Web API.
1. SQL Database, Entity Framework Core.

Unit Tests and Integration Tests are provided using [XUnit](https://github.com/xunit/xunit) and [FluentAssertions](https://github.com/fluentassertions/fluentassertions).

## Setting-up the database

Connection string have to be setup in __secrets.json__ (SecureWebAapp project):

```
{

  "ConnectionStrings": 
  {
    "DbConnect": "<your_connection_string_goes_here>"
  }

}
```

The database have only four tables, because this example have already setup database context, one may use SQL script to create tables and populate Cities and Countries from CSV files, all provided here: [Database Examples](https://github.com/TomaszKandula/SecureWebApp/tree/master/DatabaseExamples). However, instead of executing SQL, one also may use __migrations__, in Package Manager Console type and execute command:

`update-database`

And EF Core will create database with all necessary tables. Then, we may populate the tables with the script (on localhost).

## Integration Tests

Focuses on testing HTTP responses (uses Anti-Forgery Token), dependencies and theirs configuration.

Note: due to the fact that we use Razor Pages, before running test, we will require to:

1. Place `xunit.runner.json` with `{"shadowCopy": false}` in `bin\debug\<TargetFramework>\` folder.
1. Add to the project file (already commited in this repo):

```
  <Target Name="CopyDepsFiles" AfterTargets="Build" Condition="'$(TargetFramework)'!=''">

    <ItemGroup>
      <DepsFilePaths Include="$([System.IO.Path]::ChangeExtension('%(_ResolvedProjectReferencePaths.FullPath)', '.deps.json'))" />
    </ItemGroup>

    <Copy SourceFiles="%(DepsFilePaths.FullPath)" DestinationFolder="$(OutputPath)" Condition="Exists('%(DepsFilePaths.FullPath)')" />
  </Target>
```

More details: [Integration tests with ASP.NET Core causes missing references from Razor files](https://github.com/aspnet/Razor/issues/1212).

## Unit Tests

Covers all the logic used in AJAX controller (note that endpoints does not provide any business logic, they are only responsible for handling requests, validation etc.). All dependencies are mocked. For mocking [Moq](https://github.com/moq/moq4) and [MockQueryable.Moq](https://github.com/romantitov/MockQueryable) have been used. 
