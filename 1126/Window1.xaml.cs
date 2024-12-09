using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace _1126
{
    /// <summary>
    /// Window1.xaml 的互動邏輯
    /// </summary>
    public partial class Window1 : Window
    {
        Color fontColor = Colors.Black;
        Color backgroundColor = Colors.White;

        public Window1()
        {
            InitializeComponent();
            fontColorPicker.SelectedColor = fontColor;

            foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
            {
                fontFamilyComboBox.Items.Add(fontFamily.Source);
            }
            //fontFamilyComboBox.SelectedItem = "Arial";
            fontFamilyComboBox.SelectedIndex = 13;

            fontSizeComboBox.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 36, 48, 72 };
            fontSizeComboBox.SelectedIndex = 3;
        }

        private void NewCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            Window1 Window = new Window1();
            Window.Show();
        }

        private void OpenCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            openFileDialog.DefaultExt = ".rtf";
            openFileDialog.AddExtension = true;

            if (openFileDialog.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
                fileStream.Close();
            }
        }

        private void SaveCommand_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Rich Text Format (*.rtf)|*.rtf|HTML File (*.html)|*.html|All files (*.*)|*.*";
            saveFileDialog.DefaultExt = ".rtf";
            saveFileDialog.AddExtension = true;

            if (saveFileDialog.ShowDialog() == true)
            {
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    if (saveFileDialog.FilterIndex == 1)
                    {
                        range.Save(fileStream, DataFormats.Rtf);
                    }
                    else if (saveFileDialog.FilterIndex == 2)
                    {
                        range.Save(fileStream, DataFormats.Html);
                    }
                }
            }
        }

        private void fontColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue.HasValue)
            {
                fontColor = e.NewValue.Value;
                SolidColorBrush fontBrush = new SolidColorBrush(fontColor);
                rtbEditor.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, fontBrush);
            }
        }

        private void backgroundColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue.HasValue)
            {
                backgroundColor = e.NewValue.Value;
                SolidColorBrush backgroundBrush = new SolidColorBrush(backgroundColor);
                rtbEditor.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, backgroundBrush);
            }
        }

        private void fontSizeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (fontSizeComboBox.SelectedItem != null)
            {
                rtbEditor.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, fontSizeComboBox.SelectedItem);
            }
        }

        private void fontFamilyComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (fontFamilyComboBox.SelectedItem != null)
            {
                rtbEditor.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, fontFamilyComboBox.SelectedItem);
            }
        }

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var property_bold = rtbEditor.Selection.GetPropertyValue(TextElement.FontWeightProperty);
            boldButton.IsChecked = (property_bold != DependencyProperty.UnsetValue) && (property_bold.Equals(FontWeights.Bold));

            var property_italic = rtbEditor.Selection.GetPropertyValue(TextElement.FontStyleProperty);
            italicButton.IsChecked = (property_italic != DependencyProperty.UnsetValue) && (property_italic.Equals(FontStyles.Italic));

            var property_underline = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            underlineButton.IsChecked = (property_underline != DependencyProperty.UnsetValue) && (property_underline.Equals(TextDecorations.Underline));

            var property_fontFamily = rtbEditor.Selection.GetPropertyValue(TextElement.FontFamilyProperty);
            fontFamilyComboBox.SelectedItem = property_fontFamily.ToString();

            var property_fontSize = rtbEditor.Selection.GetPropertyValue(TextElement.FontSizeProperty);
            fontSizeComboBox.SelectedItem = property_fontSize;

            var property_fontColor = rtbEditor.Selection.GetPropertyValue(TextElement.ForegroundProperty);
            fontColorPicker.SelectedColor = ((SolidColorBrush)property_fontColor).Color;

            var property_backgroundColor = rtbEditor.Selection.GetPropertyValue(TextElement.BackgroundProperty);
            if (property_backgroundColor != null && property_backgroundColor is SolidColorBrush solidColorBrush)
            {
                backgroundColorPicker.SelectedColor = solidColorBrush.Color;
            }
            else
            {
                backgroundColorPicker.SelectedColor = Colors.Transparent; // 或任何預設顏色
            }

        }

        private void trashButton_Click(object sender, RoutedEventArgs e)
        {
            rtbEditor.Document.Blocks.Clear();
        }
    }
}

