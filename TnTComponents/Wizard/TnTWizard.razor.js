const wizardsByIdentifier = new Map();

export class TnTWizard extends HTMLElement {
    static observedAttributes = [TnTComponents.customAttribute];
 
}

export function onLoad(element, dotNetRef) {
    if (!customElements.get('tnt-wizard')) {
        customElements.define('tnt-wizard', TnTWizard);
    }

    if (dotNetRef) {
        //element.update();
    }
}

export function onUpdate(element, dotNetRef) {
    if (dotNetRef) {
        //element.update();
    }
}

export function onDispose(element, dotNetRef) {
}