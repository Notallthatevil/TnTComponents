import { jest } from '@jest/globals';
import '../../TnTComponents/wwwroot/TnTComponents.lib.module.js';

describe('TnTComponents.toggleSideNavGroup', () => {
   beforeEach(() => {
      document.body.innerHTML = '';
      jest.clearAllMocks();
   });

   test('toggles tnt-toggle class on side nav menu group toggler', () => {
      const parent = document.createElement('div');
      const toggler = document.createElement('div');
      toggler.className = 'tnt-side-nav-menu-group-toggler tnt-toggle';

      parent.appendChild(toggler);

      const target = document.createElement('div');
      parent.appendChild(target);
      document.body.appendChild(parent);

      const event = new Event('click');
      Object.defineProperty(event, 'target', { value: target, configurable: true });

      TnTComponents.toggleSideNavGroup(event);

      expect(toggler.classList.contains('tnt-toggle')).toBe(false);
   });

   test('adds tnt-toggle class if not present', () => {
      const parent = document.createElement('div');
      const toggler = document.createElement('div');
      toggler.className = 'tnt-side-nav-menu-group-toggler';

      parent.appendChild(toggler);

      const target = document.createElement('div');
      parent.appendChild(target);
      document.body.appendChild(parent);

      const event = new Event('click');
      Object.defineProperty(event, 'target', { value: target, configurable: true });

      TnTComponents.toggleSideNavGroup(event);

      expect(toggler.classList.contains('tnt-toggle')).toBe(true);
   });

   test('handles null toggler gracefully', () => {
      const parent = document.createElement('div');
      const target = document.createElement('div');
      parent.appendChild(target);
      document.body.appendChild(parent);

      const event = new Event('click');
      Object.defineProperty(event, 'target', { value: target, configurable: true });

      expect(() => TnTComponents.toggleSideNavGroup(event)).not.toThrow();
   });
});
