// View module to manipulate Virtual DOM
"use strict";

import Helpers from "../functions/helpers";
import RestClient from "../functions/restClient";
import Cookies from "../functions/cookies";
import MessageBox from "../components/messageBox";
import LoginButtons from "../components/loginButtons";

export default class LoginPage
{
    constructor(AContainer, ANavButtons)
    {
        this.Container  = AContainer;
        this.NavButtons = ANavButtons;
    } 

    Initialize()
    {
        if (this.Container === null) return null;

        this.BindDom();
        this.AddEvents();
        this.InitErrorCheck();

        this.Dialog = new MessageBox(this.ModalWindowHandle);
        this.Ajax = new RestClient(this.Container.dataset.xsrf, "application/json; charset=UTF-8");
        this.Cookies = new Cookies();
        this.Helpers = new Helpers();

        this.RenderButtons();
    }

    RenderButtons()
    {
        const MoveToRegisterPage = function () 
        { 
            window.location.replace(`${window.location.origin}/register`) 
        };
        
        const MoveToLoginPage = function () 
        { 
            window.location.replace(`${window.location.origin}/login`) 
        };

        this.LoginButtons = new LoginButtons(this.NavButtons, "Signup_Login", MoveToRegisterPage, MoveToLoginPage, null);
        this.LoginButtons.Show();
    }

    BindDom()
    {
        this.EmailAddrInput = this.Container.querySelector("#Input_EmailAddress");
        this.PasswordInput = this.Container.querySelector("#Input_Password");
        this.SigninHandle = this.Container.querySelector("#Handle_Signin");
        this.SigninButton = this.Container.querySelector("#Button_Signin");
        this.OK_EmailAddress = this.Container.querySelector("#OK_EmailAddress");
        this.ERR_EmailAddress = this.Container.querySelector("#ERR_EmailAddress");
        this.OK_Password = this.Container.querySelector("#OK_Password");
        this.ERR_Password = this.Container.querySelector("#ERR_Password");
        this.ModalWindowHandle = this.Container.querySelector("#Handle_Modal");
    }

    AddEvents()
    {
        this.SigninButton.addEventListener("click", (Event) => { this.ButtonSignin(Event); });
        this.EmailAddrInput.addEventListener("change", (Event) => { this.InputEmailAddress(Event); });
        this.PasswordInput.addEventListener("change", (Event) => { this.InputPassword(Event); });
    }

    InitErrorCheck()
    {
        this.IsValidEmailAddr = false;
        this.IsValidPassword = false;
    }

    IsDataValid()
    {
        if (!this.IsValidEmailAddr || !this.IsValidPassword) 
            return false;
        
        return true;
    }

    DisableFields(AState)
    {
        this.EmailAddrInput.disabled = AState;
        this.PasswordInput.disabled = AState;
    }

    ClearFields()
    {
        this.EmailAddrInput.value = "";
        this.PasswordInput.value = "";
        this.InitErrorCheck();

        this.OK_EmailAddress.style.visibility = "hidden";
        this.ERR_EmailAddress.style.visibility = "hidden";
        this.OK_Password.style.visibility = "hidden";
        this.ERR_Password.style.visibility = "hidden";
    }

    InputEmailAddress(Event)
    {
        this.OK_EmailAddress.style.display = "visibility";
        this.ERR_EmailAddress.style.display = "visibility";

        if (!this.Helpers.IsEmpty(Event.target.value) && this.Helpers.ValidateEmail(Event.target.value))
        {
            this.OK_EmailAddress.style.visibility = "visible";
            this.ERR_EmailAddress.style.visibility = "hidden";
            this.IsValidEmailAddr = true;
            return void 0;
        }

        this.OK_EmailAddress.style.visibility = "hidden";
        this.ERR_EmailAddress.style.visibility = "visible";
        this.IsValidEmailAddr = false;
    }

    InputPassword(Event)
    {
        if (this.Helpers.IsEmpty(Event.target.value))
        {
            this.OK_Password.style.visibility = "hidden";
            this.ERR_Password.style.visibility = "visible";
            this.IsValidPassword = false;
            return void 0;
        }

        this.OK_Password.style.visibility = "visible";
        this.ERR_Password.style.visibility = "hidden";
        this.IsValidPassword = true;
    }

    ButtonSignin(Event)
    {
        if (!this.IsDataValid())
        {
            this.Dialog.SetMessageType("AlertWarning");
            this.Dialog.SetTitle("Login to an account");
            this.Dialog.SetContent("Cannot login to an account. Please provide valid email address and password.");
            this.Dialog.Show();
            return false;
        }

        let SerializedPayLoad = JSON.stringify(
        {
            EmailAddr: this.EmailAddrInput.value,
            Password:  this.PasswordInput.value
        });

        this.ClearFields();
        this.DisableFields(true);
        this.SigninHandle.classList.add("is-loading");
        this.SigninButton.disabled = true;

        let Url = encodeURI(`${window.location.origin}/api/v1/ajax/users/signin/`);
        this.Ajax.Execute("POST", Url, SerializedPayLoad, this.SigninCallback.bind(this));

        return true;
    }

    async SigninCallback(Response, StatusCode)
    {
        this.DisableFields(false);
        this.SigninHandle.classList.remove("is-loading");
        this.SigninButton.disabled = false;
        if (StatusCode === 204)
        {
            /* 
            * We set a cookie to tell our UI that the user has been logged, 
            * so specific parts of the UI may be updated. This cookie does not have to be
            * protected as it does not affect sign in or the session itself. 
            * It is UI related cookie only because we do not have state management in this example.
            */
            this.Cookies.SetCookie("user_session", "alive", 0.11, "Strict", null);

            /* 
             * It is a demo application and we only redirect to the main page.
             * However, in an actual application, one may want to redirect the user to another page.
             * Please note: HttpOnly cookie must be present in the response header.
             */
            window.location.replace(`${window.location.origin}/index`);
        }
        else if (StatusCode === 400)
        {
            const ParsedResponse = JSON.parse(Response);
            this.Dialog.SetMessageType("AlertError");
            this.Dialog.SetTitle("Login to an account");
            this.Dialog.SetContent(`${ParsedResponse.ErrorDesc}.`);
            this.Dialog.Show();
        }
        else
        {
            const ParsedResponse = JSON.parse(Response);
            this.Dialog.SetMessageType("AlertError");
            this.Dialog.SetTitle("Login to an account");
            this.Dialog.SetContent(`An error has occured during the processing. Returned status code: ${StatusCode}. Description: ${ParsedResponse.ErrorDesc}.`);
            this.Dialog.Show();
        }
    }
}
