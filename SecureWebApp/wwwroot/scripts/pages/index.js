// View module to manipulate Virtual DOM

"use strict";


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

        this.MoveToRegister = function () { window.location.replace(`${window.location.origin}/register`) };
        this.MoveToLogin    = function () { window.location.replace(`${window.location.origin}/login`) };
        this.LogoutUser     = function () { window.location.replace(`${window.location.origin}/logout`) };

        if (this.Container.dataset.logout === "False")
        {
            this.LoginButtons = new LoginButtons(this.NavButtons, "Signup_Login", this.MoveToRegister, this.MoveToLogin, null);
            this.LoginButtons.Show();
        }
        else
        { 
            this.LoginButtons = new LoginButtons(this.NavButtons, "Logout", null, null, this.LogoutUser);
            this.LoginButtons.Show();
        }

    }

    BindDom()
    {
        this.ModalWindowHandle = this.Container.querySelector("#Handle_Modal");
    }

}
