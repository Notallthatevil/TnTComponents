import { jest } from '@jest/globals';
import '../../NTComponents/wwwroot/NTComponents.lib.module.js';

describe('NTComponents.toggleAccordionHeader', () => {
   beforeEach(() => {
      document.body.innerHTML = '';
      jest.clearAllMocks();
   });

   test('toggles expanded class on accordion content', () => {
      const accordion = document.createElement('tnt-accordion');
      const itemContainer = document.createElement('div');
      const header = document.createElement('h3');
      const content = document.createElement('div');
      content.className = 'tnt-collapsed';

      // Build the structure: accordion > itemContainer > [header, content]
      itemContainer.appendChild(header);
      itemContainer.appendChild(content);
      accordion.appendChild(itemContainer);
      document.body.appendChild(accordion);

      // Mock accordion methods
      accordion.limitToOneExpanded = jest.fn(() => false);
      accordion.closeChildren = jest.fn();
      accordion.updateChild = jest.fn();
      accordion.resetChildren = jest.fn();

      const event = new Event('click', { bubbles: true });
      Object.defineProperty(event, 'target', { value: header, configurable: true });

      NTComponents.toggleAccordionHeader(event);

      // After the toggle, content should be expanded (class removed from collapsed, added to expanded)
      expect(content.classList.contains('tnt-expanded')).toBe(true);
      expect(accordion.updateChild).toHaveBeenCalledWith(content);
   });
});
