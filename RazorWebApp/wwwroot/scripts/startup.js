// Startup module to cache DOM and bind events
"use strict";

import IndexPage from "./pages";
import LoginPage from "./pages/login";
import LogoutPage from "./pages/logout";
import RegisterPage from "./pages/Register";
import ErrorPage from "./pages/Error";

document.addEventListener('DOMContentLoaded', () =>
{
    const HNavButtons = document.querySelector("#Handle_Buttons");

    const HSectionIndex = document.querySelector("#IndexPage");
    const HSectionLogin = document.querySelector("#LoginPage");
    const HSectionRegister = document.querySelector("#RegisterPage");
    const HSectionLogout = document.querySelector("#LogoutPage");
    const HSectionError = document.querySelector("#ErrorPage");

    const IndexInstance = new IndexPage(HSectionIndex, HNavButtons);
    const LoginInstance = new LoginPage(HSectionLogin, HNavButtons);
    const RegisterInstance = new RegisterPage(HSectionRegister, HNavButtons);
    const LogoutInstance = new LogoutPage(HSectionLogout);
    const ErrorInstance = new ErrorPage(HSectionError);

    IndexInstance.Initialize();
    LoginInstance.Initialize();
    RegisterInstance.Initialize();
    LogoutInstance.Initialize();
    ErrorInstance.Initialize();
});
