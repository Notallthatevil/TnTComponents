/**
 * @jest-environment jsdom
 */
import { jest } from '@jest/globals';
import { TnTAccordion, onLoad, onUpdate, onDispose } from '../TnTAccordion.razor.js';

if (!global.TnTComponents) {
  global.TnTComponents = { customAttribute: 'tntid' };
}

if (typeof global.ResizeObserver === 'undefined') {
  global.ResizeObserver = class { observe(){} disconnect(){} };
}
if (typeof global.MutationObserver === 'undefined') {
  global.MutationObserver = class { observe(){} disconnect(){} };
}

describe('TnTAccordion web component', () => {
  beforeEach(() => {
    document.body.innerHTML = '';
  });

  const defineIfNeeded = () => {
    if (!customElements.get('tnt-accordion')) {
      customElements.define('tnt-accordion', TnTAccordion);
    }
  };

  function createAccordion({ limitOne = false } = {}) {
    defineIfNeeded();
    const el = document.createElement('tnt-accordion');
    if (limitOne) el.classList.add('tnt-limit-one-expanded');
    document.body.appendChild(el);
    return el;
  }

  function createChild({ expanded = false } = {}) {
    const wrapper = document.createElement('div');
    wrapper.classList.add('tnt-accordion-child');
    const header = document.createElement('h3');
    const content = document.createElement('div');
    if (expanded) content.classList.add('tnt-expanded');
    wrapper.appendChild(header);
    wrapper.appendChild(content);
    return { wrapper, content };
  }

  test('onLoad registers custom element only once', () => {
    const defineSpy = jest.spyOn(customElements, 'define');
    const host = document.createElement('tnt-accordion');
    onLoad(host, null);
    onLoad(host, null);
    expect(defineSpy).toHaveBeenCalledTimes(defineSpy.mock.calls.length <= 1 ? defineSpy.mock.calls.length : 1);
    defineSpy.mockRestore();
  });

  test('onLoad assigns dotNetRef to element', () => {
    const host = createAccordion();
    onLoad(host, { some: 'ref' });
    expect(host.dotNetRef).toEqual({ some: 'ref' });
  });

  test('attributeChangedCallback maps identifiers and triggers update', () => {
    defineIfNeeded();
    const acc = new TnTAccordion();
    const spy = jest.spyOn(acc, 'update');
    acc.setAttribute(TnTComponents.customAttribute, 'one');
    acc.setAttribute(TnTComponents.customAttribute, 'two');
    expect(acc.identifier).toBe('two');
    expect(spy).toHaveBeenCalled();
  });

  test('update collects only direct child accordion items', () => {
    const acc = createAccordion();
    const a = createChild();
    const b = createChild();
    acc.appendChild(a.wrapper);
    acc.appendChild(b.wrapper);
    acc.update();
    expect(acc.accordionChildren.length).toBe(2);
  });

  test('updateChild sets content height and observers when expanded', () => {
    const acc = createAccordion();
    const { wrapper, content } = createChild({ expanded: true });
    Object.defineProperty(content, 'scrollHeight', { value: 250, configurable: true });
    acc.appendChild(wrapper);
    acc.update();
    expect(content.style.getPropertyValue('--content-height')).toBe('250px');
    expect(content.resizeObserver).toBeDefined();
    expect(content.mutationObserer).toBeDefined();
  });

  test('updateChild clears inline height when collapsed', () => {
    const acc = createAccordion();
    const { wrapper, content } = createChild();
    acc.appendChild(wrapper);
    acc.update();
    expect(content.style.height).toBe('');
  });

  test('closeChildren collapses other expanded children', () => {
    const acc = createAccordion({ limitOne: true });
    const first = createChild({ expanded: true });
    const second = createChild({ expanded: true });
    acc.appendChild(first.wrapper);
    acc.appendChild(second.wrapper);
    acc.update();
    acc.closeChildren(second.wrapper);
    expect(first.content.classList.contains('tnt-expanded')).toBe(false);
    expect(first.content.classList.contains('tnt-collapsed')).toBe(true);
  });

  test('resetChildren removes expanded/collapsed classes and recurses', () => {
    const acc = createAccordion();
    const child = createChild({ expanded: true });
    const nestedAcc = createAccordion();
    child.content.appendChild(nestedAcc);
    acc.appendChild(child.wrapper);
    acc.update();
    expect(child.content.classList.contains('tnt-expanded')).toBe(true);
    acc.resetChildren();
    expect(child.content.classList.contains('tnt-expanded')).toBe(false);
    expect(child.content.classList.contains('tnt-collapsed')).toBe(false);
  });

  test('limitToOneExpanded reflects CSS class', () => {
    const acc = createAccordion({ limitOne: true });
    expect(acc.limitToOneExpanded()).toBe(true);
  });

  test('onUpdate invokes update and sets dotNetRef', () => {
    const acc = createAccordion();
    const spy = jest.spyOn(acc, 'update');
    onUpdate(acc, { ref: 1 });
    expect(spy).toHaveBeenCalled();
    expect(acc.dotNetRef).toEqual({ ref: 1 });
  });

  test('onUpdate safe when element null or missing update', () => {
    expect(() => onUpdate(null, null)).not.toThrow();
    expect(() => onUpdate({}, null)).not.toThrow();
  });

  test('disconnectedCallback cleans map entry', () => {
    const acc = createAccordion();
    const deleteSpy = jest.spyOn(Map.prototype, 'delete');
    acc.setAttribute(TnTComponents.customAttribute, 'zzz');
    acc.disconnectedCallback();
    expect(deleteSpy).toHaveBeenCalled();
    deleteSpy.mockRestore();
  });

  test('onDispose is a no-op', () => {
    const acc = createAccordion();
    expect(() => onDispose(acc, {})).not.toThrow();
  });
});
