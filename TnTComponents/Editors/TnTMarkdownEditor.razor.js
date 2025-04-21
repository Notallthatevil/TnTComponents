import * as EasyMDEImport from "https://unpkg.com/easymde/dist/easymde.min.js";
import * as HighlightImport from "https://cdn.jsdelivr.net/highlight.js/latest/highlight.min.js";

const markdownEditorsMap = new Map();
const elementDotNetRefMap = new Map();
export function onLoad(element, dotNetElementRef) {
    if (!customElements.get('tnt-markdown-editor')) {
        customElements.define('tnt-markdown-editor', class extends HTMLElement {
            static observedAttributes = [TnTComponents.customAttribute];

            // We use attributeChangedCallback instead of connectedCallback
            // because a page-script element might get reused between enhanced
            // navigations.
            attributeChangedCallback(name, oldValue, newValue) {
                if (name !== TnTComponents.customAttribute) {
                    return;
                }

                if (elementDotNetRefMap.get(oldValue)) {
                    elementDotNetRefMap.set(newValue, elementDotNetRefMap.get(oldValue));
                    elementDotNetRefMap.delete(newValue);
                }

                let easyMDE = null;

                if (markdownEditorsMap.get(oldValue)) {
                    easyMDE = markdownEditorsMap.get(oldValue).mde;
                    markdownEditorsMap.delete(oldValue);
                }

                if (easyMDE === null) {
                    let child = this.querySelector('textarea');
                    if (!child) {
                        child = document.createElement('textarea');
                        this.appendChild(child);
                    }
                    let initalValueElement = this.querySelector('.initial-value');
                    let initialValue = undefined;
                    if (initalValueElement) {
                        initialValue = initalValueElement.innerHTML;
                        initalValueElement.remove();
                    }

                    let self = this;
                    easyMDE = new EasyMDE({
                        element: child,
                        initialValue: initialValue,
                        sideBySideFullscreen: false,
                        previewRender: (text) => {
                            let attr = self.getAttribute(TnTComponents.customAttribute);
                            if (attr) {
                                let e = markdownEditorsMap.get(attr).mde;
                                if (e && e.markdown) {
                                    text = e.markdown(text);
                                }

                                text = text.replace(/!&gt;&lt;(.+)?!&gt;&lt;/g, '<div class="tnt-text-align-center">$1</div>');
                                text = text.replace(/!&lt;(.+)?!&lt;/g, '<div class="tnt-text-align-left">$1</div>');
                                text = text.replace(/!&gt;(.+)?!&gt;/g, '<div class="tnt-text-align-right">$1</div>');
                                text = text.replace(/<table>/, '<table style="width:100%">');
                            }
                            return text;
                        },
                        toolbar: [
                            "bold",
                            "italic",
                            "strikethrough",
                            "|",
                            {
                                name: "left-text",
                                action: (editor) => {
                                    let cm = editor.codemirror;
                                    var output = '';
                                    var selectedText = cm.getSelection();
                                    var text = selectedText || 'align-left';

                                    output = '!\\<' + text + '!\\<';
                                    cm.replaceSelection(output);
                                },
                                className: "fa fa-align-left",
                                text: "",
                                title: "Align Left"
                            },
                            {
                                name: "center-text",
                                action: (editor) => {
                                    let cm = editor.codemirror;
                                    var output = '';
                                    var selectedText = cm.getSelection();
                                    var text = selectedText || 'align-left';

                                    output = '!\\>\\<' + text + '!\\>\\<';
                                    cm.replaceSelection(output);
                                },
                                className: "fa fa-align-center",
                                text: "",
                                title: "Align Center"
                            },
                            {
                                name: "right-text",
                                action: (editor) => {
                                    let cm = editor.codemirror;
                                    var output = '';
                                    var selectedText = cm.getSelection();
                                    var text = selectedText || 'align-left';

                                    output = '!\\>' + text + '!\\>';
                                    cm.replaceSelection(output);
                                },
                                className: "fa fa-align-right",
                                text: "",
                                title: "Align Right"
                            },
                            "|",
                            "heading",
                            "heading-smaller",
                            "heading-bigger",
                            "heading-1",
                            "heading-2",
                            "heading-3",
                            "|",
                            "code",
                            "|",
                            "quote",
                            "|",
                            "unordered-list",
                            "ordered-list",
                            "|",
                            "clean-block",
                            "|",
                            "link",
                            "image",
                            "upload-image",
                            "|",
                            "table",
                            "|",
                            "horizontal-rule",
                            "|",
                            "preview",
                            "side-by-side",
                            "|",
                            "guide",
                            "|",
                            "undo",
                            "redo"
                        ]
                    });
                    if (easyMDE.codemirror && easyMDE.codemirror.on) {
                        easyMDE.codemirror.on("change", function () {
                            var text = easyMDE.value();
                            const dotNetRef = elementDotNetRefMap.get(newValue);
                            if (dotNetRef) {
                                dotNetRef.invokeMethodAsync("UpdateValue", text, easyMDE.options.previewRender(text));
                            }
                        });
                    }
                }

                markdownEditorsMap.set(newValue, {
                    element: this,
                    mde: easyMDE
                });
            }

            disconnectedCallback() {
                let attribute = this.getAttribute(TnTComponents.customAttribute);
                if (markdownEditorsMap.get(attribute)) {
                    markdownEditorsMap.delete(attribute);
                }
            }
        });
    }
}

export function onUpdate(element, dotNetElementRef) {
    if (element && dotNetElementRef) {
        const key = element.getAttribute(TnTComponents.customAttribute);

        if (elementDotNetRefMap.get(key)) {
            elementDotNetRefMap.delete(key);
        }
        elementDotNetRefMap.set(key, dotNetElementRef);
    }
}

export function onDispose(element, dotNetElementRef) {
    if (element) {
        const key = element.getAttribute(TnTComponents.customAttribute);
        elementDotNetRefMap.delete(key);
    }
}