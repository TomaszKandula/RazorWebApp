// View module to manipulate Virtual DOM

"use strict";

import Helpers      from "../functions/helpers";
import RestClient   from "../functions/restClient";
import MessageBox   from "../components/messageBox";
import LoginButtons from "../components/loginButtons";

export default class RegisterPage
{

    constructor(AContainer, ANavButtons)
    {
        this.Container  = AContainer;
        this.NavButtons = ANavButtons;
    }

    Initialize()
    {

        if (this.Container === null)
        {
            return null;
        }

        this.BindDom();
        this.AddEvents();
        this.InitErrorCheck();

        this.CountryListSelect.selectedIndex = 0;
        this.CityListSelect.disabled = true;

        this.Dialog  = new MessageBox(this.ModalWindowHandle);
        this.Ajax    = new RestClient(this.Container.dataset.xsrf, "application/json; charset=UTF-8");
        this.Helpers = new Helpers();

        this.Render_Buttons();

    }

    Render_Buttons()
    {

        let MoveToRegister = function () { window.location.replace(`${window.location.origin}/register`) };
        let MoveToLogin = function () { window.location.replace(`${window.location.origin}/login`) };

        this.LoginButtons = new LoginButtons(this.NavButtons, "Signup_Login", MoveToRegister, MoveToLogin, null);
        this.LoginButtons.Show();

    }

    BindDom()
    {
        
        this.FirstNameInput = this.Container.querySelector("#Input_FirstName");
        this.LastNameInput  = this.Container.querySelector("#Input_LastName");
        this.NicknameInput  = this.Container.querySelector("#Input_Nickname");
        this.EmailAddrInput = this.Container.querySelector("#Input_EmailAddress");
        this.PasswordInput  = this.Container.querySelector("#Input_Password");

        this.CountryListSelect = this.Container.querySelector("#Select_CountryList");
        this.CityListSelect    = this.Container.querySelector("#Select_CityList");

        this.CreateAccountHandle = this.Container.querySelector("#Handle_CreateAccount");
        this.CreateAccountButton = this.Container.querySelector("#Button_CreateAccount");

        this.ModalWindowHandle = this.Container.querySelector("#Handle_Modal");

        this.TermsLink   = this.Container.querySelector("#Link_Terms");
        this.PrivacyLink = this.Container.querySelector("#Link_Privacy");

        this.OK_FirstName   = this.Container.querySelector("#OK_FirstName");
        this.ERR_FirstName  = this.Container.querySelector("#ERR_FirstName");
        this.Info_FirstName = this.Container.querySelector("#Info_FirstName");

        this.OK_LastName   = this.Container.querySelector("#OK_LastName");
        this.ERR_LastName  = this.Container.querySelector("#ERR_LastName");
        this.Info_LastName = this.Container.querySelector("#Info_LastName");

        this.OK_Nickname   = this.Container.querySelector("#OK_Nickname");
        this.ERR_Nickname  = this.Container.querySelector("#ERR_Nickname");
        this.Info_Nickname = this.Container.querySelector("#Info_Nickname");

        this.OK_EmailAddress     = this.Container.querySelector("#OK_EmailAddress");
        this.ERR_EmailAddress    = this.Container.querySelector("#ERR_EmailAddress");
        this.Info_EmailAddress   = this.Container.querySelector("#Info_EmailAddress");

        this.OK_Password   = this.Container.querySelector("#OK_Password");
        this.ERR_Password  = this.Container.querySelector("#ERR_Password");
        this.Info_Password = this.Container.querySelector("#Info_Password");

        this.Handle_EmailAddress = this.Container.querySelector("#Handle_EmailAddress");
        this.Handle_CityList     = this.Container.querySelector("#Handle_CityList");

    }

    AddEvents()
    {
        this.FirstNameInput.addEventListener("change", (Event) => { this.Input_FirstName(Event); });
        this.LastNameInput.addEventListener("change", (Event) => { this.Input_LastName(Event); });
        this.NicknameInput.addEventListener("change", (Event) => { this.Input_Nickname(Event); });
        this.PasswordInput.addEventListener("change", (Event) => { this.Input_Password(Event) });
        this.CountryListSelect.addEventListener("change", (Event) => { this.Select_CountryList(Event); });
        this.CityListSelect.addEventListener("change", (Event) => { this.Select_CityList(Event); });
        this.CreateAccountButton.addEventListener("click", (Event) => { this.Button_CreateAccount(Event); });
        this.EmailAddrInput.onkeyup = (Event) => { this.Input_EmailAddress(Event); };
        this.TermsLink.addEventListener("click", (Event) => { this.Link_Terms(Event) });
        this.PrivacyLink.addEventListener("click", (Event) => { this.Link_Privacy(Event) });
    }

    InitErrorCheck()
    {
        this.IsValidFirstName    = false;
        this.IsValidLastName     = false;
        this.IsValidNickname     = false;
        this.IsValidEmailAddress = false;
        this.IsValidPassword     = false;
        this.IsValidCountryList  = false;
        this.IsValidCityList     = false;
    }

    IsDataValid()
    {

        if (!this.IsValidFirstName || !this.IsValidLastName
            || !this.IsValidNickname || !this.IsValidEmailAddress
            || !this.IsValidPassword || !this.IsValidCountryList
            || !this.IsValidCityList)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    DisableFields(AState)
    {
        this.FirstNameInput.disabled    = AState;
        this.LastNameInput.disabled     = AState;
        this.NicknameInput.disabled     = AState;
        this.EmailAddrInput.disabled    = AState;
        this.PasswordInput.disabled     = AState;
        this.CountryListSelect.disabled = AState;
        this.CityListSelect.disabled    = AState;
    }

    ClearFields()
    {

        this.FirstNameInput.value = "";
        this.LastNameInput.value  = "";
        this.NicknameInput.value  = "";
        this.EmailAddrInput.value = "";
        this.PasswordInput.value  = "";

        this.CountryListSelect.selectedIndex = 0;
        this.Helpers.ClearSelectElement(this.CityListSelect);

        this.CityListSelect.selectedIndex = 0;
        this.CityListSelect.disabled = true;

        this.InitErrorCheck();

    }

    ClearNotifications()
    {

        this.OK_FirstName.style.visibility   = "hidden";
        this.ERR_FirstName.style.visibility  = "hidden";
        this.Info_FirstName.style.visibility = "hidden";

        this.OK_LastName.style.visibility   = "hidden";
        this.ERR_LastName.style.visibility  = "hidden";
        this.Info_LastName.style.visibility = "hidden";

        this.OK_Nickname.style.visibility   = "hidden";
        this.ERR_Nickname.style.visibility  = "hidden";
        this.Info_Nickname.style.visibility = "hidden";

        this.OK_EmailAddress.style.visibility   = "hidden";
        this.ERR_EmailAddress.style.visibility  = "hidden";
        this.Info_EmailAddress.style.visibility = "hidden";

        this.OK_Password.style.visibility   = "hidden";
        this.ERR_Password.style.visibility  = "hidden";
        this.Info_Password.style.visibility = "hidden";

    }

    Input_FirstName(Event)
    {

        if (this.Helpers.IsEmpty(Event.target.value))
        {
            this.OK_FirstName.style.visibility  = "hidden";
            this.ERR_FirstName.style.visibility = "visible";
            this.Info_FirstName.style.display   = "inline-block";
            this.IsValidFirstName = false;
        }
        else
        {
            this.OK_FirstName.style.visibility  = "visible";
            this.ERR_FirstName.style.visibility = "hidden";
            this.Info_FirstName.style.display   = "none";
            this.IsValidFirstName = true;
        }

    }

    Input_LastName(Event)
    {

        if (this.Helpers.IsEmpty(Event.target.value))
        {
            this.OK_LastName.style.visibility  = "hidden";
            this.ERR_LastName.style.visibility = "visible";
            this.Info_LastName.style.display   = "inline-block";
            this.IsValidLastName = false;
        }
        else
        {
            this.OK_LastName.style.visibility  = "visible";
            this.ERR_LastName.style.visibility = "hidden";
            this.Info_LastName.style.display   = "none";
            this.IsValidLastName  = true;
        }

    }

    Input_Nickname(Event)
    {

        if (this.Helpers.IsEmpty(Event.target.value))
        {
            this.OK_Nickname.style.visibility  = "hidden";
            this.ERR_Nickname.style.visibility = "visible";
            this.Info_Nickname.style.display   = "inline-block";
            this.IsValidNickname = false;
        }
        else
        {
            this.OK_Nickname.style.visibility  = "visible";
            this.ERR_Nickname.style.visibility = "hidden";
            this.Info_Nickname.style.display   = "none";
            this.IsValidNickname = true;
        }

    }

    Input_EmailAddress(Event)
    {

        let Url = encodeURI(`${window.location.origin}/api/v1/ajax/validation/${Event.target.value}/`);

        this.OK_EmailAddress.style.display   = "visibility";
        this.ERR_EmailAddress.style.display  = "visibility";
        this.Info_EmailAddress.style.display = "visibility";

        Handle_EmailAddress.classList.add("is-loading");

        if (!this.Helpers.IsEmpty(Event.target.value) && this.Helpers.ValidateEmail(Event.target.value))
        {
            this.Ajax.Execute("GET", Url, null, this.CheckEmailAddress_Callback.bind(this));
        }
        else
        {
            this.OK_EmailAddress.style.visibility  = "hidden";
            this.ERR_EmailAddress.style.visibility = "hidden";
            this.Info_EmailAddress.style.display = "inline-block";
            this.Info_EmailAddress.innerHTML     = "Valid email address is mandatory.";
            this.Handle_EmailAddress.classList.remove("is-loading");
            this.IsValidEmailAddress = false;
        }

    }

    CheckEmailAddress_Callback(Response, StatusCode)
    {

        Handle_EmailAddress.classList.remove("is-loading");

        if (StatusCode === 200)
        {

            try
            {

                let ParsedResponse = JSON.parse(Response);
                if (ParsedResponse.IsEmailValid)
                {
                    this.OK_EmailAddress.style.visibility  = "visible";
                    this.ERR_EmailAddress.style.visibility = "hidden";
                    this.Info_EmailAddress.style.display = "none";
                    this.Info_EmailAddress.innerHTML     = "";
                    this.IsValidEmailAddress = true;
                }
                else
                {
                    this.OK_EmailAddress.style.visibility  = "hidden";
                    this.ERR_EmailAddress.style.visibility = "visible";
                    this.Info_EmailAddress.style.display = "inline-block";
                    this.Info_EmailAddress.innerHTML     = ParsedResponse.Error.ErrorDesc;
                    this.IsValidEmailAddress = false;
                }

            }
            catch (Error)
            {
                this.Dialog.SetMessageType("AlertError");
                this.Dialog.SetTitle("Email Address Check");
                this.Dialog.SetContent(`An error occured during parsing JSON, error: ${Error.message}`);
                this.Dialog.Show();
                console.error(`[RegisterPage].[CheckEmailAddress_Callback]: An error has been thrown: ${Error.message}`);
            }

        }
        else
        {

            this.OK_EmailAddress.style.visibility = "hidden";
            this.ERR_EmailAddress.style.visibility = "hidden";
            this.Info_EmailAddress.style.display = "inline-block";
            this.Info_EmailAddress.innerHTML     = "An error has occured during the processing";

            this.Dialog.SetMessageType("AlertError");
            this.Dialog.SetTitle("Email Address Check");
            this.Dialog.SetContent(`An error has occured during the processing. Returned status code: ${StatusCode}`);
            this.Dialog.Show();

            this.IsValidEmailAddress = false;

        }

    }

    Input_Password(Event)
    {

        if (!this.Helpers.ValidatePassword(Event.target.value))
        {
            this.OK_Password.style.visibility  = "hidden";
            this.ERR_Password.style.visibility = "visible";
            this.Info_Password.style.display   = "inline-block";
            this.IsValidPassword = false;
        }
        else
        {
            this.OK_Password.style.visibility  = "visible";
            this.ERR_Password.style.visibility = "hidden";
            this.Info_Password.style.display   = "none";
            this.IsValidPassword = true;
        }

    }

    Select_CountryList(Event)
    {
        let Url = encodeURI(`${window.location.origin}/api/v1/ajax/cities/?countryid=${Event.target.value}`);
        this.Handle_CityList.classList.add("is-loading");
        this.Ajax.Execute("GET", Url, null, this.GetCountryList_Callback.bind(this));
    }

    GetCountryList_Callback(Response, StatusCode)
    {

        this.Handle_CityList.classList.remove("is-loading");

        if (StatusCode == 200)
        {

            try
            {

                let ParsedResponse = JSON.parse(Response);
                this.Helpers.ClearSelectElement(this.CityListSelect);

                for (let Index = 0; Index < ParsedResponse.Cities.length; Index++)
                {

                    let City = ParsedResponse.Cities[Index];
                    let Option = document.createElement("option");

                    Option.value = City.id;
                    Option.innerHTML = City.name;
                    this.CityListSelect.appendChild(Option);

                }

                this.CityListSelect.removeAttribute("disabled");
                this.CityListSelect.selectedIndex = 0;
                this.IsValidCountryList = true;

            }
            catch (Error)
            {
                this.Dialog.SetMessageType("AlertError");
                this.Dialog.SetTitle("Get Country List");
                this.Dialog.SetContent(`An error occured during parsing JSON, error: ${Error.message}`);
                this.Dialog.Show();
                console.error(`[RegisterPage].[GetCountryList_Callback]: An error has been thrown: ${Error.message}`);
            }

        }
        else
        {
            this.Dialog.SetMessageType("AlertError");
            this.Dialog.SetTitle("Email Address Check");
            this.Dialog.SetContent(`An error has occured during the processing. Returned status code: ${StatusCode}`);
            this.Dialog.Show();
            this.IsValidCountryList = false;
        }

    }

    Select_CityList(Event)
    {

        if (Event.target.value === "")
        {
            this.IsValidCityList = false;
        }
        else
        {
            this.IsValidCityList = true;
        }

    }

    async Link_Terms(Event)  
    {

        let Url = encodeURI(`${window.location.origin}/modals/terms.html`);
        let Response = await fetch(Url);
        let Content = await Response.text();

        if (Response.ok)
        {
            this.Dialog.SetMessageType("Dialog");
            this.Dialog.SetTitle("Privacy Terms");
            this.Dialog.SetContent(Content);
            this.Dialog.Show();
        }
        else
        {
            this.Dialog.SetMessageType("AlertError");
            this.Dialog.SetTitle("Privacy Terms");
            this.Dialog.SetContent(`An error has occured during the processing. Returned status code: ${StatusCode}`);
            this.Dialog.Show();
        }

    }

    async Link_Privacy(Event)
    {

        let Url = encodeURI(`${window.location.origin}/modals/privacy.html`);
        let Response = await fetch(Url);
        let Content = await Response.text();

        if (Response.ok)
        {
            this.Dialog.SetMessageType("Dialog");
            this.Dialog.SetTitle("Privacy Policy");
            this.Dialog.SetContent(Content);
            this.Dialog.Show();
        }
        else
        {
            this.Dialog.SetMessageType("AlertError");
            this.Dialog.SetTitle("Privacy Policy");
            this.Dialog.SetContent(`An error has occured during the processing. Returned status code: ${StatusCode}`);
            this.Dialog.Show();
        }

    }

    async Button_CreateAccount(Event)
    {

        if (!this.IsDataValid())
        {

            this.Dialog.SetMessageType("AlertWarning");
            this.Dialog.SetTitle("Cannot create an account");
            this.Dialog.SetContent("The account cannot be created. Please make sure that all the fields are filled and valid email address is provided.");
            this.Dialog.Show();

            return false;

        }

        let SerializedPayLoad = JSON.stringify(
        {
            FirstName:    this.FirstNameInput.value,
            LastName:     this.LastNameInput.value,
            NickName:     this.NicknameInput.value,
            EmailAddress: this.EmailAddrInput.value,
            Password:     this.PasswordInput.value,
            CountryId:    Number(this.CountryListSelect.value),
            CityId:       Number(this.CityListSelect.value)
        });

        this.ClearFields();
        this.ClearNotifications()
        this.DisableFields(true);
        this.CreateAccountHandle.classList.add("is-loading");
        this.CreateAccountButton.disabled = true;

        let Url = encodeURI(`${window.location.origin}/api/v1/ajax/users/signup/`);
        this.Ajax.Execute("POST", Url, SerializedPayLoad, this.CreateAccount_Callback.bind(this));

        return true;

    }

    async CreateAccount_Callback(Response, StatusCode)
    {

        this.DisableFields(false);
        this.CreateAccountButton.disabled = false;
        this.CreateAccountHandle.classList.remove("is-loading");

        if (StatusCode === 200)
        {

            try
            {

                let ParsedResponse = JSON.parse(Response);
                if (ParsedResponse.IsUserCreated)
                {

                    /* This is demo application and we only display message to the user.
                     * However, in production, one may want to redirect user to another page. */

                    this.Dialog.SetMessageType("AlertSuccess");
                    this.Dialog.SetTitle("An account has been created");
                    this.Dialog.SetContent("Your account has been created. Please check your email box and follow the instructions to activate the account.");
                    this.Dialog.Show();

                }
                else
                {
                    this.Dialog.SetMessageType("AlertError");
                    this.Dialog.SetTitle("An account cannot be created");
                    this.Dialog.SetContent(`The account could not be created. Please contact IT support if problem persists. Description: ${ParsedResponse.Error.ErrorDesc}`);
                    this.Dialog.Show();
                }

            }
            catch (Error)
            {
                this.Dialog.SetMessageType("AlertError");
                this.Dialog.SetTitle("Create Account");
                this.Dialog.SetContent(`An error occured during parsing JSON, error: ${Error.message}`);
                this.Dialog.Show();
                console.error(`[RegisterPage].[CreateAccount_Callback]: An error has been thrown: ${Error.message}`);
            }

        }
        else
        {
            this.Dialog.SetMessageType("AlertError");
            this.Dialog.SetTitle("Create Account");
            this.Dialog.SetContent(`An error has occured during the processing. Returned status code: ${StatusCode}`);
            this.Dialog.Show();
        }

    }

}
