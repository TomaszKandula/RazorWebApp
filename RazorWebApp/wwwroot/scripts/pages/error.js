// View module to manipulate Virtual DOM
"use strict";

export default class ErrorPage
{
    /* It is a demo application. You may extend this class to introduce
    * additional interaction on the client-side. */
    constructor(Container)
    {
        this.Container = Container;
    }

    Initialize()
    {
        if (this.Container === null) 
            return null;
    }
}
