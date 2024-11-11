

export function onLoad(element, dotNetRef) {
    if (element) {
        const options = {
            root: element,
            rootMargin: "32px",
            threshold: 1.0,
        };

        function intersectCallback(entries, _) {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    dotNetRef.invokeMethodAsync("BeginLoad");
                }
            });

            observer.disconnect();
        }

        const observer = new IntersectionObserver(intersectCallback, options);
        observer.observe(element);
    }
}

export function onUpdate(element, dotNetRef) {
}

export function onDispose(element, dotNetRef) {

}