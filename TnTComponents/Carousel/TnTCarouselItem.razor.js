const carouselsByIdentifier = new Map();

export class TnTCarouselItem extends HTMLElement {
    static observedAttributes = [TnTComponents.customAttribute];
    constructor() {
        super();
        this.naturalWidth = null;
        this.backgroundImageWidth = '80%';
        this.contentContainer = null;
    }

    disconnectedCallback() {
        let identifier = this.getAttribute(TnTComponents.customAttribute);
        if (carouselsByIdentifier.get(identifier)) {
            carouselsByIdentifier.delete(identifier);
        }
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === TnTComponents.customAttribute && oldValue != newValue) {
            if (carouselsByIdentifier.get(oldValue)) {
                carouselsByIdentifier.delete(oldValue);
            }
            carouselsByIdentifier.set(newValue, this);
            this.onUpdate();
        }
    }

    calculateNaturalWidth() {
        const bgImage = this.contentContainer.style.backgroundImage;
        if (bgImage && bgImage !== 'none') {
            // Extract URL from background-image: url("...")
            const match = bgImage.match(/url\(["']?(.*?)["']?\)/);
            if (match && match[1]) {
                const img = new window.Image();
                img.onload = () => {
                    this.naturalWidth = img.naturalWidth;
                    this.backgroundImageWidth = `${this.naturalWidth}px`;
                    this.style.setProperty('--tnt-carousel-item-bg-width', this.backgroundImageWidth);
                    this.style.width = this.backgroundImageWidth;
                    this.updateItemWidth();
                };
                img.src = match[1];
            }
        }
        else {
            this.style.width = '80%';
            this.updateItemWidth();
        }
    }

    updateItemWidth() {
        // Get bounding rects
        const parentRect = this.carouselContainer.getBoundingClientRect();
        const itemRect = this.getBoundingClientRect();

        let newWidth = this.naturalWidth;
        if (itemRect.width > parentRect.width) {
            this.style.width = `${parentRect.width}px`;
            this.contentContainer.style.width = '100%';
        }
        else if (itemRect.right > parentRect.right) {
            if (itemRect.left > parentRect.right) {
                newWidth = 0;
            }
            else {
                newWidth = parentRect.right - itemRect.left;
            }
            this.contentContainer.style.width = `${newWidth}px`;
            this.style.justifyContent = null;
        }
        else if (itemRect.left < parentRect.left) {
            if (itemRect.right < parentRect.left) {
                newWidth = 0;
            }
            else {
                newWidth = itemRect.right - parentRect.left;
            }
            this.contentContainer.style.width = `${newWidth}px`;
            this.style.justifyContent = 'end';
            this.style.width = this.backgroundImageWidth;
        }
        else {
            this.contentContainer.style.width = `${newWidth}px`;
            this.style.width = `${newWidth}px`;
            this.style.justifyContent = null;
        }
    }

    onUpdate() {
        this.contentContainer = this.querySelector(':scope > .tnt-carousel-item');

        this.carouselContainer = this.closest('.tnt-carousel-viewport');
        this.calculateNaturalWidth();
    }
}
export function onLoad(element, dotNetRef) {
    if (!customElements.get('tnt-carousel-item')) {
        customElements.define('tnt-carousel-item', TnTCarouselItem);
    }
}

export function onUpdate(element, dotNetRef) {
    if (element && element instanceof TnTCarouselItem) {
        element.onUpdate();
    }
}
export function onDispose(element, dotNetRef) {

}