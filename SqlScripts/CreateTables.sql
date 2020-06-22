
create table Countries
(
    Id          int identity(1,1) not null,
    CountryName nvarchar(255) not null,
    CountryCode varchar(2) not null,
    CONSTRAINT PK__Id__Countries PRIMARY KEY (Id)
)


create table Cities
(
    Id          int identity(1,1) not null,
    CountryId   int not null,
    CityName    nvarchar(255) not null,
    CONSTRAINT PK__Id__Cities PRIMARY KEY (Id),
    CONSTRAINT FK__CountryId__Cities FOREIGN KEY (CountryId) REFERENCES Countries (Id)
)


create table Users
(
    Id          int identity(1,1) not null,
    CountryId   int not null,
    CityId      int not null,
    FirstName   nvarchar(255) not null,
    LastName    nvarchar(255) not null,
    NickName    nvarchar(255) not null,
    EmailAddr   varchar(255) not null,
    PhoneNum    char(15) not null,
    CONSTRAINT PK__Id__Users PRIMARY KEY (Id),
    CONSTRAINT UQ__EmailAddr__Users UNIQUE (EmailAddr),
    CONSTRAINT FK__CountryId__Users FOREIGN KEY (CountryId) REFERENCES Countries (Id),
    CONSTRAINT FK__CityId__Users FOREIGN KEY (CityId) REFERENCES Cities (Id)
)
