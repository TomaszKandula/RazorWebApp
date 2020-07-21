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

    }

}
