let pdfInstance;

function loadPDF(container, document, readOnly = false) {
    const initialViewState = new PSPDFKit.ViewState({
        readOnly: readOnly,
    });

    PSPDFKit.load({
        container: container,
        document: document,
        initialViewState
    }).then(function (instance) {
        pdfInstance = instance;
        instance.setToolbarItems((items) => [
            ...items,
            { type: "form-creator" }
        ]);
    });
}

async function exportPDF() {
    const arrayBuffer = await pdfInstance.exportPDF();
    const byteArray = new Uint8Array(arrayBuffer);
    const base64String = btoa(String.fromCharCode.apply(null, byteArray));
    return base64String;
}