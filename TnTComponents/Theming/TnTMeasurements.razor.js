
class TnTMeasurementsElement extends HTMLElement {
    static get observedAttributes() { return ["tnt-footer-height", "tnt-header-height", "tnt-side-nav-width"]; }

    constructor() {
        super();
        this._styleElement = null;
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === "tnt-footer-height" || name === "tnt-header-height" || name === "tnt-side-nav-width") {
            this._ensureStyleElement();
            this._applyMeasurements();
        }
    }

    _ensureStyleElement() {
        if (!this._styleElement) {
            this._styleElement = document.createElement("style");
            this._styleElement.setAttribute("id", "tnt-measurements-style");
            document.head.appendChild(this._styleElement);
        }
    }

    _applyMeasurements() {
        let css = ":root{";
        const footerHeight = this.getAttribute("tnt-footer-height");
        if (footerHeight !== null) {
            css += `--tnt-footer-height:${footerHeight}px !important;`;
        } else {
            css += `--tnt-footer-height:64px !important;`;
        }
        const headerHeight = this.getAttribute("tnt-header-height");
        if (headerHeight !== null) {
            css += `--tnt-header-height:${headerHeight}px !important;`;
        } else {
            css += `--tnt-header-height:64px !important;`;
        }
        const sideNavWidth = this.getAttribute("tnt-side-nav-width");
        if (sideNavWidth !== null) {
            css += `--tnt-side-nav-width:${sideNavWidth}px !important;`;
        } else {
            css += `--tnt-side-nav-width:256px !important;`;
        }
        css += "}";
        this._styleElement.textContent = css;
    }
}




export function onLoad(element, dotNetRef) {
    if (!window.customElements.get("tnt-measurements")) {
        window.customElements.define("tnt-measurements", TnTMeasurementsElement);
    }
}

export function onUpdate(element, dotNetRef) {
}

export function onDispose(element, dotNetRef) {
}