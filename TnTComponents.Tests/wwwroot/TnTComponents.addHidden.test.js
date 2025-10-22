import { jest } from '@jest/globals';
import '../../TnTComponents/wwwroot/TnTComponents.lib.module.js';

describe('TnTComponents.addHidden', () => {
   beforeEach(() => {
      document.body.innerHTML = '';
      jest.clearAllMocks();
   });

   test('adds tnt-hidden class to element', () => {
      const element = document.createElement('div');
      TnTComponents.addHidden(element);
      expect(element.classList.contains('tnt-hidden')).toBe(true);
   });

   test('does not add class if element is null', () => {
      expect(() => TnTComponents.addHidden(null)).not.toThrow();
   });

   test('does not add class if element lacks classList', () => {
      expect(() => TnTComponents.addHidden({})).not.toThrow();
   });

   test('does not add class twice to same element', () => {
      const element = document.createElement('div');
      TnTComponents.addHidden(element);
      TnTComponents.addHidden(element);
      const count = Array.from(element.classList).filter(c => c === 'tnt-hidden').length;
      expect(count).toBe(1);
   });
});
