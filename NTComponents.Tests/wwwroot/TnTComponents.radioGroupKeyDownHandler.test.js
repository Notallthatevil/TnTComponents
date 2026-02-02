import { jest } from '@jest/globals';
import '../../NTComponents/wwwroot/NTComponents.lib.module.js';

describe('NTComponents.radioGroupKeyDownHandler', () => {
    let fieldset;
    let radio1;
    let radio2;
    let radio3;

    beforeEach(() => {
        document.body.innerHTML = `
            <fieldset class="tnt-radio-group" id="group1">
                <input type="radio" id="r1" value="v1" name="g1">
                <input type="radio" id="r2" value="v2" name="g1">
                <input type="radio" id="r3" value="v3" name="g1">
            </fieldset>
        `;
        fieldset = document.getElementById('group1');
        radio1 = document.getElementById('r1');
        radio2 = document.getElementById('r2');
        radio3 = document.getElementById('r3');
    });

    test('selects first radio when key "1" is pressed', () => {
        const event = new KeyboardEvent('keydown', { key: '1', bubbles: true });
        // We need to mock currentTarget because we're calling the handler directly as if it were an event listener
        Object.defineProperty(event, 'currentTarget', { value: fieldset });

        NTComponents.radioGroupKeyDownHandler(event);

        expect(radio1.checked).toBe(true);
        expect(radio2.checked).toBe(false);
        expect(radio3.checked).toBe(false);
    });

    test('selects second radio when key "2" is pressed', () => {
        const event = new KeyboardEvent('keydown', { key: '2', bubbles: true });
        Object.defineProperty(event, 'currentTarget', { value: fieldset });

        NTComponents.radioGroupKeyDownHandler(event);

        expect(radio2.checked).toBe(true);
    });

    test('does nothing when key is not a number', () => {
        const event = new KeyboardEvent('keydown', { key: 'a', bubbles: true });
        Object.defineProperty(event, 'currentTarget', { value: fieldset });

        NTComponents.radioGroupKeyDownHandler(event);

        expect(radio1.checked).toBe(false);
        expect(radio2.checked).toBe(false);
        expect(radio3.checked).toBe(false);
    });

    test('does nothing when index is out of range', () => {
        const event = new KeyboardEvent('keydown', { key: '5', bubbles: true });
        Object.defineProperty(event, 'currentTarget', { value: fieldset });

        NTComponents.radioGroupKeyDownHandler(event);

        expect(radio1.checked).toBe(false);
        expect(radio2.checked).toBe(false);
        expect(radio3.checked).toBe(false);
    });

    test('does not select disabled radio', () => {
        radio2.disabled = true;
        const event = new KeyboardEvent('keydown', { key: '2', bubbles: true });
        Object.defineProperty(event, 'currentTarget', { value: fieldset });

        NTComponents.radioGroupKeyDownHandler(event);

        expect(radio2.checked).toBe(false);
    });

    test('does not select when group fieldset is disabled', () => {
        fieldset.disabled = true;
        const event = new KeyboardEvent('keydown', { key: '1', bubbles: true });
        Object.defineProperty(event, 'currentTarget', { value: fieldset });

        NTComponents.radioGroupKeyDownHandler(event);

        expect(radio1.checked).toBe(false);
    });

    test('does not select when group has tnt-disabled class', () => {
        fieldset.classList.add('tnt-disabled');
        const event = new KeyboardEvent('keydown', { key: '1', bubbles: true });
        Object.defineProperty(event, 'currentTarget', { value: fieldset });

        NTComponents.radioGroupKeyDownHandler(event);

        expect(radio1.checked).toBe(false);
    });

    test('does not select when group has tnt-readonly class', () => {
        fieldset.classList.add('tnt-readonly');
        const event = new KeyboardEvent('keydown', { key: '1', bubbles: true });
        Object.defineProperty(event, 'currentTarget', { value: fieldset });

        NTComponents.radioGroupKeyDownHandler(event);

        expect(radio1.checked).toBe(false);
    });

    test('prevents default behavior when a valid radio is selected', () => {
        const event = new KeyboardEvent('keydown', { key: '1', bubbles: true });
        Object.defineProperty(event, 'currentTarget', { value: fieldset });
        const preventDefaultSpy = jest.spyOn(event, 'preventDefault');

        NTComponents.radioGroupKeyDownHandler(event);

        expect(preventDefaultSpy).toHaveBeenCalled();
    });
});
