namespace _2048_Infinity_Merge.App;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new MainPage()) { Title = "2048 Infinity Merge.App" };
	}
}
