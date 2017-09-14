using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToolSet_Windows {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow_Binding binding_data;

		public MainWindow()
		{
			InitializeComponent();

			// Initial binding data and set to the window
			binding_data	= new MainWindow_Binding();
			DataContext	= binding_data;

			binding_data.functions.Add(new MainWindow_Function(new FilesRename_Function()));
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			MainWindow_Function	f	= binding_data.functions.First(x=>x.name==((Button)sender).Tag.ToString());
			f.function.go();
			this.Close();
		}
	}

	public interface Function_interface {
		void go();
		string name		{get;}
		string description	{get;}
	}

	// Binding Data
	public class MainWindow_Binding : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;
		
		private	List<MainWindow_Function>	_functions	= new List<MainWindow_Function> ();
		
		public	List<MainWindow_Function>	functions	{ get { return _functions	;} set { _functions	= value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(MethodBase.GetCurrentMethod().Name.Substring(4))); } }
		
	}

	// Function List type
	public class MainWindow_Function : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;
		public	Function_interface	function;
		public	MainWindow_Function(Function_interface _f)
		{
			function	= _f;
			name		= function.name;
			description	= function.description;
		}
		
		private	string	_name		;
		private	string	_description	;
		
		public	string	name		{ get { return _name		;} set { _name		= value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(MethodBase.GetCurrentMethod().Name.Substring(4))); } }	
		public	string	description	{ get { return _description	;} set { _description	= value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(MethodBase.GetCurrentMethod().Name.Substring(4))); } }
	}
}
