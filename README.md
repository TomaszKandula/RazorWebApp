# Razor WebApp

Test Razor Pages application being an opposite of the [Unsecure WebApp](https://github.com/TomaszKandula/UnsecureWebApp) that has been written for the sole purpose of the article titled [SQL Injection](https://medium.com/&#64;tomasz.kandula/sql-injection-1bde8bb76ebc) being an extension of another article [I said goodbye to Stored Procedures](https://medium.com/swlh/i-said-goodbye-to-stored-procedures-539d56350486).

The idea behind this Razor Pages application is to build small web application using server-side rendering and plain JavaScript for client-side interactions (it uses classes and components). It should:

1. Validate user input: front-end via own code or validate.js (preferable); back-end via model valiation or FluentValidation (preferable).
1. Use AJAX to perform asynchronous calls to Web API, secured by Anti-Forgery Token. 
1. Use OR/M (Entity Framework Core) and LINQ instead of ADO.NET with custom SQL strings.
1. Protect user password by hashing and salting with [BCrypt](https://auth0.com/blog/hashing-in-action-understanding-bcrypt/). Please note that we do not use SHA2_512 and GUID for hashing and salting on the server-side via SQL stored procedures (this would be the alternative approach, most likely implemented by DBA). Using BCrypt (or SCrypt) is much preferable.

## Tech-stack

### Front-end

1. Bulma CSS framework v.0.9.0 via CDN.
1. Bulma divider 0.2.0 via CDN.
1. Vanilla JavaScript with AJAX.
1. WebPack module bundler.

User interaction is coded with plain JavaScript, no third party libraries/frameworks used. Code is organized into modules and each Razor Pages have its own dedicated JavaScript class that add events, perform binding and render buttons component and message box component (it returns HTML for given handle). While this is quite clean it and easy to maintain, such approach produces large overhead and it is not recommended for large production applications.

### Back-end

1. NET Core 3.1, C# language, Razor Pages, Web API.
1. SQL Database, Entity Framework Core.

Unit Tests and Integration Tests are provided using [XUnit](https://github.com/xunit/xunit) and [FluentAssertions](https://github.com/fluentassertions/fluentassertions).

Back-end project is relatively small and therefore it is not split into sub-projects/services.

## Setting-up the database

Setup the connection string:

```
{
  "ConnectionStrings": 
  {
    "DbConnect": "<your_connection_string_goes_here>"
  }
}
```

Migrate the database by executing following command:

`Update-Database -StartupProject SecureWebApp -Project SecureWebApp -Context MainDbContext`

EF Core will create database with all necessary tables. Then, to populate Cities and Countries tables with large dataset, use separate [SQL script](https://github.com/TomaszKandula/SecureWebApp/blob/master/CsvData/ImportCsvToDatabase.sql) to import it from local file to your local database.

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
