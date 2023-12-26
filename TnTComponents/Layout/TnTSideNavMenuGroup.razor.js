const elementIdentifierMap = new Map();

const expandedClass = 'expanded';

function toggleChildren(e) {
    if (e.currentTarget) {
        let toggler = elementIdentifierMap.get(e.currentTarget.getAttribute(TnTComponents.customAttribute));
        if (toggler) {
            e.currentTarget.expanded = !e.currentTarget.expanded;
            if (toggler.dotNetRef) {
                toggler.dotNetRef.invokeMethodAsync('Toggle', e.currentTarget.expanded);
            }

            if (e.currentTarget.expanded) {
                if (!e.currentTarget.classList.contains(expandedClass)) {
                    e.currentTarget.classList.add(expandedClass);
                }
            } else {
                e.currentTarget.classList.remove(expandedClass);
            }
        }
    }
    e.stopPropagation();
}

export class TnTSideNavMenuGroup extends HTMLElement {
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
    if (!customElements.get('tnt-side-nav-menu-group')) {
        customElements.define('tnt-side-nav-menu-group', TnTSideNavMenuGroup);
    }
}

export function onUpdate(element = null, dotNetObjectRef = null) {
    if (element) {
        let identifier = element.getAttribute(TnTComponents.customAttribute);
        elementIdentifierMap.set(identifier, { element: element, dotNetRef, dotNetObjectRef });
    }
    for (const [_, tntSideNavToggle] of elementIdentifierMap) {
        if (tntSideNavToggle.element) {
            tntSideNavToggle.element.addEventListener('click', toggleChildren);
        }
    }
}

export function onDispose(element = null, dotNetObjectRef = null) {
}