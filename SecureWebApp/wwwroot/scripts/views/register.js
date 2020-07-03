
"use strict"


import * as _helpers from "../functions/helpers";
import * as _common  from "../functions/common";


function Input_FirstName(Event)
{

    var Verified  = document.getElementById("OK_FirstName");
    var Malformed = document.getElementById("ERR_FirstName");

    if (_helpers.IsEmpty(Event.target.value))
    {
        Verified.style.visibility  = "hidden";
        Malformed.style.visibility = "visible";
    }
    else
    {
        Verified.style.visibility  = "visible";
        Malformed.style.visibility = "hidden";
    }

}


function Input_LastName(Event)
{

    var Verified  = document.getElementById("OK_LastName");
    var Malformed = document.getElementById("ERR_LastName");

    if (_helpers.IsEmpty(Event.target.value))
    {
        Verified.style.visibility  = "hidden";
        Malformed.style.visibility = "visible";
    }
    else
    {
        Verified.style.visibility  = "visible";
        Malformed.style.visibility = "hidden";
    }

}


function Input_Nickname(Event)
{

    var Verified  = document.getElementById("OK_Nickname");
    var Malformed = document.getElementById("ERR_Nickname");

    if (_helpers.IsEmpty(Event.target.value))
    {
        Verified.style.visibility  = "hidden";
        Malformed.style.visibility = "visible";
    }
    else
    {
        Verified.style.visibility  = "visible";
        Malformed.style.visibility = "hidden";
    }

}


function Input_EmailAddress(EmailAddress)
{

    var Handler   = document.getElementById("Handle_EmailAddress");
    var Verified  = document.getElementById("OK_EmailAddress");
    var Malformed = document.getElementById("ERR_EmailAddress");

    Verified.style.display  = "visibility";
    Malformed.style.display = "visibility";
    Handler.classList.add("is-loading");

    if (!_helpers.IsEmpty(EmailAddress) && _helpers.ValidateEmail(EmailAddress))
    {

        _common.PerformAjaxCall(
            "GET",
            window.location.origin + "/api/v1/ajax/validation/" + EmailAddress + "/",
            null,
            CheckEmailAddress_Callback
        );

    }
    else
    {
        Verified.style.visibility  = "hidden";
        Malformed.style.visibility = "hidden";
        Handler.classList.remove("is-loading");
    }

}


function CheckEmailAddress_Callback(ParsedResponse, StatusCode)
{

    var Handler   = document.getElementById("Handle_EmailAddress");
    var Verified  = document.getElementById("OK_EmailAddress");
    var Malformed = document.getElementById("ERR_EmailAddress");

    Handler.classList.remove("is-loading");

    if (StatusCode === 200)
    {

        if (ParsedResponse.IsEmailValid)
        {
            Verified.style.visibility  = "visible";
            Malformed.style.visibility = "hidden";
        }
        else
        {
            Verified.style.visibility  = "hidden";
            Malformed.style.visibility = "visible";
        }

    }
    else
    {
        Verified.style.visibility  = "hidden";
        Malformed.style.visibility = "hidden";
        alert("An error has occured during the processing. Returned status code: " + StatusCode + ".");
    }

} 


function Input_Password(Event)
{

    var Verified  = document.getElementById("OK_Password");
    var Malformed = document.getElementById("ERR_Password");

    if (!_common.ValidatePasswordField(Event.target.value))
    {
        Verified.style.visibility  = "hidden";
        Malformed.style.visibility = "visible";
    }
    else
    {
        Verified.style.visibility  = "visible";
        Malformed.style.visibility = "hidden";
    }

}


function Select_CountryList(Event)
{

    var Handler    = document.getElementById("Handle_CityList");
    var SelectedId = Event.target.value;

    Handler.classList.add("is-loading");

    _common.PerformAjaxCall(
        "GET",
        window.location.origin + "/api/v1/ajax/cities/" + SelectedId + "/",
        null,
        GetCountryList_Callback
    );

}


function GetCountryList_Callback(ParsedResponse, StatusCode)
{

    var Handler  = document.getElementById("Handle_CityList");
    var Selector = document.getElementById("Select_CityList");

    Handler.classList.remove("is-loading");

    if (StatusCode == 200)
    {

        _helpers.ClearSelectElement(Selector);

        for (var Index = 0; Index < ParsedResponse.Cities.length; Index++)
        {

            var City = ParsedResponse.Cities[Index];
            var Option = document.createElement("option");

            Option.value = City.id;
            Option.innerHTML = City.name;
            Selector.appendChild(Option);

        }

        Selector.removeAttribute("disabled");
        Selector.selectedIndex = 0;

    }
    else
    {
        alert("An error has occured during the processing. Returned status code: " + StatusCode + ".");
    }

}


function Handle_TermsCheckbox(IsChecked)
{

    if (IsChecked)
    {
        document.getElementById("Button_CreateAccount").disabled = false;
    }
    else
    {
        document.getElementById("Button_CreateAccount").disabled = true;
    }

}


function Button_CreateAccount()
{

    //var UserInputData =
    //{
    //    FirstName:    FirstNameInput.value,
    //    LastName:     LastNameInput.value,
    //    NickName:     NicknameInput.value,
    //    EmailAddress: EmailAddressInput.value,
    //    Password:     PasswordInput.value,
    //    Country:      CountryListSelect.value,
    //    City:         CountryList.value
    //};

    //...

}


export
{
    Input_FirstName,
    Input_LastName,
    Input_Nickname,
    Input_EmailAddress,
    Input_Password,
    Select_CountryList,
    Handle_TermsCheckbox,
    Button_CreateAccount
};
