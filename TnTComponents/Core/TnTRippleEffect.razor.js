const rippleEffectByIdentifier = new Map();

export class TnTRippleEffect extends HTMLElement {
    static observedAttributes = [TnTComponents.customAttribute];
    constructor() {
        super();
    }

    disconnectedCallback() {
        let identifier = this.getAttribute(TnTComponents.customAttribute);
        if (rippleEffectByIdentifier.get(identifier)) {
            rippleEffectByIdentifier.delete(identifier);
        }
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === TnTComponents.customAttribute && oldValue != newValue) {
            if (rippleEffectByIdentifier.get(oldValue)) {
                rippleEffectByIdentifier.delete(oldValue);
            }
            rippleEffectByIdentifier.set(newValue, this);
            this.update();
        }
    }

    update() {
        if (this.parentElement && this.parentElement.addEventListener) {
            this.parentElement.addEventListener('click', TnTComponents.rippleEffect);
        }
    }
}


export function onLoad(element, dotnNetRef) {
    if (!customElements.get('tnt-ripple-effect')) {
        customElements.define('tnt-ripple-effect', TnTRippleEffect);
    }
}

export function onUpdate(element, dotnNetRef) {
    if (element && element.update) {
        element.update();
    }
}

export function onDispose(element, dotnNetRef) {
}