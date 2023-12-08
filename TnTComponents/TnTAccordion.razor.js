export function onUpdate() {
    const url = new URL(import.meta.url);
    const headerIdentifier = url.searchParams.get('tntheaderidentifier');
    const contentIdentifier = url.searchParams.get('tntcontentidentifier');

    if (headerIdentifier && contentIdentifier) {
        let header = document.querySelector(`[${headerIdentifier}]`);
        let content = document.querySelector(`[${contentIdentifier}]`);

        if (header && content) {
            header.addEventListener('click', (e) => {
                if (content.clientHeight) {
                    content.style.height = 0;
                    content.classList.remove('visible');
                }
                else {
                    content.style.height = `${content.firstChild.clientHeight}px`;
                    content.classList.add('visible');
                }
            })
        }
    }
}