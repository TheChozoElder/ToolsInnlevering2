﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using innlevering2.Model;

namespace innlevering2.ViewModel {


	/// <summary>
	/// This class contains properties that the main View can data bind to.
	/// <para>
	/// See http://www.galasoft.ch/mvvm
	/// </para>
	/// </summary>
	public class MainViewModel : ViewModelBase {


		#region Commands

		public ICommand ExportCommand
		{
			get;
			private set;
		}

		public ICommand FilePathCommand
		{
			get;
			private set;
		}

		public ICommand ImportCommand
		{
			get;
			private set;
		}
		#endregion

		#region Private fields

		private EnemyList EnemyList { get; set; }
		private string Path { get; set; }
		private bool DeserializeButtonActive { get; set; }

		#endregion

		public MainViewModel()
		{
			EnemyList = new EnemyList { ListOfEnemies = new List<Enemy>() };
			DeserializeButtonActive = true;
			Path = @"E:\sak\file.json";

			CreateCommands();
		}

		private void CreateCommands()
		{
			ExportCommand = new RelayCommand(Export, CanExport);
			FilePathCommand = new RelayCommand(ChangePath);
			ImportCommand = new RelayCommand(Import);
		}

		/// <summary>
		/// The <see cref="WelcomeTitle" /> property's name.
		/// </summary>
		public const string WelcomeTitlePropertyName = "WelcomeTitle";

		private string _welcomeTitle = string.Empty;

		/// <summary>
		/// Gets the WelcomeTitle property.
		/// Changes to that property's value raise the PropertyChanged event. 
		/// </summary>
		public string WelcomeTitle {
			get
			{
				return _welcomeTitle;
			}

			set
			{
				if (_welcomeTitle == value)
				{
					return;
				}

				_welcomeTitle = value;
				RaisePropertyChanged(WelcomeTitlePropertyName);
			}
		}

		private bool CanExport()
		{
			return DeserializeButtonActive;
		}

		private void Export()
		{

			if (EnemyList.ListOfEnemies.Count <= 0)
			{
				SetInfoText("Nothing to export!");
				return;
			}

			using (var writer = new StreamWriter(Path))
			{
				writer.Write((EnemyList.Serialize()));
			}
			SetInfoText("All data exported");
		}

		private void Import()
		{
			//TODO: Fix disable button instead
			if (!DeserializeButtonActive)
				return;

			var jsonStream = new StreamReader(Path);
			var jsonString = jsonStream.ReadToEnd();

			EnemyList.Deserialize(jsonString);

			jsonStream.Close();
			SetInfoText("All data imported");
		}

		private void ChangePath()
		{

			var dlg = new Microsoft.Win32.OpenFileDialog { DefaultExt = ".json", Filter = "JSON Files (*.json)|*.json" };

			var result = dlg.ShowDialog();


			if (result != true) return;

			var jsonStream = new StreamReader(dlg.FileName);
			var jsonString = jsonStream.ReadToEnd();

			try
			{
				var tempList = new EnemyList { ListOfEnemies = new List<Enemy>() };
				tempList.Deserialize(jsonString);

				SetInfoText("File has right format and has deserialized successfully!");
				DeserializeButtonActive = true;

			}
			catch (Exception)
			{
				DeserializeButtonActive = false;
				SetInfoText("Warning! Json file has wrong format.");
			}

			jsonStream.Close();

			Path = dlg.FileName;
		}

		private void SetInfoText(string infoText = "")
		{
			if (DeserializeButtonActive && !infoText.Equals(""))
			{
//				Info.Text = infoText;
			}
			else if (DeserializeButtonActive && infoText.Equals(""))
			{
//				Info.Text = "All systems GO!";
			}
			else
			{
//				Info.Text = "Chosen file format is not supported, importing will be disabled.";
			}
		}


		private void EnterSerializeButton(object sender, System.Windows.Input.MouseEventArgs e)
		{
			if (EnemyList.ListOfEnemies.Count == 0)
			{
				SetInfoText("Nothing to export..");
			}
			else
			{
				SetInfoText("Serializes current data. Exporting to: " + Path);

			}
		}

		private void EnterPathButton(object sender, System.Windows.Input.MouseEventArgs e)
		{
			SetInfoText("Load another file. Current file: " + Path);
		}
		private void EnterDeserializeButton(object sender, System.Windows.Input.MouseEventArgs e)
		{
			SetInfoText("Deserializes current data. Importing from: " + Path);
		}

		private void LeaveField(object sender, System.Windows.Input.MouseEventArgs e)
		{
			SetInfoText();
		}
	}
}