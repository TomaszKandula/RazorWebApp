// View module to manipulate Virtual DOM

"use strict";

import Helpers      from "../functions/helpers";
import RestClient   from "../functions/restClient";
import Cookies      from "../functions/cookies";
import MessageBox   from "../components/messageBox";
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

        this.Dialog  = new MessageBox(this.ModalWindowHandle);
        this.Ajax    = new RestClient(this.Container.dataset.xsrf, "application/json; charset=UTF-8");
        this.Cookies = new Cookies();
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

        this.EmailAddrInput = this.Container.querySelector("#Input_EmailAddress");
        this.PasswordInput  = this.Container.querySelector("#Input_Password");
        this.SigninHandle   = this.Container.querySelector("#Handle_Signin");
        this.SigninButton   = this.Container.querySelector("#Button_Signin");

        this.OK_EmailAddress  = this.Container.querySelector("#OK_EmailAddress");
        this.ERR_EmailAddress = this.Container.querySelector("#ERR_EmailAddress");

        this.OK_Password  = this.Container.querySelector("#OK_Password");
        this.ERR_Password = this.Container.querySelector("#ERR_Password");

        this.ModalWindowHandle = this.Container.querySelector("#Handle_Modal");

    }

    AddEvents()
    {
        this.SigninButton.addEventListener("click", (Event) => { this.Button_Signin(Event); });
        this.EmailAddrInput.addEventListener("change", (Event) => { this.Input_EmailAddress(Event); });
        this.PasswordInput.addEventListener("change", (Event) => { this.Input_Password(Event); });
    }

    InitErrorCheck()
    {
        this.IsValidEmailAddr = false;
        this.IsValidPassword  = false;
    }

    IsDataValid()
    {

        if (!this.IsValidEmailAddr || !this.IsValidPassword)
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
        this.EmailAddrInput.disabled = AState;
        this.PasswordInput.disabled  = AState;
    }

    ClearFields()
    {

        this.EmailAddrInput.value = "";
        this.PasswordInput.value  = "";
        this.InitErrorCheck();

        this.OK_EmailAddress.style.visibility  = "hidden";
        this.ERR_EmailAddress.style.visibility = "hidden";

        this.OK_Password.style.visibility  = "hidden";
        this.ERR_Password.style.visibility = "hidden";

    }

    Input_EmailAddress(Event)
    {

        this.OK_EmailAddress.style.display = "visibility";
        this.ERR_EmailAddress.style.display = "visibility";

        if (!this.Helpers.IsEmpty(Event.target.value) && this.Helpers.ValidateEmail(Event.target.value))
        {
            this.OK_EmailAddress.style.visibility  = "visible";
            this.ERR_EmailAddress.style.visibility = "hidden";
            this.IsValidEmailAddr = true;
        }
        else
        {
            this.OK_EmailAddress.style.visibility  = "hidden";
            this.ERR_EmailAddress.style.visibility = "visible";
            this.IsValidEmailAddr = false;
        }

    }

    Input_Password(Event)
    {

        if (this.Helpers.IsEmpty(Event.target.value))
        {
            this.OK_Password.style.visibility  = "hidden";
            this.ERR_Password.style.visibility = "visible";
            this.IsValidPassword = false;
        }
        else
        {
            this.OK_Password.style.visibility  = "visible";
            this.ERR_Password.style.visibility = "hidden";
            this.IsValidPassword = true;
        }

    }

    Button_Signin(Event)
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
        this.Ajax.Execute("POST", Url, SerializedPayLoad, this.Signin_Callback.bind(this));

        return true;

    }

    async Signin_Callback(Response, StatusCode)
    {

        this.DisableFields(false);
        this.SigninHandle.classList.remove("is-loading");
        this.SigninButton.disabled = false;

        if (StatusCode === 200)
        {

            try
            {

                let ParsedResponse = JSON.parse(Response);
                if (ParsedResponse.IsLogged)
                {

                    /* 
                     * We set cookie that tells our UI that user has been logged, and some
                     * parts of the UI may be changed on that occasion. This does not have 
                     * to be protected as it does not affect signin or session itself.
                     * This is UI related cookie only.
                     */

                    this.Cookies.SetCookie("user_session", "alive", 0.11, "Strict", null);

                    /* 
                     * This is demo application and we only redirect to the main page.
                     * However, in real application, one may want to redirect user to another page. 
                     * Please also note: the page that user is redirected to must add HttpOnly cookie
                     * to the response header. 
                     */

                    window.location.replace(`${window.location.origin}/index`);

                }
                else
                {
                    this.Dialog.SetMessageType("AlertError");
                    this.Dialog.SetTitle("Login to an account");
                    this.Dialog.SetContent(`Cannot login to the account. ${ParsedResponse.Error.ErrorDesc}`);
                    this.Dialog.Show();
                }

            }
            catch (Error)
            {
                this.Dialog.SetMessageType("AlertError");
                this.Dialog.SetTitle("Login to an account");
                this.Dialog.SetContent(`An error occured during parsing JSON, error: ${Error.message}`);
                this.Dialog.Show();
                console.error(`[LoginPage].[Signin_Callback]: An error has been thrown: ${Error.message}`);
            }

        }
        else
        {
            this.Dialog.SetMessageType("AlertError");
            this.Dialog.SetTitle("Login to an account");
            this.Dialog.SetContent(`An error has occured during the processing. Returned status code: ${StatusCode}`);
            this.Dialog.Show();
        }

    }

}
