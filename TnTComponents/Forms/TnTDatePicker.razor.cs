using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using TnTComponents.Enum;

namespace TnTComponents.Forms;
public partial class TnTDatePicker {
    [Parameter]
    public string Format { get; set; } = "yyyy-MM-dd";

    [Parameter]
    public DateOnly DefaultDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Parameter]
    public DateOnly MaxDate { get; set; }

    [Parameter]
    public DateOnly MinDate { get; set; }

    [Parameter]
    public IReadOnlySet<DateOnly> DisabledDates { get; set; } = new HashSet<DateOnly>();

    private readonly ICollection<int> _months = Enumerable.Range(1, 12).ToList();
    private readonly HashSet<int> _disabledMonths = [];
    private ICollection<int> _years = default!;

    private int _dropdownMonth;
    private int _dropdownYearValue;
    private int _dropdownYear {
        get => _dropdownYearValue;
        set {
            _dropdownYearValue = value;
            if (value == MaxDate.Year) {
                _disabledMonths.Clear();
                for (var i = MaxDate.Month + 1; i <= 12; ++i) {
                    _disabledMonths.Add(i);
                }
            }
            else if (value == MinDate.Year) {
                _disabledMonths.Clear();
                for (var i = MinDate.Month - 1; i >= 0; --i) {
                    _disabledMonths.Add(i);
                }
            }
        }
    }



    protected override void OnInitialized() {
        _dropdownMonth = DefaultDate.Month;
        _dropdownYear = DefaultDate.Year;

        if (MaxDate == default) {
            MaxDate = DefaultDate.AddYears(1);
        }

        if (MinDate == default) {
            MinDate = DefaultDate.AddYears(-1);
        }

        _years = Enumerable.Range(MinDate.Year, (MaxDate.Year - MinDate.Year) + 1).ToList();

        base.OnInitialized();
    }


    /// <inheritdoc/>
    protected override string? FormatValueAsString(DateOnly? value) {
        return value?.ToString(Format);
    }

    protected override string GetCssClass() {
        return base.GetCssClass() + " date-picker";
    }

    protected override async Task OnFocusOutAsync(FocusEventArgs e) {
        await RemoveInputFocus();
        await base.OnFocusOutAsync(e);
    }

    private void SelectDate(DateOnly selectedDate) {
        if (selectedDate >= MinDate && selectedDate <= MaxDate && !DisabledDates.Contains(selectedDate)) {
            if (selectedDate.Month == _dropdownMonth && selectedDate.Year == _dropdownYear) {
                Value = selectedDate;
                //_currentMonthYear = new DateOnly(selectedDate.Year, selectedDate.Month, 1);
                //await OnChange(new ChangeEventArgs { Value = Value });
            }
            else if (selectedDate.Year == _dropdownYear) {
                if (selectedDate.Month < _dropdownMonth) {
                    PrevPage();
                }
                else {
                    NextPage();
                }
            }
            else if (selectedDate.Year < _dropdownYear) {
                PrevPage();
            }
            else {
                NextPage();
            }
        }
    }

    private void NextPage() {
        if (CanShowNextMonth()) {
            var nextMonthPage = new DateOnly(_dropdownYear, _dropdownMonth, 1).AddMonths(1);
            _dropdownYear = nextMonthPage.Year;
            _dropdownMonth = nextMonthPage.Month;
        }
    }

    private void PrevPage() {
        if (CanShowPrevMonth()) {
            var prevMonthPage = new DateOnly(_dropdownYear, _dropdownMonth, 1).AddMonths(-1);
            _dropdownYear = prevMonthPage.Year;
            _dropdownMonth = prevMonthPage.Month;
        }
    }

    private DateOnly GetStartDate() {
        var startDate = new DateTime(_dropdownYear, _dropdownMonth, 1);
        return DateOnly.FromDateTime(startDate.AddDays(-(double)startDate.DayOfWeek));
    }

    private string GetDateItemClass(DateOnly date) {
        var strBuilder = new StringBuilder("table-content ");
        if (date == DateOnly.FromDateTime(DateTime.Today)) {
            strBuilder.Append("current-date ");
        }

        if (date.Month != _dropdownMonth) {
            strBuilder.Append("out-of-month ");
        }

        if (date == Value) {
            strBuilder.Append("selected ");
        }

        if (date > MaxDate || date < MinDate || DisabledDates.Contains(date)) {
            strBuilder.Append("disabled ");
        }

        return strBuilder.ToString();
    }


    private bool CanShowPrevMonth() {
        return new DateOnly(_dropdownYear, _dropdownMonth, 1).AddDays(-1) >= MinDate;

    }
    private bool CanShowNextMonth() {
        return new DateOnly(_dropdownYear, _dropdownMonth, 1).AddMonths(1) <= MaxDate;
    }
}
