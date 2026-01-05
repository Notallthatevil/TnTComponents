const accordionByIdentifier = new Map();

export class TnTAccordion extends HTMLElement {
    static observedAttributes = [NTComponents.customAttribute];
    constructor() {
        super();
        this.accordionChildren = [];
        this.identifier = '';
        this.dotNetRef = null;
    }

    disconnectedCallback() {
        let identifier = this.getAttribute(NTComponents.customAttribute);
        if (accordionByIdentifier.get(identifier)) {
            accordionByIdentifier.delete(identifier);
        }
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === NTComponents.customAttribute && oldValue != newValue) {
            if (accordionByIdentifier.get(oldValue)) {
                accordionByIdentifier.delete(oldValue);
            }
            accordionByIdentifier.set(newValue, this);
            this.identifier = newValue;

            
            this.update();
        }
    }

    limitToOneExpanded() { return this.classList.contains('tnt-limit-one-expanded'); }

    update() {
        this.accordionChildren = this.querySelectorAll(':scope > .tnt-accordion-child');
        let self = this;
        this.accordionChildren.forEach((child) => {
            self.updateChild(child.lastElementChild);
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

    updateChild(content) {
        if (content.resizeObserver) {
            content.resizeObserver.disconnect();
            content.resizeObserver = undefined;
        }

        if (content.mutationObserer) {
            content.mutationObserer.disconnect();
            content.mutationObserer = undefined
        }

        if (content.classList.contains('tnt-expanded')) {
            content.style.setProperty('--content-height', content.scrollHeight + 'px');

            content.resizeObserver = new ResizeObserver((entries) => {
                content.style.setProperty('--content-height', content.scrollHeight + 'px');
            });

            content.mutationObserer = new MutationObserver((mutationList) => {
                for (const mutation of mutationList) {
                    if (mutation.type === 'childList') {
                        content.style.setProperty('--content-height', content.scrollHeight + 'px');
                    }
                }
            });

            content.resizeObserver.observe(document.body);
            content.mutationObserer.observe(content, { childList: true, subtree: true });
        }
        else {
            content.style.height = null;
        }
    }
}

export function onLoad(element, dotNetRef) {
    if (!customElements.get('tnt-accordion')) {
        customElements.define('tnt-accordion', TnTAccordion);
    }
    if (element) {
        element.dotNetRef = dotNetRef;
    }
}

export function onUpdate(element, dotNetRef) {
    if (element && element.update) {
        element.update();
        element.dotNetRef = dotNetRef;
    }
}

export function onDispose(element, dotNetRef) { }