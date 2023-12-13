class TnTAccordion extends HTMLElement {
    constructor() {
        super();
    }

    connectedCallback() {
        this.header = this.firstChild;
        this.content = this.lastChild;

        if (this.header && this.content) {
            this.header.addEventListener('click', (e) => {
                if (this.content.clientHeight) {
                    this.content.style.height = 0;
                    this.content.classList.remove('visible');
                }
                else {
                    this.content.style.height = `${this.content.firstChild.clientHeight}px`;
                    this.content.classList.add('visible');
                }
            });
        }
    }

    disconnectedCallback() {
    }

    adoptedCallback() {
    }

    attributeChangedCallback(name, oldValue, newValue) {
    }
}

export function onLoad() {
    customElements.define('tnt-accordion', TnTAccordion);
}

