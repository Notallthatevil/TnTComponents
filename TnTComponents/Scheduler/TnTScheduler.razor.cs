using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Scheduler;
using TnTComponents.Scheduler.Infrastructure;

namespace TnTComponents;
public partial class TnTScheduler<TEventType> {
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    private ScheduleViewBase _selectedView { get; set; } = default!;
    protected IDictionary<Type, ScheduleViewBase> ScheduledViews = new Dictionary<Type, ScheduleViewBase>();

    public override string? CssClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-scheduler")
        .Build();

    public override string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    public override bool IsSelectedView(ScheduleViewBase scheduleViewBase) {
        return _selectedView == scheduleViewBase;
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();

        if (ScheduledViews.Count == 1) {
            _selectedView = ScheduledViews.Values.First();
        }
    }

    public override void AddView(ScheduleViewBase view) {
        ScheduledViews.Add(view.GetType(), view);
        if(ScheduledViews.Count == 1) {
            _selectedView = view;
        }
    }

    public override void RemoveView(ScheduleViewBase view) {
        ScheduledViews.Remove(view.GetType());
    }

}
