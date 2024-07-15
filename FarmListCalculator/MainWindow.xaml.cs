using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HtmlAgilityPack;

namespace FarmListCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<FarmList> FarmLists;
        private int CurrentTribe;
        private bool LoadedStatus;
        CurrentUnits CurrentUnits;
        public MainWindow()
        {
            InitializeComponent();
            FarmLists = new List<FarmList>();
            CurrentTribe = 0;
            LoadedStatus = false;
            CurrentUnits = CurrentUnits.Instance;
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.ContainsText(TextDataFormat.Html))
            {
                string htmlContent = Clipboard.GetText(TextDataFormat.Html);
                bool result = VerifyActiveTabItem(htmlContent);
                if (result)
                {
                    FarmLists.Clear();
                    ResetCheckBoxContents();
                    LoadFarmlists(htmlContent);
                }

                else
                    MessageBox.Show("Clipboard does not contain the Farm List data. Read and follow the instructions above the button.");
            }
            else
            {
                MessageBox.Show("Clipboard does not contain HTML data. Read and follow the instructions above the button.");
            }
        }
        private bool VerifyActiveTabItem(string htmlContent)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            // Find the tabItem with class "active"
            var activeTabItem = doc.DocumentNode.SelectSingleNode("//a[contains(@class, 'tabItem') and contains(@class, 'active')]");
            if (activeTabItem != null)
            {
                // Check if href contains the correct URL
                var href = activeTabItem.GetAttributeValue("href", string.Empty);
                if (href.Contains("id=39&amp;gid=16&amp;tt=99"))
                {
                    return true;
                }
            }
            return false;
        }

        private void LoadFarmlists(string htmlContent)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            // Select all all farmlists, which are not collapsed
            var expandedFarmListWrappers = doc.DocumentNode.SelectNodes("//div[contains(@class, 'farmListWrapper') and not(contains(@class, 'collapsed'))]");

            if (expandedFarmListWrappers == null || expandedFarmListWrappers.Count < 1 || expandedFarmListWrappers.Count > 16)
            {
                MessageBox.Show("1 - 16 uncollapsed Farm Lists supported. Show/Hide some Farm Lists to fit the criteria and try again.");
                return;
            }
            IndexConverter indexConverter = new IndexConverter();
            foreach (var wrapper in expandedFarmListWrappers)
            {
                var farmListNameNode = wrapper.SelectSingleNode(".//div[@class='farmListName']//div[@class='name']");
                var farmListName = farmListNameNode.InnerText.Trim();

                var farmListItems = new List<FarmListItem>();
                var itemRows = wrapper.SelectNodes(".//tbody//tr[contains(@class, 'slot')]");
                if (itemRows != null)
                {
                    foreach (var row in itemRows)
                    {
                        var itemNameNode = row.SelectSingleNode(".//td[@class='target']//a");
                        var itemName = itemNameNode.InnerText.Trim();

                        var distanceNode = row.SelectSingleNode(".//td[@class='distance']//span");
                        double.TryParse(distanceNode?.InnerText.Trim(), out var distance);

                        var units = new Dictionary<string, int>();
                        var unitNodes = row.SelectNodes(".//td[@class='troops']//i[contains(@class, 'unit_small')]");
                        if (CurrentTribe == 0)
                            int.TryParse(unitNodes.First().GetClasses().First(c => c.StartsWith("tribe"))[^1].ToString(), out CurrentTribe);
                        foreach (var unitNode in unitNodes)
                        {
                            var unitClass = unitNode.GetClasses().First(c => c.StartsWith("t") && c.Length > 1 && char.IsDigit(c[1]));
                            var unitValueNode = unitNode.SelectSingleNode("../span[@class='value']");
                            int.TryParse(unitValueNode.InnerText.Trim(), out var unitCount);
                            int.TryParse(unitClass[^1].ToString(), out int unitIndex);
                            var unitName = indexConverter.GetNameByIndex(false, CurrentTribe, unitIndex);
                            units[unitName] = unitCount;
                        }

                        var farmListItem = new FarmListItem(itemName, distance, units);
                        farmListItems.Add(farmListItem);
                    }
                }

                var farmList = new FarmList(farmListName, farmListItems);
                FarmLists.Add(farmList);
            }
            UpdateCheckBoxContents(FarmLists);
            LoadedStatus = true;
        }

        private void UpdateCheckBoxContents(List<FarmList> farmLists)
        {
            for (int i = 0; i < 16; i++)
            {
                var checkBox = FindName($"chckFL{i}") as CheckBox;
                if (checkBox != null)
                {
                    if (i < farmLists.Count)
                    {
                        checkBox.Content = farmLists[i].Name.Length <= 14 ? farmLists[i].Name : farmLists[i].Name.Substring(0, 14) + "...";
                        checkBox.ToolTip = farmLists[i].Name;
                    }
                    else
                        checkBox.IsEnabled = false;
                }
            }
        }

        private void ResetCheckBoxContents()
        {
            for (int i = 0; i < 16; i++)
            {
                var checkBox = FindName($"chckFL{i}") as CheckBox;
                if (checkBox != null)
                {
                    checkBox.Content = "None";
                    checkBox.ToolTip = null;
                }
            }
        }

        private void btnGetUnits_Click(object sender, RoutedEventArgs e)
        {
            if (!LoadedStatus)
            {
                MessageBox.Show("Load Farm Lists before filling your units in.");
                return;
            }
            FillUnitsWindow fillUnitsWindow = new FillUnitsWindow(CurrentTribe);
            fillUnitsWindow.Closed += fillUnitsWindow_Closed;
            fillUnitsWindow.Left = this.Left + 150;
            fillUnitsWindow.Top = this.Top + 150;
            fillUnitsWindow.Show();
        }
        private void fillUnitsWindow_Closed(object? sender, EventArgs e)
        {
            if (CurrentUnits.Units.All(value => value == 0))
                lblFillSucc.Content = "No units were filled!";
            else
                lblFillSucc.Content = "Units filled successfully!";
            lblFillSucc.Visibility = Visibility.Visible;
        }

        private void btnCalc_Click(object sender, RoutedEventArgs e)
        {
            List<string> selectedFLCheckBoxes = new List<string>();
            for (int i = 0; i <= 15; i++)
            {
                var checkBox = FindName($"chckFL{i}") as CheckBox;
                if (checkBox != null && checkBox.IsChecked == true)
                {
                    selectedFLCheckBoxes.Add(checkBox.ToolTip?.ToString() ?? "");
                }
            }
            if (!selectedFLCheckBoxes.Any())
            {
                MessageBox.Show("Select Farm Lists for the calculation.");
                return;
            }
            int TSLevel;
            if (string.IsNullOrWhiteSpace(txtTSLevel.Text))
            {
                TSLevel = 0;
            }
            else if (!int.TryParse(txtTSLevel.Text, out TSLevel))
            {
                MessageBox.Show("Tournament Square level has to be a number.");
                return;
            }
            double TSCoefficient = 1 + TSLevel * 0.2;
            var selectedFarmLists = FarmLists
                .Where(fl => selectedFLCheckBoxes.Contains(fl.Name))
                .ToList();

            updateFarmListItems(selectedFarmLists, TSCoefficient);

            int? delay;
            if (int.TryParse(txtDelay.Text, out int parsedDelay))
                delay = parsedDelay;
            else
                delay = null;

            if (delay != null)
            {
                if (delay == 0)
                {
                    MessageBox.Show("Use delay larger than 0.");
                    return;
                }
                int[] unitCounts = calculateRequiredUnits(selectedFarmLists, delay.Value);
                string unitCountsStr = "";
                IndexConverter indexConverter = new IndexConverter();
                for (int i = 0; i < unitCounts.Length; i++)
                {
                    unitCountsStr += unitCounts[i] > 0 ? (unitCounts[i].ToString() + " " + indexConverter.GetNameByIndex(false, CurrentTribe, i + 1) + ", ") : "";
                }
                MessageBox.Show($"Required units to keep sending FL every {delay.Value} minutes: {unitCountsStr[..^2]}");
            }
            else
            {
                int minDelay = calculateRequiredDelay(selectedFarmLists);
                MessageBox.Show($"Minimal possible delay to keep sending the selected farmlists: {minDelay} minutes.");

            }

        }

        private void updateFarmListItems(List<FarmList> farmLists, double TSCoefficient)
        {
            UnitSpeeds unitSpeeds = new UnitSpeeds();
            IndexConverter indexConverter = new IndexConverter();
            foreach (var fl in farmLists)
            {
                foreach (var target in fl.Targets)
                {
                    target.RealDistance = target.Distance > 20 ? 20 + (target.Distance - 20) / TSCoefficient : target.Distance;
                    int slowestSpeed = 20;
                    foreach (var unit in target.Units)
                    {
                        int speed = unitSpeeds.GetUnitSpeed(indexConverter.GetNameByIndex(true, CurrentTribe), unit.Key);
                        if (speed < slowestSpeed)
                            slowestSpeed = speed;
                    }
                    target.TimeToRotate = (int)Math.Round(3600 / slowestSpeed * target.RealDistance.Value) * 2;
                }
            }
        }
        private int[] calculateRequiredUnits(List<FarmList> farmLists, int delay)
        {
            IndexConverter indexConverter = new IndexConverter();
            int[] unitCounter = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
            foreach (var fl in farmLists)
            {
                foreach (var target in fl.Targets)
                {
                    if (target.TimeToRotate < delay * 60)
                        continue;
                    int startsToRotate = 1;
                    if (target.TimeToRotate.HasValue)
                    {
                        startsToRotate = (int)Math.Ceiling(target.TimeToRotate.Value / (double)(delay * 60));
                    }
                    foreach (var unit in target.Units)
                    {
                        int unitIndex = indexConverter.GetIndexByName(false, tribeIndex: CurrentTribe, unitName: unit.Key);
                        unitCounter[unitIndex - 1] += unit.Value * startsToRotate;
                    }
                }
            }
            return unitCounter;
        }

        private int calculateRequiredDelay(List<FarmList> farmLists)
        {
            int delay = 0;
            for (int i = 1; i < 360; i++)
            {
                int[] requiredUnits = calculateRequiredUnits(farmLists, i);
                for (int j = 0; j < 10; j++)
                {
                    if (requiredUnits[j] > CurrentUnits.Units[j])
                        break;
                    if (j == 9)
                        delay = i;
                }
                if (delay > 0)
                    break;
            }
            return delay;

        }
    }
}