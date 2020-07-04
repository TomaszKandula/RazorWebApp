
"use strict";


function IsEmpty(AValue)
{
    return typeof AValue === 'string' && !AValue.trim() || typeof AValue === undefined || AValue === null;
};


function IsNumeric(AValue)
{
    return !isNaN(parseFloat(AValue)) && isFinite(AValue);
};


function ValidateEmail(AEmail)
{
    let LRegex = /\S+@\S+\.\S+/;
    return LRegex.test(AEmail);
};


function FormatPhoneNumber(ANumber)
{
    ANumber = ANumber.replace(/[^\d]+/g, '').replace(/(\d{2})(\d{3})(\d{3})(\d{3})/, '($1) $2 $3 $4');

    if (isEmpty(ANumber))
    {
        return false;
    }
    else
    {
        return ANumber;
    };

};


function HasLowerCase(AText)
{
    if (AText.toUpperCase() != AText)
    {
        return true;
    }

    return false;
}


function HasUpperCase(AText)
{
    if (AText.toLowerCase() != AText)
    {
        return true;
    }

    return false;
}


function HasSpecialChar(str)
{
    let LFormat = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/;

    if (LFormat.test(str))
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

    let LCurrentUrl = window.location.href;
    let LCheck = 0;
    let LBaseUrl = "";

    for (let Index = 0; Index <= LCurrentUrl.length; Index++)
    {
        LBaseUrl = LCurrentUrl.charAt(Index);
        if (LBaseUrl.charAt(Index) === "/")
        {
            LCheck++;
            if (LCheck === 2)
            {
                break;
            };
        };
    }

    return LBaseUrl;

}


function ClearSelectElement(ASelectElement)
{
    let Index, Length = ASelectElement.options.length - 1;

    for (Index = Length; Index >= 0; Index--)
    {
        ASelectElement.remove(Index);
    }

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
    CleanBaseUrl,
    ClearSelectElement
};
