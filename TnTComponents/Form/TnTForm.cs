using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System.Linq.Expressions;
using TnTComponents.Form;

namespace TnTComponents;

public class TnTForm : EditForm {

    [Parameter]
    public FormAppearance Appearance { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenComponent<CascadingValue<bool>>(0);
        builder.AddComponentParameter(10, "Name", nameof(IFormItem.ParentFormDisabled));
        builder.AddComponentParameter(20, "Value", Disabled);
        builder.AddComponentParameter(30, "ChildContent", new RenderFragment(builderOne => {
            builderOne.OpenComponent<CascadingValue<bool>>(0);
            builderOne.AddComponentParameter(10, "Name", nameof(IFormItem.ParentFormReadOnly));
            builderOne.AddComponentParameter(20, "Value", ReadOnly);
            builderOne.AddComponentParameter(30, "ChildContent", new RenderFragment(builderTwo => {
                builderTwo.OpenComponent<CascadingValue<TnTForm>>(0);
                builderTwo.AddComponentParameter(10, "Value", this);
                builderTwo.AddComponentParameter(20, "IsFixed", true);
                builderTwo.AddComponentParameter(30, "ChildContent", new RenderFragment(builderThree => {
                    builderThree.OpenComponent<CascadingValue<FormAppearance>>(0);
                    builderThree.AddComponentParameter(10, "Value", Appearance);
                    builderThree.AddComponentParameter(20, "IsFixed", true);
                    builderThree.AddComponentParameter(20, "Name", nameof(IFormItem.ParentFormAppearance));
                    builderThree.AddComponentParameter(30, "ChildContent", new RenderFragment(builderFour => {
                        base.BuildRenderTree(builderFour);
                    }));
                    builderThree.CloseComponent();
                }));
                builderTwo.CloseComponent();
            }));
            builderOne.CloseComponent();
        }));
        builder.CloseComponent();
    }
}