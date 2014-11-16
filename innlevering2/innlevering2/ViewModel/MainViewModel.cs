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

		#region Properties
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

		private StatsObjectList entities { get; set; }
		private StatsObject selectedObject { get; set; }

		#endregion

		public MainViewModel() {
			entities = new StatsObjectList
			{
				UnnamedEntities = new List<StatsObject>(),
				NamedEntities = new List<StatsObject>()
			};

			path = "";
			InfoPicturePath = "../Assets/mark.png";
			InfoText = "Choose input file.";

			CreateCommands();
		}

		private void CreateCommands() {
			ExportCommand = new RelayCommand(Export);
			FilePathCommand = new RelayCommand(ChangePath);
			ImportCommand = new RelayCommand(Import);
		}


		private void Export() {

			if(UnNamedEntities.Count <= 0) {
				return;
			}

			using(var writer = new StreamWriter(path)) {
				writer.Write((entities.Serialize()));
			}
			InfoText = "Exported to: " + path ;
			InfoPicturePath = "../Assets/save.png";

		}

		private void Import() {

			var jsonStream = new StreamReader(path);
			var jsonString = jsonStream.ReadToEnd();

			entities.Deserialize(jsonString);

			jsonStream.Close();
			RaisePropertyChanged("UnNamedEntities");
			RaisePropertyChanged("NamedEntities");

			InfoText = "Objects imported.";
			InfoPicturePath = "../Assets/good.png";

			SelectedObject = entities.UnnamedEntities[0];

		}

		private void ChangePath() {

			var dlg = new Microsoft.Win32.OpenFileDialog { DefaultExt = ".json", Filter = "JSON Files (*.json)|*.json" };

			var result = dlg.ShowDialog();

			if(result != true) return;

			var jsonStream = new StreamReader(dlg.FileName);
			var jsonString = jsonStream.ReadToEnd();

			try {
				var tempList = new StatsObjectList { UnnamedEntities = new List<StatsObject>() };
				tempList.Deserialize(jsonString);

				jsonStream.Close();
				path = dlg.FileName;
				Import();


			} catch(Exception) {
				InfoText = "Something went wrong. Please choose a valid json file";
				InfoPicturePath = "../Assets/error.png";

			}

		}
	}
}