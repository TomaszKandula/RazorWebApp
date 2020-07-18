// Startup module to cache DOM and bind events

"use strict";


import IndexPage    from "./pages/Index";
import LoginPage    from "./pages/Login";
import RegisterPage from "./pages/Register";
import ErrorPage    from "./pages/Error";


document.addEventListener('DOMContentLoaded', () =>
{

    // Pages
    const SectionIndex    = document.querySelector("#IndexPage");
    const SectionLogin    = document.querySelector("#LoginPage");
    const SectionRegister = document.querySelector("#RegisterPage");
    const SectionError    = document.querySelector("#ErrorPage");

    const IndexInstance    = new IndexPage(SectionIndex);
    const LoginInstance    = new LoginPage(SectionLogin);
    const RegisterInstance = new RegisterPage(SectionRegister);
    const ErrorInstance    = new ErrorPage(SectionError);

    IndexInstance.Initialize();
    LoginInstance.Initialize();
    RegisterInstance.Initialize();
    ErrorInstance.Initialize();

    // Modals


});
