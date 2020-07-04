// Startup module to cache DOM and bind events

"use strict"


import * as _login    from "./views/login";
import * as _register from "./views/register";


document.addEventListener('DOMContentLoaded', () => Initialize());


function Initialize()
{

    const IsLoginPage    = document.getElementById("LoginForm");
    const IsRegisterPage = document.getElementById("RegisterForm");

    if (IsLoginPage)
    {

    }

    if (IsRegisterPage)
    {

        const FirstNameInput      = document.getElementById("Input_FirstName");
        const LastNameInput       = document.getElementById("Input_LastName");
        const NicknameInput       = document.getElementById("Input_Nickname");
        const EmailAddressInput   = document.getElementById("Input_EmailAddress");
        const PasswordInput       = document.getElementById("Input_Password");
        const CountryListSelect   = document.getElementById("Select_CountryList");
        const CityListSelect      = document.getElementById("Select_CityList");
        const TermsCheckbox       = document.getElementById("Handle_TermsCheckbox");
        const CreateAccountButton = document.getElementById("Button_CreateAccount");

        CityListSelect.selectedIndex = 0;
        CityListSelect.disabled = true;

        FirstNameInput.addEventListener("change", (event) => { _register.Input_FirstName(event); });
        LastNameInput.addEventListener("change", (event) => { _register.Input_LastName(event); });
        NicknameInput.addEventListener("change", (event) => { _register.Input_Nickname(event); });
        PasswordInput.addEventListener("change", (event) => { _register.Input_Password(event) });
        CountryListSelect.addEventListener("change", (event) => { _register.Select_CountryList(event); });

        TermsCheckbox.addEventListener("click", function () { _register.Handle_TermsCheckbox(this.checked); });
        CreateAccountButton.addEventListener("click", function () { _register.Button_CreateAccount(); });

        EmailAddressInput.onkeyup = function () { _register.Input_EmailAddress(EmailAddressInput.value); };

    }

}
