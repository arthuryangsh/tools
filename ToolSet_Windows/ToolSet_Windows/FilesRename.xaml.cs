using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ToolSet_Windows {
	/// <summary>
	/// Interaction logic for FilesRename.xaml
	/// </summary>
	public partial class FilesRename : Window {
		private	string	path	= null;
		public	FilesRename_binding	binding_data;

		public FilesRename()
		{
			InitializeComponent();

			binding_data	= new FilesRename_binding();
			DataContext	= binding_data;
		}

		// Import paper list
		private void Button_Click_import(object sender, RoutedEventArgs e)
		{
			// Get File
			OpenFileDialog dialog	= new OpenFileDialog();
			dialog.Title		= "Open TXT File";
			dialog.Filter		= "TXT file|*.txt";
			if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				string	filename	= dialog.FileName;

				// Open and import
				System.IO.StreamReader	file	= new System.IO.StreamReader(filename);  
				string		line;
				string		title	= null;
				List<string>	author	= new List<string> ();
				uint		index	= 1;
				while(true){
					line	= file.ReadLine();
					
					if(line== null) {
						if(title!=null && author.Count!=0) {
							// Last paper
							binding_data.list_paper.Add(new FilesRename_paper() {
								ID		= index++,
								author		= author,
								title		= title,
								pdf_exist	= false,
							});
						}

						break;
					}

					if(author.Count==0) {
						if(title==null) {
							title	= line;
						} else {
							foreach( string s in line.Split(',')){
								author.Add(s.Trim());
							}
						}
					}

					if(line.Trim()=="") {
						// Blank line means this paper over
						binding_data.list_paper.Add(new FilesRename_paper() {
							ID		= index++,
							author		= author,
							title		= title,
							pdf_exist	= false,
						});

						title	= null;
						author	= new List<string> ();
					}
				}
				file.Close();  
			} else {
				System.Windows.MessageBox.Show("Fail to import");
			}
		}

		// Set PDF directory 
		private void Button_Click_directory(object sender, RoutedEventArgs e)
		{
			// Get Directory
			FolderBrowserDialog dialog	= new FolderBrowserDialog();
			dialog.Description		= "请指定数据文件目录";
			if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				path	= dialog.SelectedPath+@"\";
			}

			// Search for PDF of each paper
			foreach(FilesRename_paper paper in binding_data.list_paper) {
				// Last name of the first author and first 3 words
				List<string>	author0_words	= new List<string>(paper.author[0].Split(' '));
				string	filename	= "";
				if(author0_words[author0_words.Count-2]=="de" || author0_words[author0_words.Count-2]=="Santa") {
					filename	+= author0_words[author0_words.Count-2].Replace("'","")+"_";
				}

				filename	+= author0_words[author0_words.Count-1].Replace("'","")+"_";
				
				List<string>	title_words	= new List<string>(paper.title.Split(' '));

				for(int i=0;i<title_words.Count;i++){
					if(i<3){
						filename	+= System.Text.RegularExpressions.Regex.Replace(title_words[i], "[?!\",:'+&/]", "")+"_";
					} else
						break;
				}

				filename		+= "CVPR_2017_paper.pdf";

				if(File.Exists(path+@"\"+filename)) {
					paper.filename	= filename;
					paper.pdf_exist	= true;
				} else {
					paper.pdf_exist	= false;
				}
			}
		}

		// Rename and exit 
		private void Button_Click_rename(object sender, RoutedEventArgs e)
		{
			if(path==null)
				return;

			// Rename
			foreach(FilesRename_paper paper in binding_data.list_paper) {
				string	new_filename	= string.Format("{0:D3}", paper.ID)+" "+paper.filename;
				if(paper.filename!=null)
					File.Move(path+@"\"+paper.filename, path+@"\"+new_filename);
			}

			// Write to a list
			System.IO.StreamWriter	file	= new System.IO.StreamWriter(path+@"\list.txt");  
			foreach(FilesRename_paper paper in binding_data.list_paper) {
				file.WriteLine(string.Format("{0:D3}", paper.ID));
				file.WriteLine(paper.title);
				file.WriteLine(string.Join(", ", paper.author));
				file.WriteLine("");
			}

			file.Close();

			// Exit
			this.Close();
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

	// Binding data
	public class FilesRename_binding : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;
		
		private	ObservableCollection<FilesRename_paper>	_list_paper	= new ObservableCollection<FilesRename_paper>();	// Paper list	
		public	ObservableCollection<FilesRename_paper>	list_paper	{ get { return _list_paper;	} set { _list_paper	= value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(MethodBase.GetCurrentMethod().Name.Substring(4))); } }
	}
	
	// Paper
	public class FilesRename_paper : INotifyPropertyChanged  {
		public event PropertyChangedEventHandler PropertyChanged;

		private	UInt32		_ID		;			// ID
		private	List<string>	_author		= new List<string>();	// 
		private	string		_title		;			// 
		private	bool		_pdf_exist	= false;		// If there is PDF
		private	string		_filename	= null;			// If there is PDF

		public	UInt32		ID		{ get { return _ID;		} set { _ID		= value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(MethodBase.GetCurrentMethod().Name.Substring(4))); } }
		public	List<string>	author		{ get { return _author;		} set { _author		= value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(MethodBase.GetCurrentMethod().Name.Substring(4))); } }
		public	string		title		{ get { return _title;		} set { _title		= value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(MethodBase.GetCurrentMethod().Name.Substring(4))); } }
		public	bool		pdf_exist	{ get { return _pdf_exist;	} set { _pdf_exist	= value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(MethodBase.GetCurrentMethod().Name.Substring(4))); } }
		public	string		filename	{ get { return _filename;	} set { _filename	= value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(MethodBase.GetCurrentMethod().Name.Substring(4))); } }
	}


	// =================== XAML Converters ===================
	public class Converter_string_join : IValueConverter {

		public object Convert(object value, Type targetType, object parameters, System.Globalization.CultureInfo culture)
		{
			return string.Join(@", ", ((List<string>)value).ToArray());
		}

		public object ConvertBack(object value, Type targetType, object parameters, System.Globalization.CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
