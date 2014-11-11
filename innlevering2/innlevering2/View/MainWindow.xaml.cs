using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Runtime.Serialization;
using Microsoft.Win32;
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

//		string Path { get; set; }

//		private bool DeserializeButtonActive { get; set; }

//		EnemyList EnemyList { get; set; }
		/// <summary>
		/// Initializes a new instance of the MainWindow class.
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();
			Closing += (s, e) => ViewModelLocator.Cleanup();

			//Initialize stuff
//			EnemyList = new EnemyList {ListOfEnemies = new List<Enemy>()};
//			Path = @"E:\sak\file.json";
//			DeserializeButtonActive = true;
		}


//		private void ExportButton(object sender, RoutedEventArgs e)
//		{
//
//			if (EnemyList.ListOfEnemies.Count <= 0)
//			{
//				SetInfoText("Nothing to export!");
//				return;
//			}
//
//			using (var writer = new StreamWriter(Path))
//			{
//				writer.Write((EnemyList.Serialize()));
//			}
//			SetInfoText("All data exported");
//		}
//
//		private void ImportButton(object sender, RoutedEventArgs e)
//		{
//			//TODO: Fix disable button instead
//			if (!DeserializeButtonActive)
//				return;
//
//			var jsonStream = new StreamReader(Path);
//			var jsonString = jsonStream.ReadToEnd();
//
//			EnemyList.Deserialize(jsonString);
//
//			jsonStream.Close();
//			SetInfoText("All data imported");
//		}
//
//		private void ChangePath(object sender, RoutedEventArgs e)
//		{
//
//			var dlg = new Microsoft.Win32.OpenFileDialog {DefaultExt = ".json", Filter = "JSON Files (*.json)|*.json"};
//
//			var result = dlg.ShowDialog();
//
//
//			if (result != true) return;
//
//			var jsonStream = new StreamReader(dlg.FileName);
//			var jsonString = jsonStream.ReadToEnd();
//
//			try {
//				var tempList = new EnemyList { ListOfEnemies = new List<Enemy>() };
//				tempList.Deserialize(jsonString);
//
//				SetInfoText("File has right format and has deserialized successfully!");
//				DeserializeButtonActive = true;
//
//			} catch (Exception) {
//				DeserializeButtonActive = false;
//				SetInfoText("Warning! Json file has wrong format.");
//			}
//
//			jsonStream.Close();
//
//			Path = dlg.FileName;
//		}
//
//		private void SetInfoText(string infoText = "")
//		{
//			if (DeserializeButtonActive && !infoText.Equals(""))
//			{
//				Info.Text = infoText;
//			}
//			else if (DeserializeButtonActive && infoText.Equals(""))
//			{
//				Info.Text = "All systems GO!";
//			}
//			else
//			{
//				Info.Text = "Chosen file format is not supported, importing will be disabled.";
//			}
//		}
//
//
//		private void EnterSerializeButton(object sender, System.Windows.Input.MouseEventArgs e)
//		{
//			if (EnemyList.ListOfEnemies.Count == 0)
//			{
//				SetInfoText("Nothing to export..");
//			} else {
//				SetInfoText("Serializes current data. Exporting to: " + Path);
//				
//			}
//		}
//
//		private void EnterPathButton(object sender, System.Windows.Input.MouseEventArgs e)
//		{
//			SetInfoText("Load another file. Current file: " + Path);
//		}
//		private void EnterDeserializeButton(object sender, System.Windows.Input.MouseEventArgs e)
//		{
//			SetInfoText("Deserializes current data. Importing from: " + Path);
//		}
//
//		private void LeaveField(object sender, System.Windows.Input.MouseEventArgs e)
//		{
//			SetInfoText();
//		}
	}
}