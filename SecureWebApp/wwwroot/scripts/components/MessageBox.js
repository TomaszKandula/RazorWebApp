// MessageBox component
"use strict";

export default class MessageBox
{
    constructor(AModalHandle)
    {
        this.LModalHandle = AModalHandle;
        let LMessageType = "AlertInfo";
        let LCaption     = "Title";
        let LContent     = "<p>No content</p>";

        this.SetMessageType = function (AMessageType)
        {
            LMessageType = AMessageType;
        }

        this.GetMessageType = function ()
        {
            return LMessageType;
        }

        this.SetTitle = function (ATitle)
        {
            LCaption = ATitle;
        }

        this.GetTitle = function ()
        {
            return LCaption;
        }

        this.SetContent = function (AContent)
        {
            LContent = AContent;
        }

        this.GetContent = function ()
        {
            return LContent;
        }
    }

    Show()
    {
        this.Render();
        this.BindDom();
        this.AddEvents();
        this.LModalHandle.classList.add("is-active");
    }

    BindDom()
    {
        this.Button_CloseModal = this.LModalHandle.querySelector("#ModalClose");
    }

    AddEvents()
    {
        this.Button_CloseModal.addEventListener("click", () => 
        { 
            this.LModalHandle.classList.remove("is-active"); 
        });
    }

    RenderAlert(AFlag)
    {
        return this.LModalHandle.innerHTML = 
            `<div class="modal-background"></div>
             <div class="modal-card">
                <article class="message ${AFlag}">
                    <div class="message-header">
                        <p>${this.GetTitle()}</p>
                        <button id="ModalClose" class="delete" aria-label="delete"></button>
                    </div>
                    <div class="message-body">
                        ${this.GetContent()}
                    </div>
                </article>
            </div>`;
    }

    RenderDialog()
    {
        return this.LModalHandle.innerHTML = 
            `<div class="modal-background"></div>
                <div class="modal-card">
                <header class="modal-card-head">
                    <p class="modal-card-title">${this.GetTitle()}</p>
                    <button id="ModalClose" class="delete" aria-label="close"></button>
                </header>
                <section class="modal-card-body">
                    ${this.GetContent()}
                </section>
            </div>`;
    }

    Render()
    {
        switch (this.GetMessageType())
        {
            case "AlertInfo":
                this.RenderAlert("is-info");
                break;
            case "AlertSuccess":
                this.RenderAlert("is-success");
                break;
            case "AlertWarning":
                this.RenderAlert("is-warning");
                break;
            case "AlertError":
                this.RenderAlert("is-danger");
                break;
            case "Dialog":
                this.RenderDialog();
                break;
            default:
                this.RenderDialog();
        }
    }
}
