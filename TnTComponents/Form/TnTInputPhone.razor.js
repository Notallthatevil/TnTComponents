function validatePhoneInput(e) {
    if (e && e.target) {
        const oldValue = e.target.value;
        const regex = new RegExp(e.target.getAttribute('pattern'), 'g');

        setTimeout(function () {
            const newValue = e.target.value;
            if (!regex.test(newValue) && newValue !== '' && newValue !== '(') {
                e.target.value = oldValue;
            }
        }, 1);
    }
}

function formatPhoneValue(e) {
    if (e && e.target && e.target.value && e.target.value.length > 0) {
        var cleaned = ('' + e.target.value).replace(/\D/g, '');
        var match = cleaned.match(/^(\d{3})(\d{3})(\d{4})$/);
        if (match) {
            e.target.value = '(' + match[1] + ') ' + match[2] + '-' + match[3];
        }
        else {
            e.target.value = null;
        }
    }
}

export function onLoad(element, dotNetRef) {
    var currencyInputs = document.querySelectorAll('input[tnt-input-phone]');
    for (var i = 0; i < currencyInputs.length; i++) {
        formatPhoneValue({ target: currencyInputs[i] });
    }
}

export function onUpdate(element, dotNetRef) {
    var currencyInputs = document.querySelectorAll('input[tnt-input-phone]');
    for (var i = 0; i < currencyInputs.length; i++) {
        currencyInputs[i].addEventListener('keydown', validatePhoneInput);
        currencyInputs[i].addEventListener('blur', formatPhoneValue);
    }
}

export function onDispose(element, dotNetRef) {
}