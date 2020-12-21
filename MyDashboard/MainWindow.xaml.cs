using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MyDashboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Move(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string action = @"files\MyJukebox.appref-ms";
            CreateBat(action);
            RunBat();
            //DeleteBat();
        }


        private void button2_Click(object sender, RoutedEventArgs e)
        {
            string action = @"files\MyMp3Importer.appref-ms";
            CreateBat(action);
            RunBat();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            string action = @"files\DbRecordEditor.appref-ms";
            CreateBat(action);
            RunBat();
        }

        private void CreateBat(string content)
        {
            string filename = "start.bat";

            StreamWriter writer = new StreamWriter(filename);
            writer.WriteLine("start " + "\"Start\"" + " " + content);
            writer.Close();

            Task.Delay(1000);

        }

        private void RunBat()
        {
            Process p = new Process();
            p.StartInfo.FileName = "start.bat";
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
        }

        private void DeleteBat()
        {
            if (File.Exists("start.bat"))
                File.Delete("start.bat");
        }
    }
}
