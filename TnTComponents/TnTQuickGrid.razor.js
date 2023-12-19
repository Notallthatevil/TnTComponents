export function tntInitResizable(tableElement) {
    const createResizableTable = function (table) {
        const cols = table.querySelectorAll('th');
        [].forEach.call(cols, function (col) {
            // Add a resizer element to the column
            let resizer = col.querySelector('.tnt-resizeable');
            if (!resizer) {
                const resizer = document.createElement('div');
                resizer.classList.add('tnt-resizeable');

                // Set the height
                resizer.style.height = '100%';

                col.appendChild(resizer);

                createResizableColumn(col, resizer);
            }
        });
    };

    const createResizableColumn = function (col, resizer) {
        let x = 0;
        let w = 0;

        const mouseDownHandler = function (e) {
            x = e.clientX;

            const styles = window.getComputedStyle(col);
            w = parseInt(styles.width, 10);

            document.addEventListener('mousemove', mouseMoveHandler);
            document.addEventListener('mouseup', mouseUpHandler);

            resizer.classList.add('tnt-resizing');
        };

        const mouseMoveHandler = function (e) {
            const dx = e.clientX - x;
            col.style.minWidth = (w + dx) + 'px';
        };

        const mouseUpHandler = function () {
            resizer.classList.remove('tnt-resizing');
            document.removeEventListener('mousemove', mouseMoveHandler);
            document.removeEventListener('mouseup', mouseUpHandler);
        };

        resizer.addEventListener('mousedown', mouseDownHandler);
    };

    if (!tableElement.classList.contains('compact')) {
        createResizableTable(tableElement);
    }
}

export function onLoad() {
}

export function onUpdate() {
    for (const table of [...document.querySelectorAll('table.tnt-resizable')]) {
        tntInitResizable(table);
    }
}

export function onDispose() { }