
function positionEvent(event, columnHeaders, timeSlots) {
    const startTime = new Date(event.getAttribute('tnt-start-time'));
    const day = startTime.getDay();

    let roundedStartTime = startTime;
    roundedStartTime.setMinutes(Math.floor(startTime.getMinutes() / 30) * 30);
    const roundedStartTimeString = `${String(roundedStartTime.getHours()).padStart(2, '0')}:${String(roundedStartTime.getMinutes()).padStart(2, '0')}`;

    const column = Array.from(columnHeaders)
        .find(c => c.getAttribute('tnt-day-of-week') == day);

    const row = Array.from(timeSlots)
        .find(c => c.getAttribute('tnt-time') == roundedStartTimeString);

    const columnLeft = column.getBoundingClientRect().left;
    const rowTop = row.getBoundingClientRect().top;

    event.style.left = `${columnLeft}px`;
    event.style.top = `${rowTop}px`;
    event.style.width = `${30}px`;
    event.style.height = `${30}px`;
}

function updateEvents(element, dotNetElementRef) {
    const events = element.querySelectorAll('.tnt-event');
    const columnHeaders = element.querySelectorAll('th.tnt-day-slot');
    const timeSlots = element.querySelectorAll('td.tnt-time-slot');
    events.forEach(event => {
        positionEvent(event, columnHeaders, timeSlots);
    });
}

export function onLoad(element, dotNetElementRef) {

}

export function onUpdate(element, dotNetElementRef) {
    //updateEvents(element, dotNetElementRef);   
}

export function onDispose(element, dotNetElementRef) {
}