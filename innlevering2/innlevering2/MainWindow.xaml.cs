using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Windows;
using innlevering2.ViewModel;
using innlevering2.Model;


namespace innlevering2
{

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		/// <summary>
		/// Initializes a new instance of the MainWindow class.
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();
			Closing += (s, e) => ViewModelLocator.Cleanup();
		}

		private void TabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{

		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{

			var fileName = @"E:\sak\file.txt";
			var otherFile = @"E:\sak\Smile.txt";

			var enemy = new Enemy
			{
				Name = "Flasha",
				AimingSpeed = 3,
				Invisible = false,
				MaxHealth = 12,
				MovementSpeed = 10,
				RegenerateSpeed = 12,
				Scale = 1
			};

			var path = @"E:\sak\fileb.json";

			// Serialize Character to json string.
			var jsonString = Enemy.Serialize(enemy);

			// Write string to file.
			using (var writer = new StreamWriter(path))
			{
				writer.Write(jsonString);
			}

		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var path = @"E:\sak\smile.json";

			var jsonStream = new StreamReader(path);
			var jsonString = jsonStream.ReadToEnd();

			var savedCharacter = Enemy.Deserialize(jsonString);

//			PopulateView(enemies);
		}
	}
}