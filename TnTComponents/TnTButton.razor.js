//export function onLoad() {
//  console.log('Loaded');
//}

export function onUpdate() {
    document.addEventListener('click', function (e) {
        const btn = e.target.closest('button');
        if (btn) {
            TnTComponents.ripple(btn, e);
        }
    });
}

//export function onDispose() {
//  console.log('Disposed');
//}