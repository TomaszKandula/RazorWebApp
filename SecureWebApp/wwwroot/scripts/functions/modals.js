
"use strict";


export default function CallModalWindow(ContentUrl, ModalText, ModalWindow)
{

    var Url = window.location.origin + ContentUrl;
    var Request = new XMLHttpRequest();

    Request.open('GET', Url, true);
    Request.onload = function ()
    {

        if (this.status >= 200 && this.status < 400)
        {

            var Response = this.response;

            var GetModalWindow = document.getElementById(ModalWindow);
            var GetModalText   = document.getElementById(ModalText);

            GetModalText.innerHTML(Response);
            GetModalWindow.style.display = '';

        }
        else
        {
            console.log("Status: " + status + ". Server response:" + request.responseText);
        }

    };

    Request.onerror = function ()
    {
        console.log("An error has occurred during the processing.");
    };

    Request.send();

}
