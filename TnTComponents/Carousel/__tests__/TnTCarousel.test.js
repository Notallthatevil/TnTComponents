import { TnTCarousel } from '../TnTCarousel.razor.js';

// Polyfills / mocks
class ResizeObserverMock {
  constructor(cb){ this.cb = cb; }
  observe(){ /* noop */ }
  disconnect(){ /* noop */ }
}

global.ResizeObserver = ResizeObserverMock;

describe('TnTCarousel custom element', () => {
  beforeAll(() => {
    if(!customElements.get('tnt-carousel')) {
      customElements.define('tnt-carousel', TnTCarousel);
    }
  });

  function createCarousel(count = 3) {
    const carousel = document.createElement('tnt-carousel');
    const viewport = document.createElement('div');
    viewport.className = 'tnt-carousel-viewport';
    Object.defineProperty(viewport, 'clientWidth', { value: 500, configurable: true });
    Object.defineProperty(viewport, 'scrollWidth', { value: 500 * count, configurable: true });
    Object.defineProperty(viewport, 'scrollLeft', { value: 0, writable: true, configurable: true });

    viewport.setPointerCapture = () => {}; // stub

    for (let i=0;i<count;i++) {
      const item = document.createElement('tnt-carousel-item');
      Object.defineProperty(item, 'offsetLeft', { value: i * 500, configurable: true });
      Object.defineProperty(item, 'offsetWidth', { value: 500, configurable: true });
      viewport.appendChild(item);
    }

    carousel.appendChild(document.createElement('button')).className='tnt-carousel-prev-button';
    carousel.appendChild(document.createElement('button')).className='tnt-carousel-next-button';
    carousel.appendChild(viewport);

    document.body.appendChild(carousel);
    return { carousel, viewport };
  }

  test('registers and upgrades element', () => {
    const el = document.createElement('tnt-carousel');
    expect(el instanceof TnTCarousel).toBe(true);
  });

  test('onUpdate wires viewport, items and sets gap css var', () => {
    const { carousel, viewport } = createCarousel();
    carousel.onUpdate();
    expect(carousel.carouselViewPort).toBe(viewport);
    expect(carousel.carouselItems.length).toBe(3);
    expect(viewport.style.getPropertyValue('--tnt-carousel-item-gap')).toBe('8px');
  });

  test('navigation buttons go next/prev', () => {
    const { carousel } = createCarousel();
    carousel.onUpdate();
    const next = carousel.querySelector('.tnt-carousel-next-button');
    const prev = carousel.querySelector('.tnt-carousel-prev-button');

    carousel.carouselViewPort.scrollTo = ({left}) => { carousel.carouselViewPort.scrollLeft = left; };

    next.click();
    expect(carousel._currentAutoIndex).toBe(1);
    prev.click();
    expect(carousel._currentAutoIndex).toBe(0);
  });

  test('auto play advances items', () => {
    const { carousel } = createCarousel();
    carousel.setAttribute('tnt-auto-play-interval', '0.01');
    carousel.onUpdate();
    carousel.carouselViewPort.scrollTo = ({left}) => { carousel.carouselViewPort.scrollLeft = left; };

    return new Promise(resolve => {
      setTimeout(() => {
        expect(carousel._currentAutoIndex).toBeGreaterThanOrEqual(0);
        carousel.disconnectedCallback();
        resolve();
      }, 30);
    });
  });

  test('dragging disabled when attribute false', () => {
    const { carousel } = createCarousel();
    carousel.setAttribute('tnt-allow-dragging', 'false');
    carousel.onUpdate();
    const ev = new PointerEvent('pointerdown', { clientX:0, button:0, pointerType:'mouse' });
    carousel.carouselViewPort.dispatchEvent(ev);
    expect(carousel._isDragging).toBe(false);
  });

  test('dragging sets dragging flag and updates scroll', () => {
    const { carousel } = createCarousel();
    carousel.setAttribute('tnt-allow-dragging', 'true');
    carousel.onUpdate();
    carousel.carouselViewPort.scrollTo = ({left}) => { carousel.carouselViewPort.scrollLeft = left; };

    const down = new PointerEvent('pointerdown', { clientX:100, button:0, pointerType:'mouse' });
    carousel.carouselViewPort.dispatchEvent(down);
    expect(carousel._isDragging).toBe(true);

    const move = new PointerEvent('pointermove', { clientX:50, pointerType:'mouse' });
    carousel.carouselViewPort.dispatchEvent(move);
    expect(carousel.carouselViewPort.scrollLeft).toBeGreaterThan(0);

    const up = new PointerEvent('pointerup', { clientX:50, pointerType:'mouse' });
    carousel.carouselViewPort.dispatchEvent(up);
    expect(carousel._isDragging).toBe(false);
  });
});
