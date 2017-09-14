using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ToolSet_Windows {
	/// <summary>
	/// Interaction logic for FilesRename.xaml
	/// </summary>
	public partial class FilesRename : Window {
		public FilesRename()
		{
			InitializeComponent();
		}
	}

	public class FilesRename_Function : Function_interface{
		public FilesRename_Function()
		{
			name		= "FilesRename";
			description	= "Rename a batch of files by programming";
		}

		public	FilesRename	function_window;
		public	void go(){
			function_window	= new FilesRename ();
			function_window.Show();
		}
		public	string name		{get;}
		public	string description	{get;}
	}
}
