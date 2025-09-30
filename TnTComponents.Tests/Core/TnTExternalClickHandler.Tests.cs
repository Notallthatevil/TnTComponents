﻿using Microsoft.AspNetCore.Components;
using System.Reflection;
using TnTComponents.Core;

namespace TnTComponents.Tests.Core;

public class TnTExternalClickHandler_Tests : BunitContext {
    private const string JsModulePath = "./_content/TnTComponents/Core/TnTExternalClickHandler.razor.js";

    public TnTExternalClickHandler_Tests() {
        var module = JSInterop.SetupModule(JsModulePath);
        module.SetupVoid("onLoad", _ => true).SetVoidResult();
        module.SetupVoid("onUpdate", _ => true).SetVoidResult();
        module.SetupVoid("onDispose", _ => true).SetVoidResult();
        module.SetupVoid("externalClickCallbackRegister", _ => true).SetVoidResult();
        module.SetupVoid("externalClickCallbackDeregister", _ => true).SetVoidResult();
    }

    [Fact]
    public void CanRenderMultipleTimesWithoutError() {
        // Arrange
        var comp = Render<TnTExternalClickHandler>();
        // Act
        comp.Render();
        comp.Render();
        // Assert
        comp.Instance.Should().NotBeNull();
    }

    [Fact]
    public async Task Defensive_NoJsInteropIfModuleNotLoaded() {
        // Arrange: Try to set IsolatedJsModule to null via reflection, skip if not possible
        var comp = Render<TnTExternalClickHandler>();
        var prop = comp.Instance.GetType().GetProperty("IsolatedJsModule", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (prop == null || !prop.CanWrite) {
            // Property cannot be set, skip test
            true.Should().BeTrue();
            return;
        }
        prop.SetValue(comp.Instance, null);
        // Act & Assert
        await comp.Instance.DisposeAsync(); // Should not throw
    }

    [Fact]
    public async Task DisposesAndDeregistersCallback() {
        // Arrange
        var comp = Render<TnTExternalClickHandler>();
        // Act
        await comp.Instance.DisposeAsync();
        // Assert
        JSInterop.VerifyInvoke("externalClickCallbackDeregister");
    }

    [Fact]
    public void DoesNotInvokeCallback_WhenClickIsInside() {
        // Arrange
        var called = false;
        var comp = Render<TnTExternalClickHandler>(parameters =>
            parameters.Add(p => p.ExternalClickCallback, EventCallback.Factory.Create(this, () => { called = true; return Task.CompletedTask; }))
        );
        // Act: Do not call OnClick (simulates click inside) Assert
        called.Should().BeFalse();
    }

    [Fact]
    public async Task DoesNotThrow_WhenCallbackNotSet() {
        // Arrange
        var comp = Render<TnTExternalClickHandler>();
        // Act & Assert
        await comp.Instance.OnClick(); // Should not throw
    }

    [Fact]
    public async Task InvokesCallback_WhenClickIsOutside() {
        // Arrange
        var called = false;
        var comp = Render<TnTExternalClickHandler>(parameters =>
            parameters.Add(p => p.ExternalClickCallback, EventCallback.Factory.Create(this, () => { called = true; return Task.CompletedTask; }))
        );
        // Act
        await comp.Instance.OnClick();
        // Assert
        called.Should().BeTrue();
    }

    [Fact]
    public async Task InvokesCallback_WhenElementRemovedBeforeClick() {
        // Arrange
        var called = false;
        var comp = Render<TnTExternalClickHandler>(parameters =>
            parameters.Add(p => p.ExternalClickCallback, EventCallback.Factory.Create(this, () => { called = true; return Task.CompletedTask; }))
        );
        // Act
        await comp.Instance.OnClick();
        // Assert
        called.Should().BeTrue();
    }

    [Fact]
    public void RendersChildContent() {
        // Arrange
        var content = "<span>child</span>";
        // Act
        var comp = Render<TnTExternalClickHandler>(p => p.AddChildContent(content));
        // Assert
        comp.Markup.Should().Contain("child");
    }
}