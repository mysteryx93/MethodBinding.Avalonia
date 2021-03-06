using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace MethodBinding.Avalonia
{
    /// <summary>
    /// Markup extension that enables passing of EventArgs data as a parameter to a bound method.
    /// </summary>
    public sealed class EventArgsExtension : MarkupExtension
    {
        /// <summary>
        /// Gets or sets the path to the binding source property.
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// Gets or sets the converter to use.
        /// </summary>
        public IValueConverter? Converter { get; set; }

        /// <summary>
        /// Gets or sets the parameter to pass to the <see cref="Converter"/>.
        /// </summary>
        public object? ConverterParameter { get; set; }

        /// <summary>
        /// Gets or sets the converter target type to pass to the <see cref="Converter"/>. Default is '<see cref="object"/>'.
        /// </summary>
        public Type ConverterTargetType { get; set; } = typeof(object);

        /// <summary>
        /// Gets or sets the culture in which to evaluate the converter.
        /// </summary>
        // TODO: [TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
        public CultureInfo? ConverterCulture { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgsExtension"/> class.
        /// </summary>
        public EventArgsExtension() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventArgsExtension"/> class using the specified path.
        /// </summary>
        public EventArgsExtension(string path)
        {
            this.Path = path;
        }

        /// <inheritdoc/>
        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        internal object? GetArgumentValue(EventArgs eventArgs)
        {
            if (string.IsNullOrEmpty(Path)) {
                return eventArgs;
            }

            object? value = PropertyPathHelper.Evaluate(Path, eventArgs);

            if (Converter != null)
                value = Converter.Convert(value, ConverterTargetType, ConverterParameter, ConverterCulture ?? CultureInfo.CurrentUICulture);

            return value;
        }
    }
}