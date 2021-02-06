// View module to manipulate Virtual DOM
"use strict";

export default class ErrorPage
{
    /* This is demo application. You may extend this class to introduce
    * additional interaction on client side. */
    constructor(Container)
    {
        this.Container = Container;
    }

    Initialize()
    {
        if (this.Container === null) return null;
    }
}
