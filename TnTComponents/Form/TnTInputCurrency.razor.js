function validateCurrencyInput(e) {
    if (e && e.target) {
        const oldValue = e.target.value;
        const regex = new RegExp(e.target.getAttribute('pattern'), 'g');

        setTimeout(function () {
            const newValue = e.target.value;
            if (!regex.test(newValue) && newValue !== '' && newValue !== '$') {
                e.target.value = oldValue;
            }
        }, 1);
    }
}

function formatCurrencyValue(e) {
    if (e && e.target && e.target.value && e.target.value.length > 0) {
        const numberRegex = new RegExp('[0-9.]', 'g');
        let numbers = '';
        let result;
        while ((result = numberRegex.exec(e.target.value)) != null) {
            numbers += result.toString();
        }

        // Create our number formatter.
        const formatter = new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD',
        });

        e.target.value = formatter.format(numbers);
    }
}

export function onLoad(element, dotNetRef) {
}

export function onUpdate(element, dotNetRef) {
    var currencyInputs = document.querySelectorAll('input[tnt-input-currency]');
    for (var i = 0; i < currencyInputs.length; i++) {
        currencyInputs[i].addEventListener('keydown', validateCurrencyInput);
        currencyInputs[i].addEventListener('blur', formatCurrencyValue);
    }
}

export function onDispose(element, dotNetRef) {
}