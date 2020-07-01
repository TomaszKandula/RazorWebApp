"use strict"


import * as _helpers from "./functions/helpers";
import * as _modals  from "./functions/modals";
import * as _ajax    from "./functions/ajax";


DOMReady(Initialize());


function DOMReady(Initialize)
{

    if (document.readyState != 'loading')
    {
        Initialize();
    }
    else
    {
        document.addEventListener('DOMContentLoaded', Initialize);
    }

}


function Initialize()
{

}
