using TnTComponents.Scheduler.Events;
using TnTComponents.Scheduler;

namespace LiveTest.Client.Pages;
public partial class Scheduler {
    private DateTime today = DateTime.Today;
    private int months = 12;
    private List<TnTEvent> TasksList;
    private bool draggable = true;

    protected override void OnInitialized() {
        TasksList = [
            new TnTEvent { EventStart = today.AddDays(0), EventEnd = today.AddDays(1).AddHours(12), Title = "HELLO",  },
            new TnTEvent { EventStart = today.AddDays(4).AddHours(8), EventEnd = today.AddDays(4).AddHours(11), Title = "😉 CP" } ,
            new TnTEvent { EventStart = today.AddHours(5), EventEnd = today.AddHours(10), Title = "CALL" },
            new TnTEvent { EventStart = today.AddDays(-2).AddHours(1).AddMinutes(15), EventEnd = today.AddDays(-2).AddHours(9) , Title="POD"} ,
            new TnTEvent { EventStart = today.AddDays(-2).AddHours(1).AddMinutes(15), EventEnd = today.AddDays(-2).AddHours(9), Title = "MTG"},
            new TnTEvent { EventStart = today.AddDays(-2).AddHours(1).AddMinutes(15), EventEnd = today.AddDays(-2).AddHours(9), Title = "DEV" },
            new TnTEvent { EventStart = today.AddDays(-2).AddHours(1).AddMinutes(45), EventEnd = today.AddDays(-2).AddHours(9), Title = "MEET" },
            new TnTEvent { EventStart = today.AddDays(-2).AddHours(1).AddMinutes(15), EventEnd = today.AddDays(-2).AddHours(9) , Title="POD2"} ,
            new TnTEvent { EventStart = today.AddDays(-2).AddHours(1).AddMinutes(15), EventEnd = today.AddDays(-2).AddHours(9), Title = "MTG2"},
            new TnTEvent { EventStart = today.AddDays(-2).AddHours(1).AddMinutes(15), EventEnd = today.AddDays(-2).AddHours(9), Title = "DEV2" },
            new TnTEvent { EventStart = today.AddDays(-2).AddHours(2).AddMinutes(30), EventEnd = today.AddDays(-2).AddHours(9), Title = "Lower Event" },
            new TnTEvent { EventStart = today.AddDays(32), EventEnd = today.AddDays(32), Title = "BLAZOR" } ,
            new TnTEvent { EventStart = today.AddDays(45).AddHours(8), EventEnd = today.AddDays(45).AddHours(9), Title = "MEETING" },
            new TnTEvent { EventStart = today.AddDays(-8), EventEnd = today.AddDays(-7), Title = "MEET⭐" }
        ];
    }

    private void OnEventClicked(TnTEvent @event) {
       Console.WriteLine($"Event clicked: {@event.Title} - {@event.Id}");
    }

    private void OnEventSlotClicked(DateTimeOffset dateTimeOffset) {
        Console.WriteLine($"Event slot clicked: {dateTimeOffset}");
    }
}
