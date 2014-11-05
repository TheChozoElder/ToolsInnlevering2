using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Windows;
using innlevering2.ViewModel;


namespace innlevering2
{


	[DataContract]
	class Enemy
	{
		[DataMember]
		internal string name;

		[DataMember]
		internal int maxHealth;

		[DataMember]
		internal int scale;

		[DataMember]
		internal int movementSpeed;

		[DataMember]
		internal int regenerateSpeed;

		[DataMember]
		internal bool invisible;

		[DataMember]
		internal int aimingSpeed;
	}

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
			var enemy = new Enemy
			{
				name = "Flasha",
				aimingSpeed = 3,
				invisible = false,
				maxHealth = 12,
				movementSpeed = 10,
				regenerateSpeed = 12,
				scale = 1
			};

			var json = JsonConvert.SerializeObject(enemy);

			var fileName = @"E:\sak\file.txt";

			using (var fs = File.Open(@"E:\sak\file.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
			using (var sw = new StreamWriter(fs))
			using (var jw = new JsonTextWriter(sw))
			{
				jw.Formatting = Formatting.Indented;

				var serializer = new JsonSerializer();
				serializer.Serialize(jw, json);
			}
		}
	}
}