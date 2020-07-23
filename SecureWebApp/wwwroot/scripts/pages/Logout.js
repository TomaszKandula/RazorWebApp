// View module to manipulate Virtual DOM

"use strict";

import Cookies from "../functions/Cookies";

export default class LogoutPage
{

    constructor(AContainer)
    {
        this.LContainer = AContainer;
    }

    Initialize()
    {

        if (this.LContainer === null)
        {
            return null;
        }

        this.Cookies = new Cookies();
        Cookies.EraseCookie("user_session");

    }

}
