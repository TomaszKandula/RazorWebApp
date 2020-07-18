// View module to manipulate Virtual DOM

"use strict";


import Helpers    from "../functions/Helpers";
import RestClient from "../functions/RestClient";


export default class RegisterPage
{

    constructor(Container)
    {
        this.Container = Container;
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

        this.Ajax    = new RestClient(this.Container.dataset.xsrf, "application/json; charset=UTF-8");
        this.Helpers = new Helpers();

    }

    BindDom()
    {
        this.FirstNameInput      = this.Container.querySelector("#Input_FirstName");
        this.LastNameInput       = this.Container.querySelector("#Input_LastName");
        this.NicknameInput       = this.Container.querySelector("#Input_Nickname");
        this.EmailAddressInput   = this.Container.querySelector("#Input_EmailAddress");
        this.PasswordInput       = this.Container.querySelector("#Input_Password");
        this.CountryListSelect   = this.Container.querySelector("#Select_CountryList");
        this.CityListSelect      = this.Container.querySelector("#Select_CityList");
        this.CreateAccountButton = this.Container.querySelector("#Button_CreateAccount");
        this.ModalWindowHandle   = this.Container.querySelector("#Handle_RegisterModal");
        this.TermsLink           = this.Container.querySelector("#Link_Terms");
        this.PrivacyLink         = this.Container.querySelector("#Link_Privacy");
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
        this.EmailAddressInput.onkeyup = () => { this.Input_EmailAddress(); };
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

    Input_FirstName(Event)
    {

        let Verified  = this.Container.querySelector("#OK_FirstName");
        let Malformed = this.Container.querySelector("#ERR_FirstName");
        let Info      = this.Container.querySelector("#Info_FirstName");

        if (this.Helpers.IsEmpty(Event.target.value))
        {
            Verified.style.visibility  = "hidden";
            Malformed.style.visibility = "visible";
            Info.style.display         = "inline-block";
            this.IsValidFirstName      = false;
        }
        else
        {
            Verified.style.visibility  = "visible";
            Malformed.style.visibility = "hidden";
            Info.style.display         = "none";
            this.IsValidFirstName      = true;
        }

    }


    Input_LastName(Event)
    {

        let Verified  = this.Container.querySelector("#OK_LastName");
        let Malformed = this.Container.querySelector("#ERR_LastName");
        let Info      = this.Container.querySelector("#Info_LastName");

        if (this.Helpers.IsEmpty(Event.target.value))
        {
            Verified.style.visibility  = "hidden";
            Malformed.style.visibility = "visible";
            Info.style.display         = "inline-block";
            this.IsValidLastName       = false;
        }
        else
        {
            Verified.style.visibility  = "visible";
            Malformed.style.visibility = "hidden";
            Info.style.display         = "none";
            this.IsValidLastName       = true;
        }

    }


    Input_Nickname(Event)
    {

        let Verified  = this.Container.querySelector("#OK_Nickname");
        let Malformed = this.Container.querySelector("#ERR_Nickname");
        let Info      = this.Container.querySelector("#Info_Nickname");

        if (this.Helpers.IsEmpty(Event.target.value))
        {
            Verified.style.visibility  = "hidden";
            Malformed.style.visibility = "visible";
            Info.style.display         = "inline-block";
            this.IsValidNickname       = false;
        }
        else
        {
            Verified.style.visibility  = "visible";
            Malformed.style.visibility = "hidden";
            Info.style.display         = "none";
            this.IsValidNickname       = true;
        }

    }


    Input_EmailAddress()
    {

        let Handler      = this.Container.querySelector("#Handle_EmailAddress");
        let Verified     = this.Container.querySelector("#OK_EmailAddress");
        let Malformed    = this.Container.querySelector("#ERR_EmailAddress");
        let Info         = this.Container.querySelector("#Info_EmailAddress");

        let EmailAddress = this.EmailAddressInput.value;
        let Url          = encodeURI(window.location.origin + "/api/v1/ajax/validation/" + EmailAddress + "/");

        Verified.style.display  = "visibility";
        Malformed.style.display = "visibility";
        Info.style.display      = "visibility";

        Handler.classList.add("is-loading");

        if (!this.Helpers.IsEmpty(EmailAddress) && this.Helpers.ValidateEmail(EmailAddress))
        {
            this.Ajax.Execute("GET", Url, null, this.CheckEmailAddress_Callback.bind(this));
        }
        else
        {
            Verified.style.visibility  = "hidden";
            Malformed.style.visibility = "hidden";
            Info.style.display = "inline-block";
            Info.innerHTML     = "Valid email address is mandatory.";
            Handler.classList.remove("is-loading");
            this.IsValidEmailAddress = false;
        }

    }


    CheckEmailAddress_Callback(Response, StatusCode)
    {

        let Handler   = this.Container.querySelector("#Handle_EmailAddress");
        let Verified  = this.Container.querySelector("#OK_EmailAddress");
        let Malformed = this.Container.querySelector("#ERR_EmailAddress");
        let Info      = this.Container.querySelector("#Info_EmailAddress");

        Handler.classList.remove("is-loading");

        if (StatusCode === 200)
        {

            let ParsedResponse = JSON.parse(Response); 
            if (ParsedResponse.IsEmailValid)
            {
                Verified.style.visibility  = "visible";
                Malformed.style.visibility = "hidden";
                Info.style.display         = "none";
                Info.innerHTML             = "";
                this.IsValidEmailAddress   = true;
            }
            else
            {
                Verified.style.visibility  = "hidden";
                Malformed.style.visibility = "visible";
                Info.style.display         = "inline-block";
                Info.innerHTML             = ParsedResponse.Error.ErrorDesc;
                this.IsValidEmailAddress   = false;
            }

        }
        else
        {
            Verified.style.visibility  = "hidden";
            Malformed.style.visibility = "hidden";
            Info.style.display = "inline-block";
            Info.innerHTML     = "An error has occured during the processing";
            alert("An error has occured during the processing. Returned status code: " + StatusCode + ".");
            this.IsValidEmailAddress = false;
        }

    }


    Input_Password(Event)
    {

        let Verified  = this.Container.querySelector("#OK_Password");
        let Malformed = this.Container.querySelector("#ERR_Password");
        let Info      = this.Container.querySelector("#Info_Password");

        if (!this.Helpers.ValidatePasswordField(Event.target.value))
        {
            Verified.style.visibility  = "hidden";
            Malformed.style.visibility = "visible";
            Info.style.display         = "inline-block";
            this.IsValidPassword       = false;
        }
        else
        {
            Verified.style.visibility  = "visible";
            Malformed.style.visibility = "hidden";
            Info.style.display         = "none";
            this.IsValidPassword       = true;
        }

    }


    Select_CountryList(Event)
    {

        let Handler    = this.Container.querySelector("#Handle_CityList");
        let SelectedId = Event.target.value;
        let Url        = encodeURI(window.location.origin + "/api/v1/ajax/cities/" + SelectedId + "/");

        Handler.classList.add("is-loading");
        this.Ajax.Execute("GET", Url, null, this.GetCountryList_Callback.bind(this));

    }


    GetCountryList_Callback(Response, StatusCode)
    {

        let Handler  = this.Container.querySelector("#Handle_CityList");

        Handler.classList.remove("is-loading");

        if (StatusCode == 200)
        {

            let ParsedResponse = JSON.parse(Response); 
            this.Helpers.ClearSelectElement(this.CityListSelect);

            for (let Index = 0; Index < ParsedResponse.Cities.length; Index++)
            {

                let City   = ParsedResponse.Cities[Index];
                let Option = document.createElement("option");

                Option.value = City.id;
                Option.innerHTML = City.name;
                this.CityListSelect.appendChild(Option);

            }

            this.CityListSelect.removeAttribute("disabled");
            this.CityListSelect.selectedIndex = 0;
            this.IsValidCountryList = true;

        }
        else
        {
            alert("An error has occured during the processing. Returned status code: " + StatusCode + ".");
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


    async Link_Terms(Event) //refactor!
    {

        let Url = encodeURI(window.location.origin + "/modals/terms.html");
        let Response = await fetch(Url);
        let Content = await Response.text();

        if (Response.ok)
        {

            this.ModalWindowHandle.innerHTML = Content;
            this.ModalWindowHandle.classList.add("is-active");

            let Button_CloseTerms = this.Container.querySelector("#Close_TermsModal");
            Button_CloseTerms.addEventListener("click", () =>
            {
                this.ModalWindowHandle.classList.remove("is-active");
            });

        }
        else
        {
            alert("An error has occured during the processing. Response: " + Response.status);
        }

    }


    async Link_Privacy(Event) //refactor!
    {

        let Url = encodeURI(window.location.origin + "/modals/privacy.html");
        let Response = await fetch(Url);
        let Content = await Response.text();

        if (Response.ok)
        {

            this.ModalWindowHandle.innerHTML = Content;
            this.ModalWindowHandle.classList.add("is-active");

            let Button_ClosePrivacy = this.Container.querySelector("#Close_PrivacyModal");
            Button_ClosePrivacy.addEventListener("click", () =>
            {
                this.ModalWindowHandle.classList.remove("is-active");
            });

        }
        else
        {
            alert("An error has occured during the processing. Response: " + Response.status);
        }

    }


    async Button_CreateAccount(Event) //refactor!
    {

        if (!this.IsDataValid())
        {

            let Url = encodeURI(window.location.origin + "/modals/ErrorCreateAccount.html");
            let Response = await fetch(Url);
            let Content = await Response.text();

            if (Response.ok)
            {

                this.ModalWindowHandle.innerHTML = Content;
                this.ModalWindowHandle.classList.add("is-active");

                let Button_CloseError = this.Container.querySelector("#Close_ErrorCreateAccount");
                Button_CloseError.addEventListener("click", () =>
                {
                    this.ModalWindowHandle.classList.remove("is-active");
                });

            }
            else
            {
                alert("An error has occured during the processing. Response: " + Response.status);
            }

            return false;

        }

        let SerializedPayLoad = JSON.stringify(
        {
            FirstName:    this.FirstNameInput.value,
            LastName:     this.LastNameInput.value,
            NickName:     this.NicknameInput.value,
            EmailAddress: this.EmailAddressInput.value,
            Password:     this.PasswordInput.value,
            CountryId:    Number(this.CountryListSelect.value),
            CityId:       Number(this.CityListSelect.value)
        });

        let Url = encodeURI(window.location.origin + "/api/v1/ajax/users/");
        this.Ajax.Execute("POST", Url, SerializedPayLoad, this.CreateAccount_Callback.bind(this));

        return true;

    }


    async CreateAccount_Callback(Response, StatusCode) //refactor!
    {

        if (StatusCode === 200)
        {

            let ParsedResponse = JSON.parse(Response);

            if (ParsedResponse.IsUserCreated)
            {

                let Url = encodeURI(window.location.origin + "/modals/SuccessCreateAccount.html");
                let Response = await fetch(Url);
                let Content = await Response.text();

                if (Response.ok)
                {

                    this.ModalWindowHandle.innerHTML = Content;
                    this.ModalWindowHandle.classList.add("is-active");

                    let Button_CloseError = this.Container.querySelector("#Close_SuccessCreateAccount");
                    Button_CloseError.addEventListener("click", () =>
                    {
                        this.ModalWindowHandle.classList.remove("is-active");
                    });

                }
                else
                {
                    alert("An error has occured during the processing. Response: " + Response.status);
                }

            }
            else
            {
                alert("An error has occured during the processing. Error: " + ParsedResponse.Error.ErrorDesc);
            }

        }
        else
        {
            alert("An error has occured during the processing. Returned status code: " + StatusCode + ".");
        }

    }

}
