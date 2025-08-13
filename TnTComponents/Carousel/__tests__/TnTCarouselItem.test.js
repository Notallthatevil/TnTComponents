import { TnTCarouselItem } from '../TnTCarouselItem.razor.js';

// Mock Image so onload triggers immediately with a width
class ImageMock {
  constructor(){
    setTimeout(() => { if(this.onload){ this.naturalWidth = 320; this.onload(); } }, 0);
  }
  set src(v){ this._src=v; }
  get src(){ return this._src; }
}

global.Image = ImageMock;

describe('TnTCarouselItem', () => {
  beforeAll(() => {
    if(!customElements.get('tnt-carousel-item')) {
      customElements.define('tnt-carousel-item', TnTCarouselItem);
    }
  });

  function buildDom({ hero=false, bgUrl=null }={}) {
    const viewport = document.createElement('div');
    viewport.className='tnt-carousel-viewport';
    Object.defineProperty(viewport, 'getBoundingClientRect', { value: () => ({ left:0, right:500, width:500 }), configurable: true });

    const item = document.createElement('tnt-carousel-item');
    if(hero) item.classList.add('tnt-carousel-hero');

    const content = document.createElement('div');
    content.className='tnt-carousel-item-content';
    if(bgUrl){ content.style.backgroundImage = `url('${bgUrl}')`; }
    Object.defineProperty(item, 'getBoundingClientRect', { value: () => ({ left:0, right:300, width:300 }), configurable: true });

    item.appendChild(content);
    viewport.appendChild(item);
    document.body.appendChild(viewport);
    return { item, content, viewport };
  }

  test('upgrades to custom element', () => {
    const el = document.createElement('tnt-carousel-item');
    expect(el instanceof TnTCarouselItem).toBe(true);
  });

  test('hero item sets backgroundImageWidth 100% and computes width via update', () => {
    const { item } = buildDom({ hero:true });
    item.onUpdate();
    expect(item.backgroundImageWidth).toBe('100%');
    expect(item.style.width).toBe('300px');
  });

  test('non-hero with no background uses computed width', () => {
    const { item } = buildDom();
    item.onUpdate();
    expect(item.style.width).toBe('300px');
  });

  test('background image triggers natural width calculation', async () => {
    const { item } = buildDom({ bgUrl:'test.png' });
    item.onUpdate();
    // Wait microtask queue for mock image
    await new Promise(r => setTimeout(r,10));
    expect(item.naturalWidth).toBe(320);
    expect(item.style.getPropertyValue('--tnt-carousel-item-bg-width')).toBe('320px');
  });
});
