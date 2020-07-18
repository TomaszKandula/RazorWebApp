// Startup module to cache DOM and bind events

"use strict";


import { IndexPage }    from "./pages/index";
import { LoginPage }    from "./pages/login";
import { RegisterPage } from "./pages/register";
import { ErrorPage }    from "./pages/error";


document.addEventListener('DOMContentLoaded', () =>
{

    // Pages
    const IndexForm    = document.querySelector("#IndexForm");
    const LoginForm    = document.querySelector("#LoginForm");
    const RegisterForm = document.querySelector("#RegisterForm");
    const ErrorForm    = document.querySelector("#ErrorForm");

    if (IndexForm)    { const indexPage    = new IndexPage(IndexForm); }
    if (LoginForm)    { const loginPage    = new LoginPage(LoginForm); }
    if (RegisterForm) { const registerPage = new RegisterPage(RegisterForm); }
    if (ErrorForm)    { const errorPage    = new ErrorPage(ErrorForm); }

    // Modals


});
