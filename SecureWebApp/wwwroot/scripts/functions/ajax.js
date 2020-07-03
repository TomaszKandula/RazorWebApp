
"use strict"


import * as _helpers from "./helpers";


function ValidateEmailAddress(EmailAddress, Handle, OkIcon, ErrIcon)
{

    var Handler   = document.getElementById(Handle);
    var Verified  = document.getElementById(OkIcon);
    var Malformed = document.getElementById(ErrIcon);

    Verified.style.display  = "visibility";
    Malformed.style.display = "visibility";
    Handler.classList.add("is-loading");

    if (!_helpers.IsEmpty(EmailAddress) && _helpers.ValidateEmail(EmailAddress))
    {

        var Url = window.location.origin + "/api/v1/ajax/validation/" + EmailAddress + "/";
        var Request = new XMLHttpRequest();

        Request.open('GET', Url, true);
        Request.onload = function ()
        {

            Handler.classList.remove("is-loading");

            if (this.status == 200)
            {

                var Parsed = JSON.parse(this.response);

                if (Parsed.IsEmailValid)
                {
                    Verified.style.visibility  = "visible";
                    Malformed.style.visibility = "hidden";
                }
                else
                {
                    Verified.style.visibility  = "hidden";
                    Malformed.style.visibility = "visible";
                }

            }
            else
            {
                Verified.style.visibility  = "hidden";
                Malformed.style.visibility = "hidden";
                alert("Status: " + status + ". Server response:" + request.responseText);
            }

        };

        Request.onerror = function ()
        {
            Handler.classList.remove("is-loading");
            Verified.style.visibility  = "hidden";
            Malformed.style.visibility = "visible";
            alert("An error has occurred during the processing.");
        };

        Request.send();

    }
    else
    {
        Handler.classList.remove("is-loading");
        Verified.style.visibility  = "hidden";
        Malformed.style.visibility = "hidden";
    }

}


function GetCityList(SelectedCountryId, TargetHandle, TargetSelect)
{

    var Handler  = document.getElementById(TargetHandle);
    var Selector = document.getElementById(TargetSelect);

    var Url = window.location.origin + "/api/v1/ajax/cities/" + SelectedCountryId + "/";
    var Request = new XMLHttpRequest();

    Handler.classList.add("is-loading");

    Request.open('GET', Url, true);
    Request.onload = function ()
    {

        Handler.classList.remove("is-loading");

        if (this.status == 200)
        {

            _helpers.ClearSelectElement(Selector);
            var Parsed = JSON.parse(this.response);

            for (var Index = 0; Index < Parsed.Cities.length; Index++)
            {

                var City = Parsed.Cities[Index];
                var Option = document.createElement("option");

                Option.value = City.id;
                Option.innerHTML = City.name;
                Selector.appendChild(Option);

            }

            Selector.removeAttribute("disabled");
            Selector.selectedIndex = 0;

        }
        else
        {
            alert("Status: " + status + ". Server response:" + request.responseText);
        }

    }

    Request.onerror = function ()
    {
        Handler.classList.remove("is-loading");
        alert("An error has occurred during the processing.");
    }

    Request.send();

}


export
{
    ValidateEmailAddress,
    GetCityList
};

