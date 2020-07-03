
"use strict"


import * as _helpers from "./helpers";


function ValidatePasswordField(Value)
{

    var Check = 0;

    if (Value.length < 8)                { Check++; };
    if (_helpers.IsEmpty(Value))         { Check++; };
    if (!_helpers.HasLowerCase(Value))   { Check++; };
    if (!_helpers.HasUpperCase(Value))   { Check++; };
    if (!_helpers.HasSpecialChar(Value)) { Check++; };

    if (Check != 0)
    {
        return false;
    }
    else
    {
        return true;
    }

};


function PerformAjaxCall(Method, Url, PayLoad, Callback)
{

    const ContentType = "application/json; charset=UTF-8";
    var Request = new XMLHttpRequest();

    Request.open(Method, Url, true);
    Request.setRequestHeader("Content-Type", ContentType);

    Request.onload = function ()
    {

        if (this.status === 200)
        {
            var ParsedResponse = JSON.parse(this.response);
            Callback(ParsedResponse, this.status);
        }
        else
        {
            Callback(null, this.status);
        }

    };

    Request.onerror = function ()
    {
        Callback(null, this.status);
    };

    if (Method === "GET" || Method === "DELETE")
    {
        Request.send();
    }

    if (Method === "PUT" || Method === "POST" || Method === "PATCH")
    {
        Request.send(PayLoad);
    }

}


export
{
    ValidatePasswordField,
    PerformAjaxCall
};
