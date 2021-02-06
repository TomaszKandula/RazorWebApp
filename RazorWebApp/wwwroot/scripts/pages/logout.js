// View module to manipulate Virtual DOM
"use strict";

import Cookies from "../functions/cookies";

export default class LogoutPage
{
    constructor(AContainer)
    {
        this.LContainer = AContainer;
    }

    Initialize()
    {
        if (this.LContainer === null) return null;
        this.Cookies = new Cookies();
        this.Cookies.EraseCookie("user_session");
    }
}
