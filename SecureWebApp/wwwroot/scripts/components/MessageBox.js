// MessageBox component

"use strict";


export default class MessageBox
{

    constructor(AModalHandle)
    {

        this.LModalHandle = AModalHandle;

        this.LMessageType = "AlertInfo";
        this.LCaption     = "Title";
        this.LContent     = "<p>No content</p>";

    }

    SetMessageType(AMessageType)
    {
        this.LMessageType = AMessageType;
    }

    SetTitle(ATitle)
    {
        this.LCaption = ATitle;
    }

    SetContent(AContent)
    {
        this.LContent = AContent;
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
        this.Button_CloseModal.addEventListener("click", () => { this.LModalHandle.classList.remove("is-active"); });
    }

    RenderAlert(AFlag)
    {
        return this.LModalHandle.innerHTML = 
            `<div class="modal-background"></div>
             <div class="modal-card">
                <article class="message ` + AFlag + `">
                    <div class="message-header">
                        <p>` + this.LCaption + `</p>
                        <button id="ModalClose" class="delete" aria-label="delete"></button>
                    </div>
                    <div class="message-body">
                        ` + this.LContent + `
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
                    <p class="modal-card-title">` + this.LCaption + `</p>
                    <button id="ModalClose" class="delete" aria-label="close"></button>
                </header>
                <section class="modal-card-body">
                    ` + this.LContent + `
                </section>
            </div>`;
    }

    Render()
    {

        switch (this.LMessageType)
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
