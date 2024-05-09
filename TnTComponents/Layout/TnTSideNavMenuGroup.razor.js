const elementIdentifierMap = new Map();

const expandedClass = 'tnt-expanded';

function toggleChildren(e) {
    if (e.currentTarget) {
        let parent = e.currentTarget.parentNode;
        let toggler = elementIdentifierMap.get(parent.getAttribute(TnTComponents.customAttribute));
        if (toggler) {
            if (parent.expanded == null || parent.expanded == undefined) {
                parent.expanded = parent.classList.contains(expandedClass);
            }
            parent.expanded = !parent.expanded;

            let content = parent.querySelector(':scope > div:last-child');

            if (toggler.dotNetRef) {
                toggler.dotNetRef.invokeMethodAsync('Toggle', parent.expanded);
            }

            if (parent.expanded) {
                if (!parent.classList.contains(expandedClass)) {
                    parent.classList.add(expandedClass);
                    content.style.height = `revert`;
                }
            } else {
                parent.classList.remove(expandedClass);
                content.style.height = 0;
            }
        }
    }
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
            TnTComponents.enableRipple(this);
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
            let span = tntSideNavToggle.element.querySelector(':scope > span:first-child');
            span.addEventListener('click', toggleChildren);

            if (tntSideNavToggle.element.expanded === true && !tntSideNavToggle.element.classList.contains(expandedClass)) {
                tntSideNavToggle.element.classList.add(expandedClass);
            }
            else if (tntSideNavToggle.element.expanded === false && tntSideNavToggle.element.classList.contains(expandedClass)) {
                tntSideNavToggle.element.classList.remove(expandedClass);
            }
        }
    }
}

export function onDispose(element = null, dotNetObjectRef = null) {
}