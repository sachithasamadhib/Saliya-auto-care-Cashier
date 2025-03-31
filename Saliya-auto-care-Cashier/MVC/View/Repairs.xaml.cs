﻿using Saliya_auto_care_Cashier.MVC.Controller;
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
using System.Xml.Linq;

namespace Saliya_auto_care_Cashier.MVC.View
{
    /// <summary>
    /// Interaction logic for Repairs.xaml
    /// </summary>
    public partial class Repairs : UserControl
    {

        private readonly RepairsController repairsController;
        private List<Button> selectedButtons = new List<Button>();
        public event EventHandler<List<(string Name, decimal Price)>> RepairSelected;


        public Repairs()
        {
            InitializeComponent();
            repairsController = new RepairsController();
            LoadNames();
        }

        private void LoadNames()
        {
            try
            {
                List<(string Name, decimal Price)> Repairs = repairsController.GetCategories();

                foreach (var Repair in Repairs)
                {
                    Button button = new Button
                    {
                        Content = Repair.Name,
                        Style = (Style)FindResource("Category"),
                        ToolTip = Repair.Price,
                        Tag = "Unselected"
                    };

                    button.Click += Button_Click;
                    buttonPanel.Children.Add(button);
                }


                // need to change
                // Check if there are any buttons or not 
                if (buttonPanel.Children.Count == 0)
                {
                    noButtons.Visibility = Visibility.Visible;
                    buttonPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    noButtons.Visibility = Visibility.Collapsed;
                    buttonPanel.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading category names: {ex.Message}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            if (clickedButton.Tag.ToString() == "Unselected")
            {
                clickedButton.Tag = "Selected";
                selectedButtons.Add(clickedButton);
            }
            string name = clickedButton.Content.ToString();
            decimal price = (decimal)clickedButton.ToolTip;

            List<(string Name, decimal Price)> currentSelection = new List<(string Name, decimal Price)> { (name, price) };
            RepairSelected?.Invoke(this, currentSelection); // Event is triggered with the selected category name
        }

        private void ClearSelections_Click(object sender, RoutedEventArgs e)
        {
            ClearSelection();
        }

        public void ClearSelection()
        {
            foreach (Button button in selectedButtons.ToList())
            {
                button.Tag = "Unselected";
            }

            selectedButtons.Clear();
            RepairSelected?.Invoke(this, new List<(string Name, decimal Price)>());
        }

        private void Custombtn_click(object sender, RoutedEventArgs e)
        {
            // Find the Dashboard  and show the dialog
            var dashboardWindow = Application.Current.Windows.OfType<Dashboard>().FirstOrDefault();
            if (dashboardWindow != null)
            {
                var dialogHost = dashboardWindow.FindName("CustomButtonDialogHost") as MaterialDesignThemes.Wpf.DialogHost; //the name of the dialog host in the dashboard
                if (dialogHost != null)
                {
                    dialogHost.IsOpen = true;  // Open the dialog
                }
            }
        }
    }
}
