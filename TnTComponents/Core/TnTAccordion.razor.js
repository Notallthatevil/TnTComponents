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

    update() {
        this.header = this.firstChild;
        this.content = this.lastChild;

        if (this.header && this.content) {
            this.header.addEventListener('click', (e) => {
                if (this.content.clientHeight) {
                    this.content.style.height = 0;
                    this.content.classList.remove('tnt-visible');
                }
                else {
                    this.content.style.height = `${this.content.firstChild.clientHeight}px`;
                    this.content.classList.add('tnt-visible');
                }
            });
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