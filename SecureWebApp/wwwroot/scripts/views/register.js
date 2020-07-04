// View module to manipulate DOM

"use strict"


import * as _helpers from "../functions/helpers";
import * as _common  from "../functions/common";


function Input_FirstName(Event)
{

    let Verified  = document.getElementById("OK_FirstName");
    let Malformed = document.getElementById("ERR_FirstName");

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

    let Verified  = document.getElementById("OK_LastName");
    let Malformed = document.getElementById("ERR_LastName");

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

    let Verified  = document.getElementById("OK_Nickname");
    let Malformed = document.getElementById("ERR_Nickname");

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

    let Handler   = document.getElementById("Handle_EmailAddress");
    let Verified  = document.getElementById("OK_EmailAddress");
    let Malformed = document.getElementById("ERR_EmailAddress");

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

    let Handler   = document.getElementById("Handle_EmailAddress");
    let Verified  = document.getElementById("OK_EmailAddress");
    let Malformed = document.getElementById("ERR_EmailAddress");

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

    let Verified  = document.getElementById("OK_Password");
    let Malformed = document.getElementById("ERR_Password");

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

    let Handler    = document.getElementById("Handle_CityList");
    let SelectedId = Event.target.value;

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

    let Handler  = document.getElementById("Handle_CityList");
    let Selector = document.getElementById("Select_CityList");

    Handler.classList.remove("is-loading");

    if (StatusCode == 200)
    {

        _helpers.ClearSelectElement(Selector);

        for (let Index = 0; Index < ParsedResponse.Cities.length; Index++)
        {

            let City = ParsedResponse.Cities[Index];
            let Option = document.createElement("option");

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

    let CreateAccountButton = document.getElementById("Button_CreateAccount");

    if (IsChecked)
    {
        CreateAccountButton.disabled = false;
    }
    else
    {
        CreateAccountButton.disabled = true;
    }

}


function Button_CreateAccount()
{

    let UserInputData =
    {
        FirstName:    FirstNameInput.value,
        LastName:     LastNameInput.value,
        NickName:     NicknameInput.value,
        EmailAddress: EmailAddressInput.value,
        Password:     PasswordInput.value,
        CountryId:    CountryListSelect.value,
        CityId:       CountryList.value
    };

    //...

    return true;

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
