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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FilesTOFileNames
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void ListToClipboard()
        {

            //Get all filenames
            string chaine = GetOneStringFromListBox();

            //Copy to clipboard
            Clipboard.SetText(chaine);
        }

        void _lb_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);

                    //Add each new file or directory
                    foreach (string name in data)
                    {
                        //Folder ? or file ?
                        if (System.IO.Directory.Exists(name))
                        {
                            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(name);
                            var fis = di.EnumerateFiles("*", System.IO.SearchOption.AllDirectories);
                            foreach (var fi in fis)
                                _lb.Items.Add(fi.FullName);
                        }
                        else
                        {
                            _lb.Items.Add(name);
                        }
                    }

                    ListToClipboard();

                    Title = _lb.Items.Count + " filenames copied in clipboard";
                }
            }
            catch (Exception ex)
            {
                Title = ex.Message;
            }
        }

        string GetOneStringFromListBox()
        {
            string txt = "";
            foreach (string item in _lb.Items)
            {
                if (txt != "")
                    txt += "\n";
                txt += item;
            }
            return txt;
        }

        private void _lb_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //clear ?
            var rep = MessageBox.Show("Clear list ?", "?", MessageBoxButton.YesNo, MessageBoxImage.Question);

            //clear
            if (rep == MessageBoxResult.Yes)
                _lb.Items.Clear();
        }
    }
}