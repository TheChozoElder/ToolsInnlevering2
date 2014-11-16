using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Data;
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

		#region public properties
		public string UnNamedEntitiesName = "UnNamedEntities";

		public List<StatsObject> UnNamedEntities {
			get {
				return entities.UnnamedEntities;
			}
			set {
				entities.UnnamedEntities = value;
			}
		}

		public string NamedEntitiesName = "NamedEntities";
		public List<StatsObject> NamedEntities {
			get {
				return entities.NamedEntities;
			}
			set {
				entities.NamedEntities = value;
			}
		}

		public string SelectedObjectName = "SelectedObject";
		public StatsObject SelectedObject
		{
			get
			{
				return selectedObject;
			}
			set
			{

				selectedObject = value;
				RaisePropertyChanged();
			}
		}

		public string InfoTextName = "InfoText";
		public string InfoText {
			get {
				return infoText;
			}
			set {

				infoText = value;
				RaisePropertyChanged();
			}
		}

		public string InfoPicturePathName = "InfoPicturePath";
		public string InfoPicturePath {
			get {
				return infoPicturePath;
			}
			set {

				infoPicturePath = value;
				RaisePropertyChanged();
			}
		}
		#endregion

		#region Commands

		public ICommand ExportCommand {
			get;
			private set;
		}

		public ICommand FilePathCommand {
			get;
			private set;
		}

		public ICommand ImportCommand {
			get;
			private set;
		}
		#endregion

		#region Private fields

		private string path { get; set; }
		private string infoText { get; set; }
		private string infoPicturePath { get; set; }
		private bool canDeserialize { get; set; }

		private StatsObjectList entities { get; set; }
		private StatsObject selectedObject { get; set; }

		#endregion

		public MainViewModel() {
			entities = new StatsObjectList
			{
				UnnamedEntities = new List<StatsObject>(),
				NamedEntities = new List<StatsObject>()
			};

			InfoPicturePath = "../Assets/mark.png";
			InfoText = "Choose input file.";
			canDeserialize = false;

			CreateCommands();
		}

		private void CreateCommands() {
			ExportCommand = new RelayCommand(Export);
			FilePathCommand = new RelayCommand(ChangePath);
			ImportCommand = new RelayCommand(Import);
		}

		/// <summary>
		/// Exports all current data to selected json file
		/// </summary>
		private void Export() {

			if(!canDeserialize) {
				InfoPicturePath = "../Assets/mark.png";
				InfoText = "Cannot export, please change file..";
				return;
			}

			using(var writer = new StreamWriter(path)) {
				writer.Write((entities.Serialize()));
			}

			InfoText = "Exported to: " + path ;
			InfoPicturePath = "../Assets/save.png";
		}

		/// <summary>
		/// Imports data from chosen file
		/// </summary>
		private void Import()
		{
			if (path == null || !canDeserialize) {

				InfoPicturePath = "../Assets/mark.png";
				InfoText = "Cannot import, please change file..";
				return;
			}

			var jsonStream = new StreamReader(path);
			var jsonString = jsonStream.ReadToEnd();

			entities.Deserialize(jsonString);

			jsonStream.Close();

			RaisePropertyChanged("UnNamedEntities");
			RaisePropertyChanged("NamedEntities");

			InfoText = "Objects imported.";
			InfoPicturePath = "../Assets/good.png";

			//Set selected object to the first in the list
			if (entities.UnnamedEntities.Count > 0)
			{
				SelectedObject = entities.UnnamedEntities[0];
			}
		}

		/// <summary>
		/// Changes path to the file you are working with
		/// </summary>
		private void ChangePath() {

			var dlg = new Microsoft.Win32.OpenFileDialog { DefaultExt = ".json", Filter = "JSON Files (*.json)|*.json" };

			var result = dlg.ShowDialog();

			//if noe file is chosen
			if(result != true) return;

			//Tests json format
			var jsonStream = new StreamReader(dlg.FileName);
			var jsonString = jsonStream.ReadToEnd();

			try {
				var tempList = new StatsObjectList { UnnamedEntities = new List<StatsObject>() };
				tempList.Deserialize(jsonString);

				jsonStream.Close();

				canDeserialize = true;

				path = dlg.FileName;
				Import();

				//If format is not supported
			} catch(Exception) {
				InfoText = "Please choose a valid json file. (the one unity made, remember?)";
				InfoPicturePath = "../Assets/error.png";
				canDeserialize = false;
			}
		}
	}
}