# MethodBinding.Avalonia

[![View nuget packages](https://img.shields.io/nuget/v/MethodBinding.Avalonia.svg)](https://www.nuget.org/packages/MethodBinding.Avalonia/)

**MethodBinding** provides the ability to bind events directly to methods. It supports a full range of parameter passing scenarios including constants, bindings, event sender and event argument values.

**DOESN'T YET WORK**

Features:
- No restrictions on the method signature.
- Event sender or the event args can be passed as an argument.
- Method target is resolved with full PropertyPath binding support.
- Method arguments can be provided by bindings or other common extensions (i.e. `StaticResource` or `x:Static`).
- Methods are matched by argument types if more than one method with the same name exists.
- Method arguments passed as XAML strings are converted to the required method parameter types, so you don't have to use extensions / static resources or bloat the XAML by defining the method binding in element syntax instead of attribute syntax just to pass in an `int` or `double`.

Ported to Avalonia from [Singulink.WPF.Data.MethodBinding](https://github.com/Singulink/Singulink.WPF.Data.MethodBinding)

## Installation

The package is available on NuGet - simply install the `MethodBinding.Avalonia` package.

**Supported Runtimes**: .NET Core 3.1+ and .NET 5+.

## Usage

To import the namespace for Singulink WPF components into your XAML file, add an `xmlns` definition to the root element of your XAML file as follows:

```xml
<Window xmlns:s="https://github.com/mysteryx93/MethodBinding.Avalonia"/>
```

The simplest form of a method binding just passes in the method name:
```cs
public class ViewModel
{
    public void Save() { }
}
```
```xml
<Button Content="Save" Click="{s:MethodBinding Save}" />
```

You can specify parameters to pass into the method after the method name:
```cs
public class ViewModel
{
    public void Save(string fileName) { }
}
```
```xml
<Button Content="Save" Click="{s:MethodBinding Save, 'filename.txt'}" />
```

XAML string parameters are converted automatically to method parameter types (note: this only works if there are no other overloads with the same number of parameters):

```cs
public class ViewModel
{
    public void DoStuff(int x, double y, Color z, CornerRadius cornerRadius) { }
}
```
```xml
<Button Click="{s:MethodBinding DoStuff, 10, 1234.567, #223344, '5,5,5,5'}" />
```

Parameters can accept bindings:
```cs
public class ViewModel
{
    public void Save(Document document) { }
}
```
```xml
<Button Content="Save" Click="{s:MethodBinding Save, {Binding CurrentDocument}}" />
```

Or other markup extensions:
```cs
public class ViewModel
{
    public void SetBackground(Color color) { }
}
```
```xml
<Button Content="Dark Mode" Click="{s:MethodBinding SetBackground, {StaticResource DarkBackground}}" />
```

You can pass in the sender of the event as a parameter:
```cs
public class ViewModel
{
    public void OnButtonClick(Button sender) { }
}
```
```xml
<Button Click="{s:MethodBinding OnButtonClick, {s:EventSender}}" />
```

Or the event args:
```cs
public class ViewModel
{
    public void StartDrawing(MouseEventArgs e) { }
    public void AddDrawingPoint(MouseEventArgs e) { }
    public void EndDrawing(MouseEventArgs e) { }
}
```
```xml
<Canvas MouseDown="{s:MethodBinding StartDrawing, {s:EventArgs}}"
        MouseMove="{s:MethodBinding AddDrawingPoint, {s:EventArgs}}"
        MouseUp="{s:MethodBinding EndDrawing, {s:EventArgs}}" />
```

You can even bind to properties on the event args themselves and pass them as parameters:
```cs
public class ViewModel
{
    public void UpdateSize(Size newSize) { }
}
```
```xml
<!-- Passes in SizeChangedEventArgs.NewSize as the parameter -->
<Canvas SizeChanged="{s:MethodBinding UpdateSize, {s:EventArgs NewSize}}" />
```

You can pass in as many parameters as needed. Methods that contain overloads are matched based on the number of parameters and the parameter types.

If you want to call the method on something other than the current data context, you can use bindings or other markup extensions before the method name to set the target that the method should be called on:
```cs
public class DocumentService
{
    public static DocumentService Instance { get; } = new DocumentService();
    public void Save(Document document) { }
}
```
```xml
<Button Content="Save" Click="{s:MethodBinding {x:Static local:DocumentService.Instance}, Save, {Binding CurrentDocument}}" />
```

Advanced scenarios can use a binding or other markup extension for the method name as well. 

Method binding errors (i.e. the method target is null or the method name can't be resolved) are reported in the output window, similar to how normal WPF binding errors are reported. If you are having a problem with a method binding, debug the application and look there first.
