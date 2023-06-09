let pdfInstance;
    function loadPDF(container, document) {
        PSPDFKit.load({
            container: container,
            document: document
        }).then(function (instance) {
            pdfInstance = instance;
            instance.setToolbarItems((items) => [
                ...items,
                { type: "form-creator" }
            ]);
        });
    }

async function exportPDF() {
    const CHUNK_SIZE = 8192;
    const arrayBuffer = await pdfInstance.exportPDF();
    const byteArray = new Uint8Array(arrayBuffer);
    const totalChunks = Math.ceil(byteArray.length / CHUNK_SIZE);
    let base64String = '';

    for (let i = 0; i < totalChunks; i++) {
        const chunk = byteArray.subarray(i * CHUNK_SIZE, (i + 1) * CHUNK_SIZE);
        const chunkBase64 = btoa(String.fromCharCode.apply(null, chunk));
        base64String += chunkBase64;
    }

    return base64String;
    //return base64String;

    //const blob = new Blob([arrayBuffer], { type: 'application/pdf' });
    /*
    await fetch("/upload", {
        method: "POST",
        body: formData
    });*/
}