using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Scheduler.Infrastructure;

namespace TnTComponents;

public partial class TnTScheduler<TEventType> {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? CssClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-scheduler")
        .Build();

    public override string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    protected IDictionary<Type, ScheduleViewBase> ScheduledViews = new Dictionary<Type, ScheduleViewBase>();
    private ScheduleViewBase _selectedView { get; set; } = default!;

    public override void AddView(ScheduleViewBase view) {
        ScheduledViews.Add(view.GetType(), view);
        if (ScheduledViews.Count == 1) {
            _selectedView = view;
        }
    }

    public override bool IsSelectedView(ScheduleViewBase scheduleViewBase) {
        return _selectedView == scheduleViewBase;
    }

    public override void RemoveView(ScheduleViewBase view) {
        ScheduledViews.Remove(view.GetType());
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();

        if (ScheduledViews.Count == 1) {
            _selectedView = ScheduledViews.Values.First();
        }
    }
}