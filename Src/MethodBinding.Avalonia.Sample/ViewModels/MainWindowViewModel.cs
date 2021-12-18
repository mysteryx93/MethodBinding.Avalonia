using ReactiveUI;

namespace MethodBinding.Avalonia.Sample.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; set; } = "Welcome to Avalonia!";

    public void TextBlock_Pressed()
    {
        Greeting = "Pressed!";
        this.RaisePropertyChanged(nameof(Greeting));
    }
}