const rippleEffectByIdentifier = new Map();


function getCoords(elem) { // crossbrowser version
    var box = elem.getBoundingClientRect();

    var body = document.body;
    var docEl = document.documentElement;

    var scrollTop = window.scrollY || docEl.scrollTop || body.scrollTop;
    var scrollLeft = window.scrollX || docEl.scrollLeft || body.scrollLeft;

    var clientTop = docEl.clientTop || body.clientTop || 0;
    var clientLeft = docEl.clientLeft || body.clientLeft || 0;

    var top = box.top + scrollTop - clientTop;
    var left = box.left + scrollLeft - clientLeft;

    return { top: Math.round(top), left: Math.round(left) };
}


function rippleEffect(e) {
    if (!e.target || !e.target.querySelector) {
        return;
    }

    const rippler = e.target.querySelector(':scope > tnt-ripple-effect');

    // Setup
    let posX = e.target.offsetLeft;
    let posY = e.target.offsetTop;
    let buttonWidth = e.target.offsetWidth;
    let buttonHeight = e.target.offsetHeight;

    // Make it round!
    if (buttonWidth >= buttonHeight) {
        buttonHeight = buttonWidth;
    } else {
        buttonWidth = buttonHeight;
    }

    // Get the center of the element
    const coords = getCoords(e.target);
    var x = e.pageX - coords.left - buttonWidth / 2;
    var y = e.pageY - coords.top - buttonHeight / 2;

    if (e.type === 'mousedown') {
        const rippleElement = document.createElement('div');
        rippleElement.style.pointerEvents = 'none';
        rippleElement.classList.add('tnt-rippling');

        rippleElement.style.width = `${buttonWidth}px`;
        rippleElement.style.height = `${buttonHeight}px`;
        rippleElement.style.top = `${y}px`;
        rippleElement.style.left = `${x}px`;
        rippler.appendChild(rippleElement);

        setTimeout(() => {
            rippleElement.classList.add('tnt-rippling-mouse-down');
        }, 1);
    }
    else if (e.type === 'mouseup' || e.type === 'mouseleave') {
        const rippleElements = rippler.querySelectorAll(':scope > .tnt-rippling');

        rippleElements.forEach(ripple => {
            ripple.classList.add('tnt-rippling-mouse-up');
        });

        setTimeout(() => {
            rippleElements.forEach(ripple => {
                if (rippler.contains(ripple)) {
                    rippler.removeChild(ripple);
                }
            });
        }, 600);
    }
}

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
            this.parentElement.removeEventListener('mousedown', rippleEffect);
            this.parentElement.addEventListener('mousedown', rippleEffect);

            this.parentElement.removeEventListener('mouseup', rippleEffect);
            this.parentElement.addEventListener('mouseup', rippleEffect);

            this.parentElement.removeEventListener('mouseleave', rippleEffect);
            this.parentElement.addEventListener('mouseleave', rippleEffect);
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