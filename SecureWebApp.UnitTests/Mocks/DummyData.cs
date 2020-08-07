using System;
using System.Collections.Generic;
using SecureWebApp.Models.Database;

namespace SecureWebApp.UnitTests.Mocks
{

    public static class DummyData
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
                    IsActivated = false,
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
                    Password    = "TestUnhashedPassword2",
                    CreatedAt   = DateTime.Now,
                    IsActivated = false,
                    CityId      = 1,
                    CountryId   = 1
                }

            };

            return LUsers;

        }

    }

}
