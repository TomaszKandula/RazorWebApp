// This module should not manipulate DOM/Virtual DOM
"use strict";

export default class RestClient
{
    constructor(ACustomToken, AContentType)
    {
        this.LCustomToken = ACustomToken;
        this.LContentType = AContentType;
    }

    Execute(AMethod, AUrl, APayLoad, ACallback)
    {
        const LRequest = new XMLHttpRequest();
        try
        {
            LRequest.open(AMethod, AUrl, true);
            LRequest.setRequestHeader("Content-Type", this.LContentType);
            LRequest.setRequestHeader("AntiForgeryTokenField", this.LCustomToken);

            LRequest.onload = function ()
            {
                ACallback(this.response, this.status);
            };

            LRequest.onerror = function ()
            {
                ACallback(null, this.status);
            };

            const LMethod = AMethod.toUpperCase();

            if (LMethod === "GET" || LMethod === "DELETE")
            {
                LRequest.send();
            }
            else if (LMethod === "PUT" || LMethod === "POST" || LMethod === "PATCH")
            {
                LRequest.send(APayLoad);
            }
        }
        catch (Error)
        {
            console.error(`[RestClient].[Execute]: An error has been thrown: ${Error.message}`);
        }
    }
}
