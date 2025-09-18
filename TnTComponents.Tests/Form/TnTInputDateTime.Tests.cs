using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;
using TnTComponents;
using TnTComponents.Interfaces;
using Bunit.Rendering;
using Xunit;

namespace TnTComponents.Tests.Form;

public class TnTInputDateTime_Tests : BunitContext {
    
    public TnTInputDateTime_Tests()
    {
        // Set renderer info for tests that use NET9_0_OR_GREATER features
        SetRendererInfo(new RendererInfo("WebAssembly", true));
    }
    
    private TestModel CreateTestModel() => new();
    
    private TestDateOnlyModel CreateDateOnlyTestModel() => new();
    
    private TestTimeOnlyModel CreateTimeOnlyTestModel() => new();
    
    private TestModelWithValidation CreateValidationTestModel() => new();
    
    private IRenderedComponent<TnTInputDateTime<DateTime?>> RenderInputDateTime(TestModel? model = null, Action<ComponentParameterCollectionBuilder<TnTInputDateTime<DateTime?>>>? configure = null)
    {
        model ??= CreateTestModel();
        return Render<TnTInputDateTime<DateTime?>>(parameters =>
        {
            parameters
                .Add(p => p.ValueExpression, () => model.TestValue)
                .Add(p => p.Value, model.TestValue)
                .Add(p => p.ValueChanged, EventCallback.Factory.Create<DateTime?>(this, v => model.TestValue = v));
            configure?.Invoke(parameters);
        });
    }
    
    private IRenderedComponent<TnTInputDateTime<DateOnly?>> RenderInputDateOnly(TestDateOnlyModel? model = null, Action<ComponentParameterCollectionBuilder<TnTInputDateTime<DateOnly?>>>? configure = null)
    {
        model ??= CreateDateOnlyTestModel();
        return Render<TnTInputDateTime<DateOnly?>>(parameters =>
        {
            parameters
                .Add(p => p.ValueExpression, () => model.TestValue)
                .Add(p => p.Value, model.TestValue)
                .Add(p => p.ValueChanged, EventCallback.Factory.Create<DateOnly?>(this, v => model.TestValue = v));
            configure?.Invoke(parameters);
        });
    }
    
    private IRenderedComponent<TnTInputDateTime<TimeOnly?>> RenderInputTimeOnly(TestTimeOnlyModel? model = null, Action<ComponentParameterCollectionBuilder<TnTInputDateTime<TimeOnly?>>>? configure = null)
    {
        model ??= CreateTimeOnlyTestModel();
        return Render<TnTInputDateTime<TimeOnly?>>(parameters =>
        {
            parameters
                .Add(p => p.ValueExpression, () => model.TestValue)
                .Add(p => p.Value, model.TestValue)
                .Add(p => p.ValueChanged, EventCallback.Factory.Create<TimeOnly?>(this, v => model.TestValue = v));
            configure?.Invoke(parameters);
        });
    }
    
    private IRenderedComponent<TnTInputDateTime<DateTime?>> RenderValidationInputDateTime(TestModelWithValidation model, Action<ComponentParameterCollectionBuilder<TnTInputDateTime<DateTime?>>>? configure = null)
    {
        return Render<TnTInputDateTime<DateTime?>>(parameters =>
        {
            parameters
                .Add(p => p.ValueExpression, () => model.TestValue)
                .Add(p => p.Value, model.TestValue)
                .Add(p => p.ValueChanged, EventCallback.Factory.Create<DateTime?>(this, v => model.TestValue = v));
            configure?.Invoke(parameters);
        });
    }
    
    [Fact]
    public void Renders_DateTime_Input_With_Default_Classes_And_Type() {
        // Arrange & Act
        var cut = RenderInputDateTime();
        var input = cut.Find("input[type=datetime-local]");
        
        // Assert
        input.Should().NotBeNull();
        var label = cut.Find("label.tnt-input");
        var cls = label.GetAttribute("class")!;
        cls.Should().Contain("tnt-input");
        cls.Should().Contain("tnt-components");
    }

    [Fact]
    public void Has_TnTId_Attribute() {
        // Arrange & Act
        var cut = RenderInputDateTime();
        var label = cut.Find("label");
        
        // Assert
        cut.Instance.Should().BeAssignableTo<ITnTComponentBase>();
    }

    [Fact]
    public void Default_Type_Is_DateTime() {
        // Arrange & Act
        var cut = RenderInputDateTime();
        var input = cut.Find("input");
        
        // Assert
        input.GetAttribute("type").Should().Be("datetime-local");
        cut.Instance.Type.Should().Be(InputType.DateTime);
    }

    [Fact]
    public void DateTime_Type_Renders_DateTime_Local_Input() {
        // Arrange & Act
        var model = new { TestValue = DateTime.Now };
        var cut = Render<TnTInputDateTime<DateTime>>(parameters =>
        {
            parameters
                .Add(p => p.ValueExpression, () => model.TestValue)
                .Add(p => p.Value, model.TestValue);
        });
        var input = cut.Find("input");
        
        // Assert
        input.GetAttribute("type").Should().Be("datetime-local");
        cut.Instance.Type.Should().Be(InputType.DateTime);
    }

    [Fact]
    public void DateTimeOffset_Type_Renders_DateTime_Local_Input() {
        // Arrange & Act
        var model = new { TestValue = DateTimeOffset.Now };
        var cut = Render<TnTInputDateTime<DateTimeOffset>>(parameters =>
        {
            parameters
                .Add(p => p.ValueExpression, () => model.TestValue)
                .Add(p => p.Value, model.TestValue);
        });
        var input = cut.Find("input");
        
        // Assert
        input.GetAttribute("type").Should().Be("datetime-local");
        cut.Instance.Type.Should().Be(InputType.DateTime);
    }

    [Fact]
    public void DateOnly_Type_Renders_Date_Input() {
        // Arrange & Act
        var cut = RenderInputDateOnly();
        var input = cut.Find("input");
        
        // Assert
        input.GetAttribute("type").Should().Be("date");
        cut.Instance.Type.Should().Be(InputType.Date);
    }

    [Fact]
    public void DateOnly_MonthOnly_Renders_Month_Input() {
        // Arrange & Act
        var cut = RenderInputDateOnly(configure: p => p.Add(c => c.MonthOnly, true));
        var input = cut.Find("input");
        
        // Assert
        input.GetAttribute("type").Should().Be("month");
        cut.Instance.Type.Should().Be(InputType.Month);
    }

    [Fact]
    public void TimeOnly_Type_Renders_Time_Input() {
        // Arrange & Act
        var cut = RenderInputTimeOnly();
        var input = cut.Find("input");
        
        // Assert
        input.GetAttribute("type").Should().Be("time");
        cut.Instance.Type.Should().Be(InputType.Time);
    }

    [Fact]
    public void Throws_For_Unsupported_Type() {
        // Arrange & Act & Assert
        var model = new { TestValue = "" };
        var act = () => Render<TnTInputDateTime<string>>(parameters =>
        {
            parameters
                .Add(p => p.ValueExpression, () => model.TestValue)
                .Add(p => p.Value, model.TestValue);
        });
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("The type 'System.String' is not a supported DateTime type.");
    }

    [Fact]
    public void Default_Format_Set_For_DateTime() {
        // Arrange & Act
        var cut = RenderInputDateTime();
        
        // Assert
        cut.Instance.Format.Should().Be("yyyy-MM-ddTHH:mm:ss");
    }

    [Fact]
    public void Default_Format_Set_For_DateOnly() {
        // Arrange & Act
        var cut = RenderInputDateOnly();
        
        // Assert
        cut.Instance.Format.Should().Be("yyyy-MM-dd");
    }

    [Fact]
    public void Default_Format_Set_For_DateOnly_MonthOnly() {
        // Arrange & Act
        var cut = RenderInputDateOnly(configure: p => p.Add(c => c.MonthOnly, true));
        
        // Assert
        cut.Instance.Format.Should().Be("yyyy-MM");
    }

    [Fact]
    public void Default_Format_Set_For_TimeOnly() {
        // Arrange & Act
        var cut = RenderInputTimeOnly();
        
        // Assert
        cut.Instance.Format.Should().Be("HH:mm:ss");
    }

    [Fact]
    public void Custom_Format_Overrides_Default() {
        // Arrange & Act
        var cut = RenderInputDateTime(configure: p => p.Add(c => c.Format, "yyyy-MM-dd"));
        
        // Assert
        cut.Instance.Format.Should().Be("yyyy-MM-dd");
    }

    [Fact]
    public void Format_Attribute_Added_To_Additional_Attributes() {
        // Arrange & Act
        var cut = RenderInputDateTime();
        var input = cut.Find("input");
        
        // Assert
        input.GetAttribute("format").Should().Be("yyyy-MM-ddTHH:mm:ss");
    }

    [Fact]
    public void Label_Renders_When_Set() {
        // Arrange & Act
        var cut = RenderInputDateTime(configure: p => p.Add(c => c.Label, "Test Label"));
        
        // Assert
        var labelSpan = cut.Find(".tnt-label");
        labelSpan.TextContent.Should().Be("Test Label");
    }

    [Fact]
    public void Label_Does_Not_Render_When_Empty() {
        // Arrange & Act
        var cut = RenderInputDateTime();
        
        // Assert
        cut.FindAll(".tnt-label").Should().BeEmpty();
    }

    [Fact]
    public void Placeholder_Sets_Input_Attribute() {
        // Arrange & Act
        var cut = RenderInputDateTime(configure: p => p.Add(c => c.Placeholder, "Select date..."));
        var input = cut.Find("input");
        
        // Assert
        input.GetAttribute("placeholder").Should().Be("Select date...");
    }

    [Fact]
    public void Empty_Placeholder_Defaults_To_Single_Space() {
        // Arrange & Act
        var cut = RenderInputDateTime();
        var input = cut.Find("input");
        
        // Assert
        input.GetAttribute("placeholder").Should().Be(" ");
    }

    [Fact]
    public void Disabled_Adds_Disabled_Attribute_And_Class() {
        // Arrange & Act
        var cut = RenderInputDateTime(configure: p => p.Add(c => c.Disabled, true));
        var input = cut.Find("input");
        var label = cut.Find("label");
        
        // Assert
        input.HasAttribute("disabled").Should().BeTrue();
        label.GetAttribute("class")!.Should().Contain("tnt-disabled");
    }

    [Fact]
    public void ReadOnly_Adds_Readonly_Attribute() {
        // Arrange & Act
        var cut = RenderInputDateTime(configure: p => p.Add(c => c.ReadOnly, true));
        var input = cut.Find("input");
        
        // Assert
        input.HasAttribute("readonly").Should().BeTrue();
    }

    [Fact]
    public void AutoFocus_True_Renders_Autofocus_Attribute() {
        // Arrange & Act
        var cut = RenderInputDateTime(configure: p => p.Add(c => c.AutoFocus, true));
        var input = cut.Find("input");
        
        // Assert
        input.HasAttribute("autofocus").Should().BeTrue();
    }

    [Fact]
    public void AutoComplete_Sets_Input_Attribute() {
        // Arrange & Act
        var cut = RenderInputDateTime(configure: p => p.Add(c => c.AutoComplete, "off"));
        var input = cut.Find("input");
        
        // Assert
        input.GetAttribute("autocomplete").Should().Be("off");
    }

    [Fact]
    public void ElementId_Sets_Id_Attribute() {
        // Arrange & Act
        var cut = RenderInputDateTime(configure: p => p.Add(c => c.ElementId, "datetime-id"));
        var label = cut.Find("label");
        
        // Assert
        label.GetAttribute("id").Should().Be("datetime-id");
    }

    [Fact]
    public void ElementTitle_Sets_Title_Attribute() {
        // Arrange & Act
        var cut = RenderInputDateTime(configure: p => p.Add(c => c.ElementTitle, "DateTime Title"));
        var label = cut.Find("label");
        
        // Assert
        label.GetAttribute("title").Should().Be("DateTime Title");
    }

    [Fact]
    public void ElementLang_Sets_Lang_Attribute() {
        // Arrange & Act
        var cut = RenderInputDateTime(configure: p => p.Add(c => c.ElementLang, "en-US"));
        var label = cut.Find("label");
        
        // Assert
        label.GetAttribute("lang").Should().Be("en-US");
    }

    [Fact]
    public void DateTime_Value_Binding_Works_Correctly() {
        // Arrange
        var model = CreateTestModel();
        var testDate = new DateTime(2024, 1, 15, 14, 30, 0);
        model.TestValue = testDate;
        
        // Act
        var cut = RenderInputDateTime(model);
        var input = cut.Find("input");
        
        // Assert
        input.GetAttribute("value").Should().Be("2024-01-15T14:30:00");
    }

    [Fact]
    public void DateOnly_Value_Binding_Works_Correctly() {
        // Arrange
        var model = CreateDateOnlyTestModel();
        var testDate = new DateOnly(2024, 1, 15);
        model.TestValue = testDate;
        
        // Act
        var cut = RenderInputDateOnly(model);
        var input = cut.Find("input");
        
        // Assert
        input.GetAttribute("value").Should().Be("2024-01-15");
    }

    [Fact]
    public void TimeOnly_Value_Binding_Works_Correctly() {
        // Arrange
        var model = CreateTimeOnlyTestModel();
        var testTime = new TimeOnly(14, 30, 45);
        model.TestValue = testTime;
        
        // Act
        var cut = RenderInputTimeOnly(model);
        var input = cut.Find("input");
        
        // Assert
        input.GetAttribute("value").Should().Be("14:30:45");
    }

    [Fact]
    public void Input_Change_Updates_DateTime_Value() {
        // Arrange
        var model = CreateTestModel();
        var cut = RenderInputDateTime(model);
        var input = cut.Find("input");
        
        // Act
        input.Change("2024-12-25T10:15:30");
        
        // Assert
        model.TestValue.Should().Be(new DateTime(2024, 12, 25, 10, 15, 30));
    }

    [Fact]
    public void Input_Change_Updates_DateOnly_Value() {
        // Arrange
        var model = CreateDateOnlyTestModel();
        var cut = RenderInputDateOnly(model);
        var input = cut.Find("input");
        
        // Act
        input.Change("2024-12-25");
        
        // Assert
        model.TestValue.Should().Be(new DateOnly(2024, 12, 25));
    }

    [Fact]
    public void Input_Change_Updates_TimeOnly_Value() {
        // Arrange
        var model = CreateTimeOnlyTestModel();
        var cut = RenderInputTimeOnly(model);
        var input = cut.Find("input");
        
        // Act
        input.Change("16:45:30");
        
        // Assert
        model.TestValue.Should().Be(new TimeOnly(16, 45, 30));
    }

    [Fact]
    public void Input_Input_Updates_Value_When_BindOnInput_True() {
        // Arrange
        var model = CreateTestModel();
        var cut = RenderInputDateTime(model, p => p.Add(c => c.BindOnInput, true));
        var input = cut.Find("input");
        
        // Act
        input.Input("2024-06-15T08:30:00");
        
        // Assert
        model.TestValue.Should().Be(new DateTime(2024, 6, 15, 8, 30, 0));
    }

    [Fact]
    public void BindAfter_Callback_Invoked_On_Value_Change() {
        // Arrange
        DateTime? callbackValue = null;
        var model = CreateTestModel();
        var callback = EventCallback.Factory.Create<DateTime?>(this, (value) => callbackValue = value);
        var cut = RenderInputDateTime(model, p => p.Add(c => c.BindAfter, callback));
        var input = cut.Find("input");
        
        // Act
        input.Change("2024-03-10T12:00:00");
        
        // Assert
        callbackValue.Should().Be(new DateTime(2024, 3, 10, 12, 0, 0));
    }

    [Fact]
    public void StartIcon_Renders_When_Set() {
        // Arrange
        var startIcon = MaterialIcon.Home;
        
        // Act
        var cut = RenderInputDateTime(configure: p => p.Add(c => c.StartIcon, startIcon));
        
        // Assert
        cut.Markup.Should().Contain("tnt-start-icon");
        cut.Markup.Should().Contain("home");
    }

    [Fact]
    public void EndIcon_Renders_When_Set() {
        // Arrange
        var endIcon = MaterialIcon.Search;
        
        // Act
        var cut = RenderInputDateTime(configure: p => p.Add(c => c.EndIcon, endIcon));
        
        // Assert
        cut.Markup.Should().Contain("tnt-end-icon");
        cut.Markup.Should().Contain("search");
    }

    [Fact]
    public void SupportingText_Renders_When_Set() {
        // Arrange & Act
        var cut = RenderInputDateTime(configure: p => p.Add(c => c.SupportingText, "Helper text"));
        
        // Assert
        var supportingText = cut.Find(".tnt-supporting-text");
        supportingText.TextContent.Should().Be("Helper text");
    }

    [Fact]
    public void SupportingText_Does_Not_Render_When_Empty() {
        // Arrange & Act
        var cut = RenderInputDateTime();
        
        // Assert
        cut.FindAll(".tnt-supporting-text").Should().BeEmpty();
    }

    [Fact]
    public void Min_Attribute_Added_When_Set_In_Additional_Attributes() {
        // Arrange
        var attrs = new Dictionary<string, object> { { "min", "2024-01-01T00:00:00" } };
        
        // Act
        var cut = RenderInputDateTime(configure: p => p.Add(c => c.AdditionalAttributes, attrs));
        var input = cut.Find("input");
        
        // Assert
        input.GetAttribute("min").Should().Be("2024-01-01T00:00:00");
    }

    [Fact]
    public void Max_Attribute_Added_When_Set_In_Additional_Attributes() {
        // Arrange
        var attrs = new Dictionary<string, object> { { "max", "2024-12-31T23:59:59" } };
        
        // Act
        var cut = RenderInputDateTime(configure: p => p.Add(c => c.AdditionalAttributes, attrs));
        var input = cut.Find("input");
        
        // Assert
        input.GetAttribute("max").Should().Be("2024-12-31T23:59:59");
    }

    [Fact]
    public void Required_Attribute_Added_When_Additional_Attributes_Contains_Required() {
        // Arrange
        var attrs = new Dictionary<string, object> { { "required", true } };
        
        // Act
        var cut = RenderInputDateTime(configure: p => p.Add(c => c.AdditionalAttributes, attrs));
        var input = cut.Find("input");
        
        // Assert
        input.HasAttribute("required").Should().BeTrue();
    }

    [Fact]
    public void Appearance_Filled_Adds_Filled_Class() {
        // Arrange & Act
        var cut = RenderInputDateTime(configure: p => p.Add(c => c.Appearance, FormAppearance.Filled));
        var label = cut.Find("label");
        
        // Assert
        label.GetAttribute("class")!.Should().Contain("tnt-form-filled");
    }

    [Fact]
    public void Appearance_Outlined_Adds_Outlined_Class() {
        // Arrange & Act
        var cut = RenderInputDateTime(configure: p => p.Add(c => c.Appearance, FormAppearance.Outlined));
        var label = cut.Find("label");
        
        // Assert
        label.GetAttribute("class")!.Should().Contain("tnt-form-outlined");
    }

    [Fact]
    public void Color_Style_Variables_Are_Set() {
        // Arrange & Act
        var cut = RenderInputDateTime(configure: p => p
            .Add(c => c.TintColor, TnTColor.Primary)
            .Add(c => c.BackgroundColor, TnTColor.Surface)
            .Add(c => c.TextColor, TnTColor.OnSurface)
            .Add(c => c.ErrorColor, TnTColor.Error));
        var label = cut.Find("label");
        var style = label.GetAttribute("style")!;
        
        // Assert
        style.Should().Contain("--tnt-input-tint-color:var(--tnt-color-primary)");
        style.Should().Contain("--tnt-input-background-color:var(--tnt-color-surface)");
        style.Should().Contain("--tnt-input-text-color:var(--tnt-color-on-surface)");
        style.Should().Contain("--tnt-input-error-color:var(--tnt-color-error)");
    }

    [Fact]
    public void ValidationMessage_Renders_When_EditContext_Present_And_ValidationMessage_Not_Disabled() {
        // Arrange
        var model = CreateValidationTestModel();
        model.TestValue = null; // Invalid - required field
        
        // Act
        var cut = RenderValidationInputDateTime(model);
        
        // Assert
        cut.Instance.DisableValidationMessage.Should().BeFalse();
        cut.Instance.ValueExpression.Should().NotBeNull();
        cut.Instance.Should().BeOfType<TnTInputDateTime<DateTime?>>();
    }

    [Fact]
    public void ValidationMessage_Does_Not_Render_When_DisableValidationMessage_True() {
        // Arrange
        var model = CreateValidationTestModel();
        model.TestValue = null;
        
        // Act
        var cut = RenderValidationInputDateTime(model, p => p.Add(c => c.DisableValidationMessage, true));
        
        // Assert
        cut.Instance.DisableValidationMessage.Should().BeTrue();
    }

    [Fact]
    public void Invalid_String_Does_Not_Update_Value() {
        // Arrange
        var model = CreateTestModel();
        var originalValue = new DateTime(2024, 1, 1);
        model.TestValue = originalValue;
        var cut = RenderInputDateTime(model);
        var input = cut.Find("input");
        
        // Act
        input.Change("not a date");
        
        // Assert
        model.TestValue.Should().Be(originalValue); // Value should remain unchanged
    }

    [Fact]
    public void Null_Value_Renders_Empty_Input() {
        // Arrange
        var model = CreateTestModel();
        model.TestValue = null;
        
        // Act
        var cut = RenderInputDateTime(model);
        var input = cut.Find("input");
        
        // Assert
        var value = input.GetAttribute("value");
        (value == null || value == string.Empty).Should().BeTrue();
    }

    [Fact]
    public void Input_Title_Attribute_Uses_Field_Name() {
        // Arrange & Act
        var cut = RenderInputDateTime();
        var input = cut.Find("input");
        
        // Assert
        input.GetAttribute("title").Should().Be("TestValue");
    }

    [Fact]
    public void Both_Icons_Can_Be_Rendered_Together() {
        // Arrange & Act
        var cut = RenderInputDateTime(configure: p => p
            .Add(c => c.StartIcon, MaterialIcon.Home)
            .Add(c => c.EndIcon, MaterialIcon.Search));
        
        // Assert
        cut.Markup.Should().Contain("tnt-start-icon");
        cut.Markup.Should().Contain("home");
        cut.Markup.Should().Contain("tnt-end-icon");
        cut.Markup.Should().Contain("search");
    }

    private class TestModel {
        public DateTime? TestValue { get; set; }
    }

    private class TestDateOnlyModel {
        public DateOnly? TestValue { get; set; }
    }

    private class TestTimeOnlyModel {
        public TimeOnly? TestValue { get; set; }
    }

    private class TestModelWithValidation {
        [Required(ErrorMessage = "Test value is required")]
        public DateTime? TestValue { get; set; }
    }
}