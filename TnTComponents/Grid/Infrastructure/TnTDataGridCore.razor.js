export function onUpdate() {
    const url = new URL(import.meta.url);
    const dataGridIdentifier = url.searchParams.get("tntdatagrididentifier");

    if (dataGridIdentifier) {
        var dataGrid = document.querySelector(`[${dataGridIdentifier}]`);

        if (dataGrid) {
            TnTComponents.registerTnTDataGrid(dataGrid);
        }
    }
}