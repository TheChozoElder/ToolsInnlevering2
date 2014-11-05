using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
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
		internal int maxHp;
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
				maxHp = 12, 
				name = "Flasha"

			};

			var ser = new DataContractJsonSerializer(typeof(Enemy));
			var fileName = @"E:\sak\file.txt";
			var file = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			ser.WriteObject(file, enemy);
			file.SetLength(file.Position);
			file.Close();
		}
	}
}