// This module should not manipulate DOM/Virtual DOM

"use strict";


import Helpers from "../functions/Helpers";


export default class Cookies
{

    constructor()
    {
        this.Helpers = new Helpers();
    }

    SetCookie(ACookieName, AValue, ADays, ASameSite, ASecure)
    {

        let LDate = new Date();
        if (ADays)
        {

            // We set time in miliseconds
            LDate.setTime(
                LDate.getTime() + (ADays * 24 * 60 * 60 * 1000)
            );

            let LSecure = !this.Helpers.IsEmpty(ASecure) ? `; ${ASecure}` : "";
            let LNewCookie = `${ACookieName}=${AValue}; expires=${LDate.toUTCString()}; path=/; SameSite=${ASameSite} ${LSecure}`;
            document.cookie = LNewCookie;

        }

    }

    GetCookie(ACookieName)
    {

        let LCookieName = `${ACookieName}=`;
        let LCookieArray = document.cookie.split(";");

        for (let Index = 0; Index < LCookieArray.length; Index++)
        {

            let LCookie = LCookieArray[Index];

            while (LCookie.charAt(0) === " ")
            {
                LCookie = LCookie.substring(1, LCookie.length);
            }

            if (LCookie.indexOf(LCookieName) === 0)
            {
                return LCookie.substring(LCookieName.length, LCookie.length);
            }

        }

        return null;

    }

    EraseCookie(ACookieName)
    {
        document.cookie = `${ACookieName}=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;`;
    }

}
