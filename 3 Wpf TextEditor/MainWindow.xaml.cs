using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace _3_Wpf_TextEditor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string fileName;
        public MainWindow()
        {
            InitializeComponent();
            List<string> theme = new List<string>() { "Светлая тема", "Темная тема" };
            themeBox.ItemsSource = theme;
            themeBox.SelectionChanged += themeChange;
            themeBox.SelectedIndex = 0;
        }

        private void themeChange(object sender, SelectionChangedEventArgs e)
        {
            int themeIndex = themeBox.SelectedIndex;
            Uri uri = new Uri("Light.xaml", UriKind.Relative);
            if (themeIndex == 1)
            {
                uri = new Uri("Dark.xaml", UriKind.Relative);
            }
            ResourceDictionary resource = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resource);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string fontName = (string)(sender as ComboBox).SelectedItem;
            if (textBox != null)
                textBox.FontFamily = new FontFamily(fontName);
        }
        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            int fontHeight = Convert.ToInt32((sender as ComboBox).SelectedItem);
            if (textBox != null)
                textBox.FontSize = fontHeight;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            textBox.FontWeight = (textBox.FontWeight != FontWeights.Bold) ? FontWeights.Bold : FontWeights.Normal;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            textBox.FontStyle = (textBox.FontStyle != FontStyles.Italic) ? FontStyles.Italic : FontStyles.Normal;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            textBox.TextDecorations = (textBox.TextDecorations != TextDecorations.Underline) ? TextDecorations.Underline : null;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (textBox != null)
                textBox.Foreground = Brushes.Black;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            if (textBox != null)
                textBox.Foreground = Brushes.Red;
        }

        private void OpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.FileName;
                textBox.Text = File.ReadAllText(fileName);
            }
        }

        private void SaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (File.Exists(fileName))
            {
                File.WriteAllText(fileName, textBox.Text);
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == true)
                {
                    fileName = saveFileDialog.FileName;
                    File.WriteAllText(fileName, textBox.Text);
                }
            }
        }
        private void SaveAsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                fileName = saveFileDialog.FileName;
                File.WriteAllText(fileName, textBox.Text);
            }
        }

        private void CloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (textBox.Text.Length != 0)
            {
                switch (MessageBox.Show("Хотите сохранить изменения?", "Выход", MessageBoxButton.YesNoCancel))
                {
                    case MessageBoxResult.Cancel:
                        break;
                    case MessageBoxResult.Yes:
                        SaveExecuted(sender, e);
                        Application.Current.Shutdown();
                        break;
                    case MessageBoxResult.No:
                        Application.Current.Shutdown();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

    }
}
