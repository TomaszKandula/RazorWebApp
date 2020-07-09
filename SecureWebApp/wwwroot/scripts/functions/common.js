// This module should not manipulate DOM

"use strict"


import * as _helpers from "./helpers";


function ValidatePasswordField(AValue)
{

    let LCheck = 0;

    if (AValue.length < 8)                { LCheck++; };
    if (_helpers.IsEmpty(AValue))         { LCheck++; };
    if (!_helpers.HasLowerCase(AValue))   { LCheck++; };
    if (!_helpers.HasUpperCase(AValue))   { LCheck++; };
    if (!_helpers.HasSpecialChar(AValue)) { LCheck++; };

    if (LCheck != 0)
    {
        return false;
    }
    else
    {
        return true;
    }

};


function PerformAjaxCall(AMethod, AUrl, ACustomToken, AContentType, APayLoad, ACallback)
{

    let LRequest = new XMLHttpRequest();

    LRequest.open(AMethod, AUrl, true);
    LRequest.setRequestHeader("Content-Type", AContentType);
    LRequest.setRequestHeader("X-ApiKey", ACustomToken);

    LRequest.onload = function ()
    {

        if (this.status >= 200 && this.status < 400)
        {
            ACallback(this.response, this.status);
        }
        else
        {
            ACallback(null, this.status);
        }

    };

    LRequest.onerror = function ()
    {
        ACallback(null, this.status);
    };

    let LMethod = AMethod.toUpperCase();

    if (LMethod === "GET" || LMethod === "DELETE")
    {
        LRequest.send();
    }

    if (LMethod === "PUT" || LMethod === "POST" || LMethod === "PATCH")
    {
        LRequest.send(APayLoad);
    }

}


export
{
    ValidatePasswordField,
    PerformAjaxCall
};
