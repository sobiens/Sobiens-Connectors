using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using Sobiens.Office.SharePointOutlookConnector.BLL;

namespace Sobiens.Office.SharePointOutlookConnector
{
    public partial class AlertEditForm : Form
    {
        private EUSiteSetting siteSetting = null;
        private string webUrl = String.Empty;
        private string listName = String.Empty;
        private string listID = String.Empty;
        private EUAlert CurrentAlert = null;
        public AlertEditForm()
        {
            InitializeComponent();
        }

        private void AlertMaintenanceForm_Load(object sender, EventArgs e)
        {
            Initialize();
        }

        public void Initialize(EUSiteSetting _siteSetting, string _webUrl, string _listName, string _listID, EUAlert _alert)
        {
            siteSetting = _siteSetting;
            webUrl = _webUrl;
            listName = _listName;
            listID = _listID;
            CurrentAlert = _alert;
        }

        private List<EUField> fields = null;
        public List<EUField> Fields
        {
            get
            {
                if(fields == null)
                    fields = SharePointManager.GetFields(siteSetting, webUrl, listName);
                return fields;
            }
        }


        public void Initialize()
        {
            SelectedListNameLabel.Text = listName;
            TitleTextBox.Text = CurrentAlert.Title.Replace("[SPOutlookConnector]", "");

            switch (CurrentAlert.EventType)
            {
                case "All":
                    AllRadioButton.Checked = true;
                    break;
                case "Add":
                    AddRadioButton.Checked = true;
                    break;
                case "Modify":
                    ModifyRadioButton.Checked = true;
                    break;
                case "Delete":
                    DeleteRadioButton.Checked = true;
                    break;
            }
            switch (CurrentAlert.AlertFrequency)
            {
                case "Immediate":
                    ImmediateRadioButton.Checked = true;
                    break;
                case "Daily":
                    DailyRadioButton.Checked = true;
                    SetHour();
                    break;
                case "Weekly":
                    WeeklyRadioButton.Checked = true;
                    SetWeekDay();
                    SetHour();
                    break;
            }
            if (HoursComboBox.SelectedItem == null)
                HoursComboBox.SelectedIndex = 12;
            if (WeekDayComboBox.SelectedItem == null)
                WeekDayComboBox.SelectedIndex = 0;

            SetTimeControlsEnability();
            FillFilters();
        }

        public void SetHour()
        {
            string selectedHour = CurrentAlert.GetTime().ToString();
            if (selectedHour.Length == 1)
                selectedHour = "0" + selectedHour;
            selectedHour += ":00";
            for (int i = 0; i < HoursComboBox.Items.Count;i++ )
            {
                if (HoursComboBox.Items[i].ToString() == selectedHour)
                    HoursComboBox.SelectedIndex = i;
            }
        }

        public void SetWeekDay()
        {
            int selectedDayIndex = CurrentAlert.GetWeekDay() ;
            if (selectedDayIndex == 0)
                selectedDayIndex = 6;
            else
                selectedDayIndex--;
            WeekDayComboBox.SelectedIndex = selectedDayIndex;
        }

        private void SaveFilterButton_Click(object sender, EventArgs e)
        {
            CurrentAlert.Title = TitleTextBox.Text;
            if (ImmediateRadioButton.Checked == true)
            {
                CurrentAlert.AlertFrequency = "Immediate";
            }
            else if (DailyRadioButton.Checked == true)
            {
                CurrentAlert.AlertFrequency = "Daily";
            }
            else if (WeeklyRadioButton.Checked == true)
            {
                CurrentAlert.AlertFrequency = "Weekly";
            }

            if (AllRadioButton.Checked == true)
            {
                CurrentAlert.EventType = "All";
            }
            else if (AddRadioButton.Checked == true)
            {
                CurrentAlert.EventType = "Add";
            }
            else if (ModifyRadioButton.Checked == true)
            {
                CurrentAlert.EventType = "Modify";
            }
            else if (DeleteRadioButton.Checked == true)
            {
                CurrentAlert.EventType = "Delete";
            }

            int weekDay = WeekDayComboBox.SelectedIndex;
            int time = int.Parse(HoursComboBox.SelectedItem.ToString().Substring(0, 2));
            DateTime alertTimeDate = GetNextDateForDay(DateTime.Now, DayOfWeek.Monday);
            int weekDayNumber = 6;
            if (weekDay > 0)
                weekDayNumber = weekDay - 1;
            alertTimeDate = new DateTime(alertTimeDate.Year, alertTimeDate.Month, alertTimeDate.Day + weekDayNumber, time, 0, 0);
            CurrentAlert.AlertTime = alertTimeDate.ToString("dd-MM-yyyy HH:mm");
            AlertManager.UpdateAlert(siteSetting, webUrl, CurrentAlert);
            this.DialogResult = DialogResult.OK;
        }

        public static DateTime GetNextDateForDay(DateTime startDate, DayOfWeek desiredDay)
        {
            return startDate.AddDays(DaysToAdd(startDate.DayOfWeek, desiredDay));
        }

        public static int DaysToAdd(DayOfWeek current, DayOfWeek desired)
        {
            int c = (int)current;
            int d = (int)desired;
            int n = (7 - c + d);
            return (n > 7) ? n % 7 : n;
        }

        private void SetTimeControlsEnability()
        {
            WeekDayComboBox.Enabled = false;
            HoursComboBox.Enabled = false;
            if (DailyRadioButton.Checked == true)
            {
                HoursComboBox.Enabled = true;
            }
            else if (WeeklyRadioButton.Checked == true)
            {
                WeekDayComboBox.Enabled = true;
                HoursComboBox.Enabled = true;
            }
        }

        private void ImmediateRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SetTimeControlsEnability();
        }

        private void DailyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SetTimeControlsEnability();
        }

        private void WeeklyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            SetTimeControlsEnability();
        }

        private void CancelSaveFilterButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AlertFilterEditForm alertFilterEditForm = new AlertFilterEditForm();
            alertFilterEditForm.Initialize(webUrl, listName, listID, null, CurrentAlert, Fields);
            alertFilterEditForm.ShowDialog();
            FillFilters();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (FiltersTreeView.SelectedNode == null)
            {
                MessageBox.Show("Please select a group/filter.");
                return;
            }
            EUCamlFilters orGroup = FiltersTreeView.SelectedNode.Tag as EUCamlFilters;
            if (orGroup == null)
            {
                orGroup = FiltersTreeView.SelectedNode.Parent.Tag as EUCamlFilters;
            }
            AlertFilterEditForm alertFilterEditForm = new AlertFilterEditForm();
            alertFilterEditForm.Initialize(webUrl, listName, listID, orGroup, CurrentAlert, Fields);
            alertFilterEditForm.ShowDialog();
            FillFilters();
        }

        public void FillFilters()
        {
            FiltersTreeView.Nodes.Clear();
            foreach (EUCamlFilters group in CurrentAlert.OrGroups)
            {
                TreeNode orGroupNode = FiltersTreeView.Nodes.Add("OR");
                orGroupNode.Tag = group;
                foreach (EUCamlFilter filter in group)
                {
                    string nodeText = String.Empty;
                    nodeText = filter.FieldName + " " + filter.FilterType.ToString() + " " + filter.FilterValue;
                    TreeNode filterNode = orGroupNode.Nodes.Add(nodeText);
                    filterNode.Tag = filter;
                }
            }
            FiltersTreeView.ExpandAll();
        }
    }
}
