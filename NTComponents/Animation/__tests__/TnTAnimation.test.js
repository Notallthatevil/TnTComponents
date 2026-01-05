import { TnTAnimation, onLoad } from '../TnTAnimation.razor.js';

// Polyfill for IntersectionObserver for deterministic control in tests
class MockIntersectionObserver {
  static instances = [];
  constructor(callback, options) {
    this._callback = callback;
    this._options = options;
    this.elements = new Set();
    MockIntersectionObserver.instances.push(this);
  }
  observe(el) { this.elements.add(el); }
  unobserve(el) { this.elements.delete(el); }
  disconnect() { this.elements.clear(); }
  // helper to trigger intersections
  trigger(isIntersecting = true, ratio = 1) {
    const entries = [...this.elements].map(el => ({
      isIntersecting,
      intersectionRatio: ratio,
      target: el
    }));
    this._callback(entries, this);
  }
}

global.IntersectionObserver = MockIntersectionObserver;

describe('TnTAnimation custom element', () => {
  beforeEach(() => {
    document.body.innerHTML = '';
    MockIntersectionObserver.instances.length = 0;
  });

  test('registers custom element via onLoad', () => {
    onLoad();
    expect(customElements.get('tnt-animation')).toBe(TnTAnimation);
  });

  test('creates observer when custom attribute is set', () => {
    onLoad();
    const el = document.createElement('tnt-animation');
    document.body.appendChild(el);

    // attribute change triggers attributeChangedCallback
    el.setAttribute(global.NTComponents.customAttribute, 'x1');

    expect(MockIntersectionObserver.instances.length).toBe(1);
    expect(MockIntersectionObserver.instances[0]._options.threshold).toBe(0.5); // default
  });

  test('uses provided threshold attribute', () => {
    onLoad();
    const el = document.createElement('tnt-animation');
    el.setAttribute('tnt-threshold', '0.25');
    document.body.appendChild(el);
    el.setAttribute(global.NTComponents.customAttribute, 'x2');

    expect(MockIntersectionObserver.instances[0]._options.threshold).toBe(0.25);
  });

  test('adds class and unobserves on intersection', () => {
    onLoad();
    const el = document.createElement('tnt-animation');
    document.body.appendChild(el);
    el.setAttribute(global.NTComponents.customAttribute, 'x3');

    const observer = MockIntersectionObserver.instances[0];
    expect(observer.elements.has(el)).toBe(true);

    // simulate intersection
    observer.trigger(true, 0.75);

    expect(el.classList.contains('tnt-animation-intersected')).toBe(true);
    expect(observer.elements.has(el)).toBe(false); // unobserved after intersecting
  });

  test('disconnectedCallback disconnects observer', () => {
    onLoad();
    const el = document.createElement('tnt-animation');
    document.body.appendChild(el);
    el.setAttribute(global.NTComponents.customAttribute, 'x4');

    const observer = MockIntersectionObserver.instances[0];
    expect(observer.elements.size).toBe(1);

    el.remove(); // triggers disconnectedCallback

    expect(observer.elements.size).toBe(0);
  });

  // Additional tests
  test('does not create multiple observers on subsequent attribute changes', () => {
    onLoad();
    const el = document.createElement('tnt-animation');
    document.body.appendChild(el);
    el.setAttribute(global.NTComponents.customAttribute, 'first');
    expect(MockIntersectionObserver.instances.length).toBe(1);
    const firstInstance = MockIntersectionObserver.instances[0];

    // trigger attributeChangedCallback again
    el.setAttribute(global.NTComponents.customAttribute, 'second');
    expect(MockIntersectionObserver.instances.length).toBe(1);
    expect(MockIntersectionObserver.instances[0]).toBe(firstInstance);
  });

  test('non-intersecting entries do not add class or unobserve', () => {
    onLoad();
    const el = document.createElement('tnt-animation');
    document.body.appendChild(el);
    el.setAttribute(global.NTComponents.customAttribute, 'x5');
    const observer = MockIntersectionObserver.instances[0];

    observer.trigger(false, 0.3); // not intersecting

    expect(el.classList.contains('tnt-animation-intersected')).toBe(false);
    expect(observer.elements.has(el)).toBe(true); // still observed
  });

  test('invalid threshold value falls back to default', () => {
    onLoad();
    const el = document.createElement('tnt-animation');
    el.setAttribute('tnt-threshold', 'not-a-number');
    document.body.appendChild(el);
    el.setAttribute(global.NTComponents.customAttribute, 'x6');

    expect(MockIntersectionObserver.instances[0]._options.threshold).toBe(0.5); // default fallback
  });
});
