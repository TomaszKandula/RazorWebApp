# SecureWebAapp

Opposite of the __Unsecure WebApp__ example available at [Unsecure WebApp](https://github.com/TomaszKandula/UnsecureWebApp) that has been written for the sole purpose of the article [I said goodbye to Stored Procedures](https://medium.com/swlh/i-said-goodbye-to-stored-procedures-539d56350486).

## Purpose

The article __I said goodbye to Stored Procedures__ has been extended by another short article titled 
[SQL Injection](https://medium.com/&#64;tomasz.kandula/sql-injection-1bde8bb76ebc) that also has been posted on [Medium.com](https://medium.com/), and this demo application has been built as an example of an application that is in compliance with some security best practices describded in the article. 

## Tech-stack

### Front-end

1. Bulma CSS framework v.0.9.0 via CDN.
1. Bulma divider 0.2.0 via CDN.
1. Vanilla JavaScript with AJAX.
1. WebPack module bundler.

### Back-end

1. NET Core 3.1, C# language, Razor Pages, Web API.
1. SQL Database, Entity Framework Core.

Unit Tests and Integration Tests are provided.

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

and for Integration Test (SecureWebAapp.IntegrationTests project):

```
{
    "DbConnect": "<your_connection_string_goes_here>"
}
```

The database have only four tables, because this example have already setup database context, one may use SQL script to create tables and populate Cities and Countries from CSV files, all provided here: [Database Examples](https://github.com/TomaszKandula/SecureWebApp/tree/master/DatabaseExamples).

## Integration Tests

Focuses on testing dependencies and database setup with [XUnit](https://github.com/xunit/xunit) and [FluentAssertions](https://github.com/fluentassertions/fluentassertions). One dependecy is skipped (IAppLogger) that abstracts away used logger. There are no HTTP tests so far.

## Unit Tests

Covers all the logic used in AJAX controllers (note that endpoints are not provide any business logic, they are only responsible for handling requests, validation etc.). [XUnit](https://github.com/xunit/xunit) and [FluentAssertions](https://github.com/fluentassertions/fluentassertions) are used and all dependencies are mocked. For mocking, [Moq](https://github.com/moq/moq4) and [MockQueryable.Moq](https://github.com/romantitov/MockQueryable) have been used. 
