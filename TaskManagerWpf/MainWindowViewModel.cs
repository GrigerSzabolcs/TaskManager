using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TaskManagerWpf.Logic;
using TaskManagerWpf.Models;
using TaskManagerWpf.Services;

namespace TaskManagerWpf
{
    internal class MainWindowViewModel : ObservableRecipient, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public Interval Period { get; set; }
        UserInfo UserInfo { get; set; }
        ApiService apiService;

        private string minute;
        public string Minute
        {
            get { return minute; }
            set
            {
                if (minute != value)
                {
                    minute = value;
                    OnPropertyChanged(nameof(Minute));
                }
            }
        }
        private string hour;
        public string Hour
        {
            get { return hour; }
            set
            {
                if (hour != value)
                {
                    hour = value;
                    OnPropertyChanged(nameof(Hour));
                }
            }
        }
        private Occupation occupation;
        public Occupation Occupation
        {
            get { return occupation; }
            set { SetProperty(ref occupation, value); }
        }
        private Activity selectedActivity;
        public Activity SelectedActivity
        {
            get { return selectedActivity; }
            set { SetProperty(ref selectedActivity, value); }
        }

        public ObservableCollection<Occupation> Occupations { get; set; }
        public ObservableCollection<Activity> Activities { get; set; }
        public ObservableCollection<string> MinutesOptions { get; set; }
        public ObservableCollection<string> HoursOptions { get; set; }
        public ICommand CreateActivityCommand { get; set; }
        public ICommand CreateOccupationCommand { get; set; }
        public ICommand CreateMyReportCommand { get; set; }
        public ICommand CreateBigReportCommand { get; set; }

        public MainWindowViewModel(TokenModel jwsToken)
        {
            Period = new Interval() { From = DateTime.Now.AddDays(-1), To = DateTime.Now.AddDays(1) };
            HoursOptions = new ObservableCollection<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8" };
            MinutesOptions = new ObservableCollection<string> { "0", "30" };
            occupation = new Occupation();
            Hour = "0";
            Minute = "0";

            apiService = new ApiService(jwsToken);

            Task.Run(async () =>
            {
                await LoadActivities();
                await LoadOccupations();
                AssignActivities();
                await LoadUserInfo();
            }).Wait();
            

            CreateActivityCommand = new RelayCommand(async () =>
            {
                await AddActivity();
            });
            CreateOccupationCommand = new RelayCommand(async () =>
            {
                await AddOccupation();
            });
            CreateMyReportCommand = new RelayCommand(async () =>
            {
                await MakeMyReport();
            });
            CreateBigReportCommand = new RelayCommand(async () =>
            {
                await MakeBigReport();
            });
        }

        //Makes a txt report containing how much time the employee spent with a task within a given perid.
        async Task MakeMyReport()
        {
            try
            {
                var report = await apiService.GetMyReport(Period);
                SaveReportToFile(report);
                MessageBox.Show("Report done!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating report: " + ex.Message);
            }
        }
        //Makes a txt report containing how much time the company spent with a task within a given perid.
        async Task MakeBigReport()
        {
            try
            {
                var report = await apiService.MakeBigReport(Period);
                SaveBigReportToFile(report);
                MessageBox.Show("Report done!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating report: " + ex.Message);
            }
        }     
        //Creates a new occupation and sends it to the server through apiService
        async Task AddOccupation()
        {
            if (SelectedActivity == null)
            {
                MessageBox.Show("Please select an activity!");
                return;
            }

            Occupation.ActivityId = SelectedActivity.Uid;
            Occupation.Date = DateTime.Now;

            if (!IsValidNumberBetween0And60(Hour) || !IsValidNumberBetween0And60(Minute))
            {
                MessageBox.Show("Please enter a number between 0 and 60 for the hour and minute fields!");
                return;
            }

            Occupation.Hour = int.Parse(Hour);
            Occupation.Minute = int.Parse(Minute);

            try
            {
                var wasAddedSuccessfully = await apiService.AddOccupation(Occupation, UserInfo);

                if (wasAddedSuccessfully)
                {
                    MessageBox.Show("Occupation successfully added.");
                    await Refresh();
                }
                else
                {
                    MessageBox.Show("Failed to add occupation. Please try again.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
        //Creates a new occupation and sends it to the server  through apiService
        async Task AddActivity()
        {
            ActivityCreatorWindow win = new ActivityCreatorWindow();

            if (win.ShowDialog() == true)
            {
                var wasAddedSuccessfully = await apiService.AddActivity(win.Activity);

                if (wasAddedSuccessfully)
                {
                    MessageBox.Show("Activity successfully added.");
                    await Refresh();
                }
                else
                {
                    MessageBox.Show("Failed to add activity. Please try again.");
                }
            }
            else
            {
                MessageBox.Show("Cant leave the title field empty");
            }
        }
        async Task Refresh()
        {
            try
            {
                var (newActivities, newOccupations) = await apiService.FetchDataAsync();

                Activities = new ObservableCollection<Activity>(newActivities);
                OnPropertyChanged(nameof(Activities));

                Occupations = new ObservableCollection<Occupation>(newOccupations);
                AssignActivities();
                OnPropertyChanged(nameof(Occupations));           
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during data refresh: " + ex.Message);
            }
        }
        async Task LoadUserInfo()
        {
            try
            {
                UserInfo = await apiService.GetUserInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading user info: " + ex.Message);
            }
        }

        async Task LoadActivities()
        {
            try
            {
                var activities = await apiService.GetActivities();
                Activities = new ObservableCollection<Activity>(activities);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading activities: " + ex.Message);
            }
        }
        async Task LoadOccupations()
        {
            try
            {
                var occupations = await apiService.GetOccupations();
                Occupations = new ObservableCollection<Occupation>(occupations);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading activities: " + ex.Message);
            }
        }




        void SaveBigReportToFile(IEnumerable<KeyValuePair<string, int>> report)
        {
            using (StreamWriter sw = new StreamWriter("BigReport.txt"))
            {
                sw.WriteLine("How much time the company spent with each task ({0}-{1}):", Period.From, Period.To);
                foreach (var item in report)
                {
                    sw.WriteLine("{0} - {1}Hour {2}Minute", item.Key, (item.Value / 60), (item.Value % 60));
                }
            }
        }
        void SaveReportToFile(IEnumerable<KeyValuePair<string, int>> report)
        {
            using (StreamWriter sw = new StreamWriter(UserInfo.FirstName + "_" + UserInfo.LastName + ".txt"))
            {
                sw.WriteLine("{0} spent this much time with each task ({1}-{2}):", UserInfo.UserName, Period.From, Period.To);
                foreach (var item in report)
                {
                    sw.WriteLine("{0} - {1}Hour {2}Minute", item.Key, (item.Value / 60), (item.Value % 60));
                }
            }
        }    
        //For each occupation, assigns the title of the activity based on the ActivityId
        void AssignActivities()
        {
            for (int i = 0; i < Occupations.Count; i++)
            {
                Occupations[i].Activity = Activities.Where(x => x.Uid == Occupations[i].ActivityId).First().Title;
            }
        }
        bool IsValidNumberBetween0And60(string str)
        {
            bool result = int.TryParse(str, out int number);
            return result && number >= 0 && number <= 60;
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
