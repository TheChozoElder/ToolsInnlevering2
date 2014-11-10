using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

		string Path { get; set; }

		EnemyList EnemyList { get; set; }
		/// <summary>
		/// Initializes a new instance of the MainWindow class.
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();
			Closing += (s, e) => ViewModelLocator.Cleanup();

			//Initialize stuff
			EnemyList = new EnemyList {ListOfEnemies = new List<Enemy>()};
			Path = @"E:\sak\file.json";
		}


		private void ExportButton(object sender, RoutedEventArgs e)
		{

			if (!EnemyList.ListOfEnemies.Any())
			{
				Console.WriteLine("Nothing to serialize!");
				return;
			}

			using (var writer = new StreamWriter(Path))
			{
				writer.Write((EnemyList.Serialize()));
			}

		}

		private void ImportButton(object sender, RoutedEventArgs e)
		{

			var jsonStream = new StreamReader(Path);
			var jsonString = jsonStream.ReadToEnd();

			EnemyList.Deserialize(jsonString);

			jsonStream.Close();
		}
	}
}