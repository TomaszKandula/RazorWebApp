// Startup module to cache DOM and bind events

"use strict"


import * as _login    from "./views/login";
import * as _register from "./views/register";


document.addEventListener('DOMContentLoaded', () =>
{

    const LoginPage    = document.getElementById("LoginForm");
    const RegisterPage = document.getElementById("RegisterForm");

    if (LoginPage)    { const LoginView    = new _login.LoginClass(LoginPage); }
    if (RegisterPage) { const RegisterView = new _register.RegisterClass(RegisterPage); }

});
