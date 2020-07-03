
"use strict"


import * as _validation from "./functions/validation";
import * as _modals     from "./functions/modals";
import * as _ajax       from "./functions/ajax";


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

        const FirstNameInput = document.getElementById("Input_FirstName");
        FirstNameInput.addEventListener("change", (event) =>
        {
            const Icon_Ok  = "OK_FirstName";
            const Icon_Err = "ERR_FirstName";
            _validation.ValidateEditTextField(event.target.value, Icon_Ok, Icon_Err);
        });

        const LastNameInput = document.getElementById("Input_LastName");
        LastNameInput.addEventListener("change", (event) =>
        {
            const Icon_Ok  = "OK_LastName";
            const Icon_Err = "ERR_LastName";
            _validation.ValidateEditTextField(event.target.value, Icon_Ok, Icon_Err);
        });

        const NicknameInput = document.getElementById("Input_Nickname");
        NicknameInput.addEventListener("change", (event) =>
        {
            const Icon_Ok  = "OK_Nickname";
            const Icon_Err = "ERR_Nickname";
            _validation.ValidateEditTextField(event.target.value, Icon_Ok, Icon_Err);
        });

        const EmailAddressInput = document.getElementById("Input_EmailAddress");
        EmailAddressInput.onkeyup = function()
        {
            const TargetHandler = "Handle_EmailAddress";
            const Icon_Ok       = "OK_EmailAddress";
            const Icon_NonOk    = "ERR_EmailAddress";
            _ajax.ValidateEmailAddress(EmailAddressInput.value, TargetHandler, Icon_Ok, Icon_NonOk);
        };

        const PasswordInput = document.getElementById("Input_Password");
        PasswordInput.addEventListener("change", (event) =>
        {
            const Icon_Ok  = "OK_Password";
            const Icon_Err = "ERR_Password";
            _validation.ValidatePasswordField(event.target.value, Icon_Ok, Icon_Err);
        });

        const CountryListSelect = document.getElementById("Select_CountryList");
        CountryListSelect.addEventListener("change", (event) => 
        {
            const TargetSelector = "Select_CityList";
            const TargetHandler  = "Handle_CityList";
            _ajax.GetCityList(event.target.value, TargetHandler, TargetSelector);
        });

        const CountryList = document.getElementById("Select_CityList");
        CountryList.selectedIndex = 0;
        CountryList.disabled = true;

        const TermsCheckbox = document.getElementById("Handle_TermsCheckbox");
        TermsCheckbox.addEventListener("click", function()
        {

            if (this.checked)
            {
                document.getElementById("Button_CreateAccount").disabled = false;
            }
            else
            {
                document.getElementById("Button_CreateAccount").disabled = true;
            }

        });

        const CreateAccountButton = document.getElementById("Button_CreateAccount");
        CreateAccountButton.addEventListener("click", function ()
        {
            // ...ajax call
        });

    }

}
