// View module to manipulate Virtual DOM

"use strict";


export default class IndexPage
{

    constructor(AContainer, ANavButtons)
    {
        this.Container  = AContainer;
        this.NavButtons = ANavButtons;
    }

    Initialize()
    {

        if (this.Container === null)
        {
            return null;
        }

        // call to render nav bar buttons:
        // singup + login - if session expired / no session
        // logout - if user is logged (session valid)

    }

}
