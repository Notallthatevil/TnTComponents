using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text;
using TnTComponents.Enum;

namespace TnTComponents.Forms;
public partial class TnTDatePicker {
    [Parameter]
    public string Format { get; set; } = "yyyy-MM-dd";

    [Parameter]
    public DateOnly DefaultDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    private readonly ICollection<int> _months = Enumerable.Range(1, 12).ToList();
    private readonly IReadOnlySet<int> _disabledMonths = new HashSet<int>();
    private readonly ICollection<int> _years = Enumerable.Range(2000, 2050).ToList();

    private int _dropdownMonths;
    private int _dropdownYears;


    public TnTDatePicker() {
        _dropdownMonths = DefaultDate.Month;
        _dropdownYears = DefaultDate.Year;
    }


    /// <inheritdoc/>
    protected override string? FormatValueAsString(DateOnly value) {
        return value.ToString(Format);
    }

    protected override string GetCssClass() {
        return base.GetCssClass() + " date-picker";
    }

    protected override async Task OnFocusOutAsync(FocusEventArgs e) {
        await RemoveInputFocus();
        await base.OnFocusOutAsync(e);
    }

    private void PrevMonth() { }
    private void NextMonth() { }
    //[Parameter]
    //public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    //[Parameter]
    //public DateOnly MinDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddYears(-50));

    //[Parameter]
    //public DateOnly MaxDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddYears(50));

    //[Parameter]
    //public IReadOnlySet<DateOnly>? DisabledDates { get; set; }

    //private DateOnly _currentMonthYear;
    //private int _selectedMonth { get => _currentMonthYear.Month; set => UpdateCurrentDateView(new DateOnly(_selectedYear, value, 1)); }
    //private int _selectedYear { get => _currentMonthYear.Year; set => UpdateCurrentDateView(new DateOnly(value, _selectedMonth, 1)); }
    //private ICollection<int> _months = Enumerable.Range(1, 12).ToArray();
    //private ICollection<int> _years = default!;

    //private HashSet<int> _disabledMonths = new HashSet<int>();

    //protected override void OnInitialized() {
    //    if (StartDate < MinDate) {
    //        throw new ArgumentException($"{nameof(StartDate)} cannot be less then {nameof(MinDate)}");
    //    }

    //    if (StartDate > MaxDate) {
    //        throw new ArgumentException($"{nameof(StartDate)} cannot be greater then {nameof(MaxDate)}");
    //    }

    //    _selectedMonth = StartDate.Month;
    //    _selectedYear = StartDate.Year;

    //    _years = Enumerable.Range(MinDate.Year, (MaxDate.Year - MinDate.Year) + 1).ToArray();

    //    if (string.IsNullOrWhiteSpace(Format)) {
    //        Format = "yyyy-MM-dd";
    //    }

    //    base.OnInitialized();
    //}

    //protected override string GetCssClass() {
    //    return $"{base.GetCssClass()} date-picker";
    //}

    //protected override async Task OnChange(ChangeEventArgs e) {
    //    await ValueChanged.InvokeAsync((DateOnly?)e?.Value);
    //}

    //private async Task SelectDate(DateOnly selectedDate) {
    //    if (selectedDate >= MinDate && selectedDate <= MaxDate && !DisabledDates.Contains(selectedDate)) {
    //        if (selectedDate.Month == _currentMonthYear.Month && selectedDate.Year == _currentMonthYear.Year) {
    //            Value = selectedDate;
    //            _currentMonthYear = new DateOnly(selectedDate.Year, selectedDate.Month, 1);
    //            await OnChange(new ChangeEventArgs { Value = Value });
    //        }
    //        else if (selectedDate < _currentMonthYear) {
    //            PrevPage();
    //            return;
    //        }
    //        else if (selectedDate > _currentMonthYear) {
    //            NextPage();
    //            return;
    //        }
    //    }
    //}

    //private void NextPage() {
    //    var nextMonth = _currentMonthYear.AddMonths(1);
    //    var nextMonthPage = new DateOnly(nextMonth.Year, nextMonth.Month, 1);
    //    if (nextMonth <= MaxDate) {
    //        UpdateCurrentDateView(nextMonth);
    //    }
    //}

    //private void PrevPage() {
    //    var prevMonth = _currentMonthYear.AddMonths(-1);
    //    var prevPageDate = new DateOnly(prevMonth.Year, prevMonth.Month, DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month));
    //    if (prevPageDate >= MinDate) {
    //        UpdateCurrentDateView(prevMonth);
    //    }
    //}

    private DateOnly GetStartDate() {
        var startDate = new DateTime(_dropdownYears, _dropdownMonths, 1);
        return DateOnly.FromDateTime(startDate.AddDays(-(double)startDate.DayOfWeek));
    }

    private string GetDateItemClass(DateOnly date) {
        var strBuilder = new StringBuilder("table-content ");
        if (date == DateOnly.FromDateTime(DateTime.Today)) {
            strBuilder.Append("current-date ");
        }

        if (date.Month != _dropdownMonths) {
            strBuilder.Append("out-of-month ");
        }

        if (date == Value) {
            strBuilder.Append("selected ");
        }

        //if (date > MaxDate || date < MinDate || DisabledDates.Contains(date)) {
        //    strBuilder.Append("disabled ");
        //}

        return strBuilder.ToString();
    }

    //private void UpdateCurrentDateView(DateOnly newDate) {
    //    if (newDate < MinDate) {
    //        _currentMonthYear = MinDate;
    //    }
    //    else if (newDate > MaxDate) {
    //        _currentMonthYear = MaxDate;
    //    }
    //    else {
    //        _currentMonthYear = newDate;
    //    }

    //    _disabledMonths.Clear();
    //    if (_currentMonthYear.Year == MinDate.Year) {
    //        foreach (var month in Enumerable.Range(1, MinDate.Month - 1)) {
    //            _disabledMonths.Add(month);
    //        }
    //    }

    //    if (_currentMonthYear.Year == MaxDate.Year) {
    //        foreach (var month in Enumerable.Range(MaxDate.Month + 1, 12 - MaxDate.Month)) {
    //            _disabledMonths.Add(month);
    //        }
    //    }
    //}
}
