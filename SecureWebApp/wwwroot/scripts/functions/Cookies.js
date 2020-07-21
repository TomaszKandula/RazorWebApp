// This module should not manipulate DOM/Virtual DOM

"use strict";


export default class Cookies
{

    SetCookie(AName, AValue, ADays, ASameSite)
    {

        let LDate = new Date();
        if (ADays)
        {

            LDate.setTime(
                LDate.getTime() + (ADays * 24 * 60 * 60 * 1000)
            );

            document.cookie = `${AName}=${AValue}; expires=${LDate.toUTCString()}"; path=/; SameSite=${ASameSite}`;

        }

    }

    GetCookie(AName)
    {

        let LCookieName = `${AName}=`;
        let LCookieArray = document.cookie.split(";");

        for (let Index = 0; Index < LCookieArray.length; Index++)
        {

            let LCookie = LCookieArray[Index];

            while (LCookie.charAt(0) == " ")
            {
                LCookie = LCookie.substring(1, LCookie.length);
            }

            if (LCookie.indexOf(LCookieName) == 0)
            {
                return LCookie.substring(LCookieName.length, LCookie.length);
            }

        }

        return null;

    }

    EraseCookie(AName)
    {
        document.cookie = `${AName}=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;`;
    }

}
