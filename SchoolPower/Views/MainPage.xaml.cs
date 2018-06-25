﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using SchoolPower.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Core;
using System;
using System.Collections.ObjectModel;
using SchoolPower.Views.Dialogs;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;

namespace SchoolPower.Views {
    public sealed partial class MainPage : Page {
        private List<Subject> subjects;
        private Windows.UI.Xaml.GridLength zeroGridLength;
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public MainPage() {
            Initialize();
            SetUI();

            //NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        void Initialize() {

            subjects = StudentData.subjects;
            InitializeComponent();
            zeroGridLength = new Windows.UI.Xaml.GridLength(); zeroGridLength = EmptyColumn.Width;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) => {
                if (CurrentVisualState.Text == "Narrow" && GradeOverViewColumn.Width == zeroGridLength) {
                    Swap();
                }
            };

        }

        void SetUI() {
            Views.Shell.HamburgerMenu.IsFullScreen = !true;
            Views.Shell.HamburgerMenu.HamburgerButtonVisibility = !false ? Visibility.Visible : Visibility.Collapsed;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Windows.UI.Color.FromArgb(255, 0, 99, 177);
            titleBar.ForegroundColor = Windows.UI.Colors.White;
            titleBar.ButtonBackgroundColor = Windows.UI.Color.FromArgb(255, 0, 99, 177);
            titleBar.ButtonForegroundColor = Windows.UI.Colors.White;
            titleBar.ButtonPressedBackgroundColor = Windows.UI.Color.FromArgb(0, 25, 114, 184);
            titleBar.ButtonPressedForegroundColor = Windows.UI.Colors.White;
            titleBar.ButtonInactiveBackgroundColor = Windows.UI.Color.FromArgb(255, 0, 99, 177);
            titleBar.ButtonInactiveForegroundColor = Windows.UI.Colors.LightGray;
            titleBar.InactiveBackgroundColor = Windows.UI.Color.FromArgb(255, 0, 99, 177);
            titleBar.InactiveForegroundColor = Windows.UI.Colors.LightGray;
            titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(0, 25, 114, 184);
            titleBar.ButtonHoverForegroundColor = Windows.UI.Colors.White;
        }

        void Swap() {
            Windows.UI.Xaml.GridLength gridLength = new Windows.UI.Xaml.GridLength();
            gridLength = GradeDetailColumn.Width;
            GradeDetailColumn.Width = GradeOverViewColumn.Width;
            GradeOverViewColumn.Width = gridLength;
        }

        private async void GPA_But_Click(object sender, RoutedEventArgs e) {
            SchoolPower.Views.Dialogs.GPADialog dialog = new SchoolPower.Views.Dialogs.GPADialog();
            await dialog.ShowAsync();
        }

        private async void Refresh_But_Click(object sender, RoutedEventArgs e) {
            await StudentData.Refresh();
            Initialize();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            // remove img
            NoGradeIcnImg.SetValue(Grid.ColumnProperty, 2);
            // change listview selected itm data template
            // Assign DataTemplate 
            foreach (var item in e.AddedItems) { 
                ListViewItem lvi = (sender as ListView).ContainerFromItem(item) as ListViewItem;
                lvi.ContentTemplate = (DataTemplate)this.Resources["CoursesListDataTemplate_Detail"];
            }
            // Remove DataTemplate
            foreach (var item in e.RemovedItems) { 
                ListViewItem lvi = (sender as ListView).ContainerFromItem(item) as ListViewItem;
                lvi.ContentTemplate = (DataTemplate)this.Resources["CoursesListDataTemplate_Compact"];
            }
            // navigate when normal
            if (CurrentVisualState.Text == "Normal") {
                int index = ListV.SelectedIndex;
                GradeDetailFrame.Navigate(typeof(MainPageGradePage), index);
            }
        }

        private void GoToDetailBut_Loaded(object sender, RoutedEventArgs e) {
            Button _Button = sender as Button;
            switch (CurrentVisualState.Text) {
                case "Narrow":
                    _Button.Visibility = Visibility.Visible;
                    break;
                case "Normal":
                    _Button.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void GoToDetailBut_Click(object sender, RoutedEventArgs e) {
            if (CurrentVisualState.Text == "Narrow") {
                Swap();
                GradeDetailFrame.Navigate(typeof(MainPageGradePage), ListV.SelectedIndex);
            }
        }

        private async void GradeDetailGridView_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            GridView gridView = sender as GridView;
            GradeInfoDialog dialog = new GradeInfoDialog(subjects[ListV.SelectedIndex].Grades[gridView.SelectedIndex]);
            await dialog.ShowAsync();
        }

        private void EditAssignment(object sender, RoutedEventArgs e) {

        }
    }
}