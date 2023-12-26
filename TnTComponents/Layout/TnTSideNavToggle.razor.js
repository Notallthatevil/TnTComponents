const elementIdentifierMap = new Map();

const expandedClass = 'expanded';
const elementArray = ['tnt-header', 'tnt-side-nav', 'tnt-body', 'tnt-footer'];
function toggleElements(expand) {
    elementArray.forEach(tagName => {
        const elements = document.getElementsByTagName(tagName);
        if (elements) {
            for (const ele of elements) {
                if (ele && ele.classList) {
                    if (expand) {
                        if (!ele.classList.contains(expandedClass)) {
                            ele.classList.add(expandedClass);
                        }
                    }
                    else {
                        ele.classList.remove(expandedClass);
                    }
                }
            }
        }
    });
}

function toggleSideNav(e) {
    if (e.currentTarget) {
        let toggler = elementIdentifierMap.get(e.currentTarget.getAttribute(TnTComponents.customAttribute));
        if (toggler) {
            e.currentTarget.expanded = !e.currentTarget.expanded;
            if (toggler.dotNetRef) {
                toggler.dotNetRef.invokeMethodAsync('Toggle', e.currentTarget.expanded);
            }
            toggleElements(e.currentTarget.expanded);
        }
    }
}

export class TnTSideNavToggle extends HTMLElement {
    static observedAttributes = [TnTComponents.customAttribute];

    constructor() {
        super();
        this.expanded = this.getAttribute(expandedClass);
    }

    disconnectedCallback() {
        let identifier = this.getAttribute(TnTComponents.customAttribute);
        if (elementIdentifierMap.has(identifier)) {
            elementIdentifierMap.delete(identifier);
        }
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === TnTComponents.customAttribute && oldValue != newValue) {
            let old = elementIdentifierMap.get(oldValue);
            if (elementIdentifierMap.has(oldValue)) {
                elementIdentifierMap.delete(oldValue);
            }
            elementIdentifierMap.set(newValue, { element: this, dotNetRef: old && old.dotNetRef ? old.dotNetRef : null });
        }
    }
}

export function onLoad(element = null, dotNetObjectRef = null) {
    if (!customElements.get('tnt-side-nav-toggle')) {
        customElements.define('tnt-side-nav-toggle', TnTSideNavToggle);
    }
}

export function onUpdate(element = null, dotNetObjectRef = null) {
    if (element) {
        let identifier = element.getAttribute(TnTComponents.customAttribute);
        elementIdentifierMap.set(identifier, { element: element, dotNetRef, dotNetObjectRef });
    }
    for (const [_, tntSideNavToggle] of elementIdentifierMap) {
        if (tntSideNavToggle.element) {
            tntSideNavToggle.element.expanded = tntSideNavToggle.element.getAttribute(expandedClass);
            tntSideNavToggle.element.addEventListener('click', toggleSideNav);
        }
    }
}

export function onDispose(element = null, dotNetObjectRef = null) {
}