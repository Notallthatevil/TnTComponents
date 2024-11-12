
const accordionByIdentifier = new Map();

function toggleAccordionHeader(e) {
    const target = e.target;
    const accordion = target.closest('tnt-accordion');
    const content = e.target.parentElement.lastElementChild;
    if (accordion) {
        if (accordion.allowOnlyOneOpen && !content.classList.contains('tnt-expanded')) {
            accordion.closeChildren(target.parentElement);
        }
        content.classList.toggle('tnt-expanded');
        updateChild(content);
    }
}

function updateChild(content) {
    if (content.resizeObserver) {
        content.resizeObserver.disconnect();
        content.resizeObserver = undefined;
    }
    if (content.classList.contains('tnt-expanded')) {
        content.style.setProperty('--content-height', content.scrollHeight + 'px');

        content.resizeObserver = new ResizeObserver((entries) => {
            content.style.setProperty('--content-height', content.scrollHeight + 'px');
        });

        content.resizeObserver.observe(document.body);
    }
    else {
        content.style.height = null;
    }
}

export class TnTAccordion extends HTMLElement {
    static observedAttributes = [TnTComponents.customAttribute];
    constructor() {
        super();
        this.allowOnlyOneOpen = false;
        this.accordionChildren = [];
    }

    disconnectedCallback() {
        let identifier = this.getAttribute(TnTComponents.customAttribute);
        if (accordionByIdentifier.get(identifier)) {
            accordionByIdentifier.delete(identifier);
        }
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === TnTComponents.customAttribute && oldValue != newValue) {
            if (accordionByIdentifier.get(oldValue)) {
                accordionByIdentifier.delete(oldValue);
            }
            accordionByIdentifier.set(newValue, this);

            this.allowOnlyOneOpen = this.classList.contains('tnt-limit-one-expanded');
            this.update();
        }
    }


    update() {
        this.accordionChildren = this.querySelectorAll('.tnt-accordion-child');
        this.accordionChildren.forEach((child) => {
            const header = child.firstElementChild;

            if (!header.classList.contains('tnt-disabled')) {
                header.addEventListener('click', toggleAccordionHeader)
            }

            updateChild(child.lastElementChild);
        });
    }

    closeChildren(exclude) {
        this.accordionChildren.forEach((child) => {
            if (child !== exclude) {
                child.lastElementChild.classList.remove('tnt-expanded');
            }
        });
    }
}

export function onLoad(element, dotNetRef) {
    if (!customElements.get('tnt-accordion')) {
        customElements.define('tnt-accordion', TnTAccordion);
    }
}

export function onUpdate(element, dotNetRef) {
}

export function onDispose(element, dotNetRef) {
}