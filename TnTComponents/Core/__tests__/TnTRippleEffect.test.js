import { jest } from '@jest/globals';
import { TnTRippleEffect, onLoad, onUpdate } from '../TnTRippleEffect.razor.js';

if (!global.TnTComponents) {
  global.TnTComponents = { customAttribute: 'tnt-id' };
}

describe('TnTRippleEffect web component', () => {
  beforeEach(() => {
    document.body.innerHTML = '';
    jest.useFakeTimers();
  });

  afterEach(() => {
    jest.runOnlyPendingTimers();
    jest.useRealTimers();
  });

  test('onLoad registers custom element only once', () => {
    const defineSpy = jest.spyOn(customElements, 'define');
    const el = document.createElement('tnt-ripple-effect');
    onLoad(el, null);
    onLoad(el, null); // second call should not re-define
    expect(defineSpy).toHaveBeenCalledTimes(1);
    defineSpy.mockRestore();
  });

  test('attributeChangedCallback tracks identifiers and triggers update', () => {
    onLoad(null, null);
    const el = new TnTRippleEffect();
    const parent = document.createElement('button');
    parent.appendChild(el);
    document.body.appendChild(parent);
    const updateSpy = jest.spyOn(el, 'update');
    el.setAttribute(TnTComponents.customAttribute, 'id1');
    el.setAttribute(TnTComponents.customAttribute, 'id2');
    expect(updateSpy).toHaveBeenCalledTimes(2);
  });

  test('update wires mouse event listeners on parent', () => {
    onLoad(null, null);
    const el = new TnTRippleEffect();
    const parent = document.createElement('div');
    parent.appendChild(el);
    document.body.appendChild(parent);
    const addSpy = jest.spyOn(parent, 'addEventListener');
    el.update();
    expect(addSpy).toHaveBeenCalledWith('mousedown', expect.any(Function));
    expect(addSpy).toHaveBeenCalledWith('mouseup', expect.any(Function));
  });

  test('mousedown then mouseup creates and then schedules removal of ripple elements', () => {
    onLoad(null, null);
    const el = new TnTRippleEffect();
    const parent = document.createElement('div');
    parent.style.position = 'relative';
    parent.style.width = '100px';
    parent.style.height = '40px';
    parent.appendChild(el);
    document.body.appendChild(parent);
    el.update();
    parent.dispatchEvent(new MouseEvent('mousedown', { bubbles: true, clientX: 10, clientY: 10, pageX: 10, pageY: 10 }));
    let ripple = el.querySelector('.tnt-rippling');
    expect(ripple).not.toBeNull();
    parent.dispatchEvent(new MouseEvent('mouseup', { bubbles: true, clientX: 10, clientY: 10, pageX: 10, pageY: 10 }));
    ripple = el.querySelector('.tnt-rippling');
    expect(ripple.classList.contains('tnt-rippling-mouse-up')).toBe(true);
    jest.advanceTimersByTime(610);
    expect(el.querySelector('.tnt-rippling')).toBeNull();
  });

  test('mouseleave after mousedown also triggers cleanup path', () => {
    onLoad(null, null);
    const el = new TnTRippleEffect();
    const parent = document.createElement('div');
    parent.appendChild(el);
    document.body.appendChild(parent);
    el.update();
    parent.dispatchEvent(new MouseEvent('mousedown', { bubbles: true, pageX: 5, pageY: 5 }));
    expect(el.querySelectorAll('.tnt-rippling').length).toBe(1);
    parent.dispatchEvent(new MouseEvent('mouseleave', { bubbles: true }));
    const ripples = el.querySelectorAll('.tnt-rippling');
    ripples.forEach(r => expect(r.classList.contains('tnt-rippling-mouse-up')).toBe(true));
    jest.advanceTimersByTime(610);
    expect(el.querySelector('.tnt-rippling')).toBeNull();
  });

  test('rapid multiple mousedown events create multiple ripple elements that are all cleaned', () => {
    onLoad(null, null);
    const el = new TnTRippleEffect();
    const parent = document.createElement('div');
    parent.style.width = '50px';
    parent.style.height = '50px';
    parent.appendChild(el);
    document.body.appendChild(parent);
    el.update();
    for (let i = 0; i < 3; i++) {
      parent.dispatchEvent(new MouseEvent('mousedown', { bubbles: true, pageX: 5 + i, pageY: 5 + i }));
    }
    expect(el.querySelectorAll('.tnt-rippling').length).toBe(3);
    parent.dispatchEvent(new MouseEvent('mouseup', { bubbles: true }));
    jest.advanceTimersByTime(610);
    expect(el.querySelectorAll('.tnt-rippling').length).toBe(0);
  });

  test('ripple sets CSS variable --tnt-ripple-max-size relative to largest dimension', () => {
    onLoad(null, null);
    const el = new TnTRippleEffect();
    const parent = document.createElement('div');
    // JSDOM does not compute layout; mock offset dimensions explicitly
    Object.defineProperty(parent, 'offsetWidth', { value: 80, configurable: true });
    Object.defineProperty(parent, 'offsetHeight', { value: 20, configurable: true });
    parent.appendChild(el);
    document.body.appendChild(parent);
    el.update();
    parent.dispatchEvent(new MouseEvent('mousedown', { bubbles: true, pageX: 1, pageY: 1 }));
    const styleAttr = el.getAttribute('style');
    expect(styleAttr).toMatch(/--tnt-ripple-max-size: 200/);
  });

  test('special case: mousedown on child inside a tnt-data-grid-body-row uses currentTarget dimensions', () => {
    onLoad(null, null);
    const el = new TnTRippleEffect();
    const row = document.createElement('tr');
    row.classList.add('tnt-data-grid-body-row');

    // Mock offsets on the row (currentTarget)
    Object.defineProperty(row, 'offsetWidth', { value: 120, configurable: true });
    Object.defineProperty(row, 'offsetHeight', { value: 30, configurable: true });

    // create a child cell which will be the event.target
    const cell = document.createElement('td');
    cell.textContent = 'cell';

    row.appendChild(cell);
    row.appendChild(el); // ripple element is direct child of row (scope > tnt-ripple-effect)
    document.body.appendChild(row);

    el.update();

    // Dispatch mousedown on the child cell; rippleEffect should detect the special case and use row dims
    cell.dispatchEvent(new MouseEvent('mousedown', { bubbles: true, pageX: 10, pageY: 10 }));

    const ripple = el.querySelector('.tnt-rippling');
    expect(ripple).not.toBeNull();

    // The ripple max size should be based on the larger of offsetWidth/offsetHeight (120) * 2.5 = 300
    const styleAttr = el.getAttribute('style') || '';
    expect(styleAttr).toMatch(/--tnt-ripple-max-size: 300/);
  });

  test('onUpdate calls element.update when available', () => {
    const mock = { update: jest.fn() };
    onUpdate(mock, null);
    expect(mock.update).toHaveBeenCalled();
  });

  test('onUpdate safe when element is null or has no update', () => {
    expect(() => onUpdate(null, null)).not.toThrow();
    expect(() => onUpdate({}, null)).not.toThrow();
  });

  test('disconnectedCallback removes identifier mapping', () => {
    onLoad(null, null);
    const el = new TnTRippleEffect();
    const parent = document.createElement('div');
    parent.appendChild(el);
    document.body.appendChild(parent);
    el.setAttribute(TnTComponents.customAttribute, 'abc');
    const spy = jest.spyOn(Map.prototype, 'delete');
    el.disconnectedCallback();
    expect(spy).toHaveBeenCalled();
    spy.mockRestore();
  });

  test('ignores events if no <tnt-ripple-effect> child present', () => {
    onLoad(null, null);
    const parent = document.createElement('div');
    document.body.appendChild(parent);
    parent.dispatchEvent(new MouseEvent('mousedown', { bubbles: true, pageX: 5, pageY: 5 }));
    expect(parent.querySelector('.tnt-rippling')).toBeNull();
  });

  test('early return when event target lacks querySelector', () => {
    const el = new TnTRippleEffect();
    const parent = document.createElement('div');
    parent.appendChild(el);
    document.body.appendChild(parent);
    el.update();
    const evt = new Event('mousedown', { bubbles: true });
    Object.defineProperty(evt, 'target', { value: { } });
    parent.dispatchEvent(evt);
    expect(el.querySelector('.tnt-rippling')).toBeNull();
  });
});
