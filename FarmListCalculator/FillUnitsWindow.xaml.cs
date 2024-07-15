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

namespace FarmListCalculator
{
    /// <summary>
    /// Interaction logic for FillUnitsWindow.xaml
    /// </summary>
    public partial class FillUnitsWindow : Window
    {
        private int Tribe;
        CurrentUnits CurrentUnits;
        public FillUnitsWindow(int tribe)
        {
            InitializeComponent();
            Tribe = tribe;
            CurrentUnits = CurrentUnits.Instance;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IndexConverter indexConverter = new IndexConverter();
            string tribeName = indexConverter.GetNameByIndex(true, Tribe);
            UpdateLabels(tribeName);
            FillCurrentValues();
        }
        private void UpdateLabels(string tribe)
        {
            string[] units;

            switch (tribe)
            {
                case "Romans":
                    units = new string[]
                    {
                        "Legi", "Praet", "Imper", "Legati", "EI",
                        "EC", "Ram", "Cat", "Chief", "Sett"
                    };
                    break;
                case "Teutons":
                    units = new string[]
                    {
                        "Club", "Spear", "Axe", "Scout", "Pala",
                        "TK", "Ram", "Cat", "Chief", "Sett"
                    };
                    break;
                case "Gauls":
                    units = new string[]
                    {
                        "Phal", "Sword", "PF", "TT", "Druid",
                        "Haedu", "Ram", "Cat", "Chief", "Sett"
                    };
                    break;
                default:
                    units = new string[10];
                    break;
            }
            for (int i = 1; i < 11; i++)
            {
                var label = FindName($"lblUnit{i}") as Label;
                if (label != null)
                    label.Content = units[i - 1];
            }
        }
        private void FillCurrentValues()
        {
            for (int i = 1; i < 11; i++)
            {
                var textBox = FindName($"txtUnit{i}") as TextBox;
                if (textBox != null)
                    textBox.Text = CurrentUnits.Units[i-1] != 0 ? CurrentUnits.Units[i-1].ToString() : "";
            }

        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            int[] units = new int[10];
            for (int i = 1; i < 11; i++)
            {
                var textBox = FindName($"txtUnit{i}") as TextBox;
                if (textBox != null && textBox.Text != "")
                {
                    if (!(int.TryParse(textBox.Text, out units[i-1])))
                    {
                        MessageBox.Show("Only put numbers in the boxes and try again.");
                        return;
                    }
                }
            }
            CurrentUnits.Units = units;
            this.Close();
        }
    }
}
