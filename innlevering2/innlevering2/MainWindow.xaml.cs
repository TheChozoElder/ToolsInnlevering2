using System.Collections.Generic;
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

			var listaaa = new EnemyList() {ListOfEnemies = new List<Enemy>()};

			var enemy1 = new Enemy
			{
				Name = "Karl",
				AimingSpeed = 3,
				Invisible = false,
				MaxHealth = 12,
				MovementSpeed = 10,
				RegenerateSpeed = 12,
				Scale = 1
			}; 
			var enemy2 = new Enemy
			{
				Name = "Adrian",
				AimingSpeed = 3,
				Invisible = true,
				MaxHealth = 12,
				MovementSpeed = 10,
				RegenerateSpeed = 12,
				Scale = 13
			};

			listaaa.ListOfEnemies.Add(enemy1);
			listaaa.ListOfEnemies.Add(enemy2);

			var path = @"E:\sak\file.json";

			using (var writer = new StreamWriter(path))
			{
				writer.Write((listaaa.Serialize()));
			}

		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var listaaa = new EnemyList { ListOfEnemies = new List<Enemy>() };


			var path = @"E:\sak\file.json";

			var jsonStream = new StreamReader(path);
			var jsonString = jsonStream.ReadToEnd();


			listaaa.Deserialize(jsonString);



//			var enemy = JsonConvert.DeserializeObject<EnemyJson>(jsonString);
//
//			enemyList.Add(enemy);




//			var savedCharacter = Enemy.Deserialize(jsonString);

//			PopulateView(enemies);
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{

		}
	}
}