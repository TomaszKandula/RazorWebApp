// LoginButtons component

"use strict";


export default class LoginButtons
{

    constructor(AButtonHandle, ACombination, ASignupAction, ASigninAction, ALogoutAction)
    {
        this.LButtonHandle = AButtonHandle;
        this.LCombination  = ACombination;
        this.LSignupAction = ASignupAction;
        this.LSigninAction = ASigninAction;
        this.LLogoutAction = ALogoutAction;
    }

    Show()
    {
        this.Render();
        this.BindDom();
        this.AddEvents();
    }

    BindDom()
    {
        this.Handle_Signup = this.LButtonHandle.querySelector("#Handle_Signup");
        this.Handle_Login  = this.LButtonHandle.querySelector("#Handle_Login");
        this.Handle_Logout = this.LButtonHandle.querySelector("#Handle_Logout");
    }

    AddEvents()
    {
        if (this.Handle_Signup) this.Handle_Signup.addEventListener("click", () => { this.LSignupAction(); });
        if (this.Handle_Login)  this.Handle_Login.addEventListener("click", () => { this.LSigninAction(); });
        if (this.Handle_Logout) this.Handle_Logout.addEventListener("click", () => { this.LLogoutAction(); });
    }

    Render_Signup_Login()
    {
        return this.LButtonHandle.innerHTML = 
            `<div id="Handle_Signup" class="button is-info">
                <strong>Sign up</strong>
            </div>
            <div id="Handle_Login" class="button is-light">
                Log in
            </div>`;
    }

    Render_Logout()
    {
        return this.LButtonHandle.innerHTML =
            `<div id="Handle_Logout" class="button is-light">
                Log out
            </div>`;
    }

    Render()
    {

        switch (this.LCombination)
        {

            case "Signup_Login":
                this.Render_Signup_Login();
                break;

            case "Logout":
                this.Render_Logout();
                break;

            default:
                this.Render_Signup_Login();

        }

    }

}
