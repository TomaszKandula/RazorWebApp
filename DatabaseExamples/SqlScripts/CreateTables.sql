
CREATE TABLE Countries
(
    Id          INT IDENTITY(1,1) NOT NULL,
    CountryName NVARCHAR(255) NOT NULL,
    CountryCode VARCHAR(2) NOT NULL,
    CONSTRAINT PK__Id__Countries PRIMARY KEY (Id)
)


CREATE TABLE Cities
(
    Id          INT IDENTITY(1,1) NOT NULL,
    CountryId   INT NOT NULL,
    CityName    NVARCHAR(255) NOT NULL,
    CONSTRAINT PK__Id__Cities PRIMARY KEY (Id),
    CONSTRAINT FK__CountryId__Cities FOREIGN KEY (CountryId) REFERENCES Countries (Id)
)


CREATE TABLE Users
(
    Id          INT IDENTITY(1,1) NOT NULL,
    CountryId   INT NOT NULL,
    CityId      INT NOT NULL,
    FirstName   NVARCHAR(255) NOT NULL,
    LastName    NVARCHAR(255) NOT NULL,
    NickName    NVARCHAR(255) NOT NULL,
    EmailAddr   VARCHAR(255) NOT NULL,
    PhoneNum    CHAR(15) NULL,
    Password    VARCHAR(255) NOT NULL,
    CreatedAt   DATETIME NOT NULL,
    IsActivated BIT NOT NULL,
    CONSTRAINT PK__Id__Users PRIMARY KEY (Id),
    CONSTRAINT UQ__EmailAddr__Users UNIQUE (EmailAddr),
    CONSTRAINT FK__CountryId__Users FOREIGN KEY (CountryId) REFERENCES Countries (Id),
    CONSTRAINT FK__CityId__Users FOREIGN KEY (CityId) REFERENCES Cities (Id),
)


CREATE TABLE SigninHistory
(
    Id          INT IDENTITY(1,1) NOT NULL,
    UserId	INT NOT NULL,
    LoggedAt    DATETIME NOT NULL,
    CONSTRAINT PK__Id__SigninHistory PRIMARY KEY (Id),
    CONSTRAINT FK__UserId__Users FOREIGN KEY (UserId) REFERENCES Users (Id)
)
