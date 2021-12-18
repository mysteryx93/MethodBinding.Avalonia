using System;
using Avalonia;
using Avalonia.Data;

namespace MethodBinding.Avalonia
{
    internal static class PropertyPathHelper
    {
        private static readonly object s_fallbackValue = new object();

        public static object? Evaluate(string path, object source)
        {
            var target = new DependencyTarget();
            var binding = new Binding() {
                    Path = path, Source = source, Mode = BindingMode.OneTime, FallbackValue = s_fallbackValue,
                }
                .Initiate(target, DependencyTarget.ValueProperty);
            BindingOperations.Apply(target, DependencyTarget.ValueProperty, binding, null);

            if (target.Value == s_fallbackValue)
                throw new ArgumentException(
                    $"Could not resolve property path '{path}' on source object type '{source.GetType()}'.");

            return target.Value;
        }

        private class DependencyTarget : AvaloniaObject
        {
            public static readonly AvaloniaProperty ValueProperty =
                AvaloniaProperty.RegisterDirect<DependencyTarget, object?>(nameof(Value), (o) => o.Value, (o, v) => o.Value = v);

            public object? Value {
                get => GetValue(ValueProperty);
                set => SetValue(ValueProperty, value);
            }
        }
    }
}