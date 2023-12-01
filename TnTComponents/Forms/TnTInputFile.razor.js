// Called when the script first gets loaded on the page.
export function onLoad() {
    const url = new URL(import.meta.url);
    const btnIdentifier = url.searchParams.get("tnt-button-identifier");
    const tntComponentIdentifier = url.searchParams.get("tnt-input-identifier");
    if (btnIdentifier && tntComponentIdentifier) {
        var button = document.getElementById(btnIdentifier);
        var fileInput = document.querySelector(`[${tntComponentIdentifier}]`);
        if (button && fileInput) {
            button.addEventListener("click", function (e) {
                fileInput.click();
            });
        }
    }
}

// Called when an enhanced page update occurs, plus once immediately after
// the initial load.
export function onUpdate() {
}

// Called when an enhanced page update removes the script from the page.
export function onDispose() {
}