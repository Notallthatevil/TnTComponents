import { jest } from '@jest/globals';
import '../../TnTComponents/wwwroot/TnTComponents.lib.module.js';

describe('TnTComponents.enableRipple', () => {
   beforeEach(() => {
      document.body.innerHTML = '';
      jest.clearAllMocks();
   });

   test('adds click event listener to element', () => {
      const element = document.createElement('button');
      const addEventListenerSpy = jest.spyOn(element, 'addEventListener');

      TnTComponents.enableRipple(element);

      expect(addEventListenerSpy).toHaveBeenCalledWith('click', expect.any(Function));
   });

   test('sets ripple offset CSS variables on click', () => {
      const button = document.createElement('button');
      button.style.position = 'relative';
      button.style.width = '100px';
      button.style.height = '100px';
      document.body.appendChild(button);

      TnTComponents.enableRipple(button);

      const clickEvent = new MouseEvent('click', {
         clientX: 50,
         clientY: 50,
         bubbles: true,
      });

      button.getBoundingClientRect = jest.fn(() => ({
         left: 10,
         top: 10,
         width: 100,
         height: 100,
      }));

      button.dispatchEvent(clickEvent);

      const offsetX = button.style.getPropertyValue('--ripple-offset-x');
      const offsetY = button.style.getPropertyValue('--ripple-offset-y');

      expect(offsetX).toBe('-10px');
      expect(offsetY).toBe('-10px');
   });

   test('handles null element gracefully', () => {
      expect(() => TnTComponents.enableRipple(null)).not.toThrow();
   });
});
