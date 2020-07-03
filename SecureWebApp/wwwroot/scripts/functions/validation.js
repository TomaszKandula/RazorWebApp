
"use strict"


import * as _helpers from "./helpers";


function ValidateEditTextField(Value, OkIcon, ErrIcon)
{

    var Verified  = document.getElementById(OkIcon);
    var Malformed = document.getElementById(ErrIcon);

    if (_helpers.IsEmpty(Value))
    {
        Verified.style.visibility  = "hidden";
        Malformed.style.visibility = "visible";
    }
    else
    {
        Verified.style.visibility  = "visible";
        Malformed.style.visibility = "hidden";
    }

}


function ValidatePasswordField(Value, OkIcon, ErrIcon)
{




};


export
{
    ValidateEditTextField,
    ValidatePasswordField
};
