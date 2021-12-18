using System;
using Avalonia.Markup.Xaml;

namespace MethodBinding.Avalonia
{
    /// <summary>
    /// Markup extension that enables passing of the event sender as a parameter to a bound method.
    /// </summary>
    public sealed class EventSenderExtension : MarkupExtension
    {
        /// <inheritdoc/>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}