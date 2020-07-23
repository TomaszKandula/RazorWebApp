// View module to manipulate Virtual DOM

"use strict";


import MessageBox   from "../components/MessageBox";
import LoginButtons from "../components/LoginButtons";


export default class IndexPage
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

        this.Dialog = new MessageBox(this.ModalWindowHandle);

        this.MoveToRegister = function () { window.location.replace(`${window.location.origin}/register`) };
        this.MoveToLogin    = function () { window.location.replace(`${window.location.origin}/login`) };
        this.LogoutUser     = function () { console.log("Logout!"); };

        if (this.Container.dataset.logout === "False")
        {
            this.Render_Signup_Login();
        }
        else
        { 
            this.Render_Logout();
        }

    }

    BindDom()
    {
        this.ModalWindowHandle = this.Container.querySelector("#Handle_Modal");
    }

    Render_Logout()
    {
        this.LoginButtons = new LoginButtons(this.NavButtons, "Logout", null, null, this.LogoutUser);
        this.LoginButtons.Show();
    }

    Render_Signup_Login()
    {
        this.LoginButtons = new LoginButtons(this.NavButtons, "Signup_Login", this.MoveToRegister, this.MoveToLogin, null);
        this.LoginButtons.Show();
    }

}
