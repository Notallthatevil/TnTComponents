
function toggle(e) {
    if (e && e.target && e.target.parentElement) {
        let parent = e.target.parentElement;

        if (parent.content.classList.contains('tnt-visible') || parent.content.clientHeight) {
            parent.close();
        }
        else {
            parent.open();
        }
    }
}

class TnTAccordion extends HTMLElement {
    constructor() {
        super();
    }

    connectedCallback() {
        this.update();

    }

    disconnectedCallback() {
    }

    adoptedCallback() {
    }

    attributeChangedCallback(name, oldValue, newValue) {
    }

    open() {
        this.content.style.height = `${this.content.firstChild.clientHeight}px`;
        this.content.classList.add('tnt-visible');
    }

    close() {
        this.content.style.height = 0;
        this.content.classList.remove('tnt-visible');
    }

    update() {
        this.header = this.firstChild;
        this.content = this.lastChild;
        if (this.header && this.content) {
            this.header.addEventListener('click', toggle);
        }
    }
}

export function onLoad(element, dotnNetRef) {
    if (!customElements.get('tnt-accordion')) {
        customElements.define('tnt-accordion', TnTAccordion);
    }
}

export function onUpdate(element, dotnNetRef) {
    if (element && dotnNetRef) {
        element.update();
    }
}

export function onDispose(element, dotnNetRef) {
}