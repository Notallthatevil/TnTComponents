import { jest } from '@jest/globals';
import '../../TnTComponents/wwwroot/TnTComponents.lib.module.js';

describe('TnTComponents.getColorValueFromEnumName', () => {
   beforeEach(() => {
      document.body.innerHTML = '';
      jest.clearAllMocks();
      document.documentElement.style.cssText = '';
   });

   test('returns null for null or non-string input', () => {
      expect(TnTComponents.getColorValueFromEnumName(null)).toBeNull();
      expect(TnTComponents.getColorValueFromEnumName(undefined)).toBeNull();
      expect(TnTComponents.getColorValueFromEnumName(123)).toBeNull();
   });

   test('converts PascalCase color name to kebab-case CSS variable', () => {
      document.documentElement.style.setProperty('--tnt-color-primary', '#ff0000');
      const result = TnTComponents.getColorValueFromEnumName('Primary');
      expect(result).toBe('#ff0000');
   });

   test('converts complex camelCase color names correctly', () => {
      document.documentElement.style.setProperty('--tnt-color-on-primary-container', '#00ff00');
      const result = TnTComponents.getColorValueFromEnumName('OnPrimaryContainer');
      expect(result).toBe('#00ff00');
   });

   test('returns null for non-existent color variable', () => {
      const result = TnTComponents.getColorValueFromEnumName('NonExistentColor');
      expect(result).toBeNull();
   });

   test('converts rgb format to hex', () => {
      document.documentElement.style.setProperty('--tnt-color-test', 'rgb(255, 0, 0)');
      const result = TnTComponents.getColorValueFromEnumName('Test');
      expect(result).toBe('#ff0000');
   });

   test('converts rgb with spaces to hex', () => {
      document.documentElement.style.setProperty('--tnt-color-test', 'rgb(0 255 0)');
      const result = TnTComponents.getColorValueFromEnumName('Test');
      expect(result).toBe('#00ff00');
   });

   test('converts rgba format to hex (ignoring alpha)', () => {
      document.documentElement.style.setProperty('--tnt-color-test', 'rgba(0, 0, 255, 0.5)');
      const result = TnTComponents.getColorValueFromEnumName('Test');
      expect(result).toBe('#0000ff');
   });

   test('returns original value if not rgb/rgba format', () => {
      document.documentElement.style.setProperty('--tnt-color-test', '#abcdef');
      const result = TnTComponents.getColorValueFromEnumName('Test');
      expect(result).toBe('#abcdef');
   });
});
