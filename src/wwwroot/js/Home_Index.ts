
let rtmpUrlTextbox = <HTMLInputElement | null>document.getElementById("rtmp-url");
let copyToClipboardButton = <HTMLButtonElement | null>document.getElementById("copyUrlButton");

if (rtmpUrlTextbox) {
    rtmpUrlTextbox.value = `rtmp://${location.hostname}/live/stream`;
}

if (copyToClipboardButton) {
    copyToClipboardButton.addEventListener("click", function () {
        if (rtmpUrlTextbox) {
            rtmpUrlTextbox.select();
            rtmpUrlTextbox.setSelectionRange(0, 99999); // For mobile devices

            // Copy the text inside the text field
            navigator.clipboard.writeText(rtmpUrlTextbox.value);
        }
    });
}