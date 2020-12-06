using System;
using System.Collections.Generic;
using SecureWebApp.Database.Models;

namespace SecureWebApp.UnitTests.Database
{

    public static class DummyLoad
    {

        public static List<Countries> ReturnDummyCountries()
        {

            var LCountries = new List<Countries>()
            {

                new Countries 
                { 
                    CountryName = "Poland",
                    CountryCode = "POL"
                },

                new Countries
                {
                    CountryName = "Spain",
                    CountryCode = "ESP"
                }

            };

            return LCountries;

        }

        public static List<Cities> ReturnDummyCities() 
        {

            var LCitises = new List<Cities>() 
            {  

                new Cities
                { 
                    CityName = "Poznan",
                    CountryId = 1               
                },

                new Cities
                {
                    CityName = "Barcelona",
                    CountryId = 2
                },

                new Cities
                {
                    CityName = "Madrid",
                    CountryId = 2
                }

            };

            return LCitises;

        }

        public static List<Users> ReturnDummyUsers() 
        {

            var LUsers = new List<Users>()
            {

                new Users
                {
                    FirstName   = "Bob",
                    LastName    = "Dylan",
                    NickName    = "Bobby",
                    EmailAddr   = "bob.dylan@gmail.com",
                    PhoneNum    = null,
                    Password    = "TestUnhashedPassword1",
                    CreatedAt   = DateTime.Now,
                    IsActivated = true,
                    CityId      = 1,
                    CountryId   = 1
                },

                new Users
                {
                    FirstName   = "Freddie",
                    LastName    = "Mercury",
                    NickName    = "Fred",
                    EmailAddr   = "f.mercury@gmail.com",
                    PhoneNum    = null,
                    Password    = "$2a$12$O0.Q.zXRLN7Frcn.xZzwFOHR2wJLtkUVEUq0Qstqgf18sMIfv5.Qy",
                    CreatedAt   = DateTime.Now,
                    IsActivated = true,
                    CityId      = 1,
                    CountryId   = 1
                }

            };

            return LUsers;

        }

        public static List<SigninHistory> ReturnSigninHistory() 
        {

            var LSigninHistory = new List<SigninHistory>()
            {
                
                new SigninHistory
                { 
                    UserId   = 1,
                    LoggedAt = DateTime.Parse("2020-07-30 15:30:00")
                },

                new SigninHistory
                {
                    UserId   = 2,
                    LoggedAt = DateTime.Parse("2020-07-05 11:30:00")
                }

            };

            return LSigninHistory;

        }

    }

}
