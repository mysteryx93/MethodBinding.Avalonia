using System;
using Avalonia;
using Avalonia.Markup.Xaml;

namespace MethodBinding.Avalonia.Tests
{
    public static class TestHelper
    {
        public static void RunMethodBinding(object? dataContext, MethodBindingExtension binding)
        {
            var obj = new StyledElement {
                Name = "TestElement",
                DataContext = dataContext,
            };

            var handler = (EventHandler)binding.ProvideValue(new ServiceProvider(obj, typeof(StyledElement).GetEvent("Initialized")!));
            handler.Invoke(obj, EventArgs.Empty);
        }

        private class ServiceProvider : IServiceProvider, IProvideValueTarget
        {
            public object TargetObject { get; }

            public object TargetProperty { get; }

            public ServiceProvider(object targetObject, object targetProperty)
            {
                TargetObject = targetObject;
                TargetProperty = targetProperty;
            }

            public object? GetService(Type serviceType) => serviceType.IsInstanceOfType(this) ? this : null;
        }
    }
}