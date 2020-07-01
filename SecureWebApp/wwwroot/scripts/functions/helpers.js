"use strict";


function IsEmpty(value)
{
    return typeof value === 'string' && !value.trim() || typeof value === undefined || value === null;
};


function IsNumeric(n)
{
    return !isNaN(parseFloat(n)) && isFinite(n);
};


function ValidateEmail(email)
{
    var re = /\S+@\S+\.\S+/;
    return re.test(email);
};


function FormatPhoneNumber(Number)
{
    Number = Number.replace(/[^\d]+/g, '').replace(/(\d{2})(\d{3})(\d{3})(\d{3})/, '($1) $2 $3 $4');

    if (isEmpty(Number))
    {
        return false;
    }
    else
    {
        return Number;
    };

};


function HasLowerCase(str)
{
    if (str.toUpperCase() != str)
    {
        return true;
    }

    return false;
}


function HasUpperCase(str)
{
    if (str.toLowerCase() != str)
    {
        return true;
    }

    return false;
}


function HasSpecialChar(str)
{
    var format = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/;

    if (format.test(str))
    {
        return true;
    }
    else
    {
        return false;
    }

}

function CleanBaseUrl()
{
    var CurrentUrl = window.location.href;
    var Check = 0;
    var BaseUrl = "";

    for (var iCNT = 0; iCNT <= CurrentUrl.length; iCNT++)
    {
        BaseUrl = CurrentUrl.charAt(iCNT);
        if (BaseUrl.charAt(iCNT) === "/")
        {
            Check++;
            if (Check === 2)
            {
                break;
            };
        };
    }

    return BaseUrl;

}


export
{
    FormatPhoneNumber,
    HasSpecialChar,
    HasLowerCase,
    HasUpperCase,
    IsNumeric,
    IsEmpty,
    ValidateEmail,
    CleanBaseUrl
};
