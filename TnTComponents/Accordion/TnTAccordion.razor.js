const accordionByIdentifier = new Map();

function toggleAccordionHeader(e) {
    const target = e.target;
    const accordion = target.closest('tnt-accordion');
    const content = e.target.parentElement.lastElementChild;
    if (accordion) {
        if (accordion.allowOnlyOneOpen && !content.classList.contains('tnt-expanded')) {
            accordion.closeChildren(target.parentElement);
        }
        if (content.classList.contains('tnt-expanded')) {
            content.classList.remove('tnt-expanded');
            content.classList.add('tnt-collapsed');

            const nestedAccordion = content.querySelectorAll('tnt-accordion');
            nestedAccordion.forEach((accordion) => {
                accordion.resetChildren();
            });
        }
        else {
            content.classList.remove('tnt-collapsed');
            content.classList.add('tnt-expanded');
        }
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
        this.identifier = '';
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
            this.identifier = newValue;

            this.allowOnlyOneOpen = this.classList.contains('tnt-limit-one-expanded');
            this.update();
        }
    }

    update() {
        this.accordionChildren = this.querySelectorAll(':scope > .tnt-accordion-child');
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
                if (child.lastElementChild.classList.contains('tnt-expanded')) {
                    child.lastElementChild.classList.remove('tnt-expanded');
                    child.lastElementChild.classList.add('tnt-collapsed');
                }
            }
        });
    }

    resetChildren() {
        this.accordionChildren.forEach((child) => {
            child.lastElementChild.classList.remove('tnt-expanded');
            child.lastElementChild.classList.remove('tnt-collapsed');

            const nestedAccordion = child.querySelectorAll('tnt-accordion');
            if (nestedAccordion) {
                nestedAccordion.forEach((accordion) => {
                    accordion.resetChildren();
                });
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
    if (element && element.update) {
        element.update();
    }
}

export function onDispose(element, dotNetRef) {
}