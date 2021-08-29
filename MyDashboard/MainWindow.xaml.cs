using MyDashboard.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MyDashboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string _fileName = "actions.json";
        int _itemsperrow = 0;
        int _actioncount = 0;

        List<Button> _buttons = new List<Button>();
        ActionModel.ActionCollection _actionCollection;

        public MainWindow()
        {
            InitializeComponent();
            FillActionList();
            FillButtonList();
            AddButtonsToWrappanel();
        }

        private void Window_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Window_Move(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            int id = (int)b.Tag;
            ButtonHandler(id);
        }

        private void AddButtonsToWrappanel()
        {
            foreach (var btn in _buttons)
            {
                wrappanel.Children.Add(btn);
            }
        }

        private void FillButtonList()
        {
            foreach (var action in _actionCollection.Actions)
            {
                Button button = GetButton(action);
                _buttons.Add(button);
            }
        }

        private Button GetButton(ActionModel.Action action)
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            string image = appPath + "Images\\" + action.Image;

            Button button = new Button();

            StackPanel stackPanel = new StackPanel();

            TextBlock textBlock = new TextBlock
            {
                Text = action.Caption,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            Image img = new Image // action.Caption;
            {
                Source = new BitmapImage(new Uri(image, UriKind.Absolute)),
                Width = 60,
                Height = 60
            };

            stackPanel.Children.Add(img);
            stackPanel.Children.Add(textBlock);

            button.Content = stackPanel;
            button.Tag = action.ID;
            button.Click += new RoutedEventHandler(button_Click);

            return button;
        }

        private void FillActionList()
        {
            if (!File.Exists(_fileName))
                return;

            var RawJSON = File.ReadAllText(_fileName);

            // get the rowperitem
            ActionModel.Rootobject settings = JsonConvert.DeserializeObject<ActionModel.Rootobject>(RawJSON);
            _itemsperrow = settings.RowItems;

            // get action list
            _actionCollection = JsonConvert.DeserializeObject<ActionModel.ActionCollection>(RawJSON);
            _actioncount = _actionCollection.Actions.Count;

            var height = (int)Math.Ceiling((double)_actioncount / (double)_itemsperrow);

            if (_itemsperrow < 2)
                this.textblockHeader.Visibility = Visibility.Hidden;

            this.Height = (height * 120) + 60;
            this.Width = (_itemsperrow * 120) + 20;
        }

        private void ButtonHandler(int id)
        {
            ActionModel.Action action = _actionCollection.Actions[id];
            string path = action.Start;
            string cmd = action.Command;
            string file = "";
            string command = action.Command;

            if (String.IsNullOrEmpty(path))
            {
                CreateBat(command);
                file = "start.bat";
            }
            else
                file = command;

            RunBat(file, path);
            this.WindowState = WindowState.Minimized;
        }

        private void CreateBat(string content)
        {
            string filename = "start.bat";
            StreamWriter writer = new StreamWriter(filename);
            writer.WriteLine("start " + "\"Start\"" + " " + content);
            writer.Close();
            Task.Delay(1000);
        }

        private void RunBat(string fileName, string workingDirectory = "")
        {
            fileName = Path.Combine(workingDirectory, fileName);

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = fileName;
            startInfo.WorkingDirectory = workingDirectory;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;

            Process.Start(startInfo);
        }

    }
}
