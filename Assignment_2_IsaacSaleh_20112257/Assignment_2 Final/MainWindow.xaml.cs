using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace Assignment_2_Final
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {    
        DataManager manager = new DataManager();
        public MainWindow()
        {
            

            InitializeComponent();

            RefreshAvailableJobContractors();

            RefreshJobs();
                                            
            
        }

        //-------------- Update Jobs and Contractors for current Usages in combo boxes --------------------
        private void RefreshAvailableJobContractors()
        {
            Combobox_assignContractor.Items.Clear();
            Combobox_assignContractor.Items.Add(Combobox_item_assignContractorDefault);
            Combobox_item_assignContractorDefault.IsSelected = true;
            foreach (Contractor contractor in manager.GetContractor())
            {
                Combobox_assignContractor.Items.Add($"{contractor.FirstName} {contractor.LastName}");
            }

            Combobox_selectContractor.Items.Clear();
            Combobox_selectContractor.Items.Add(Combobox_item_default);
            Combobox_item_default.IsSelected = true;
            foreach (Contractor contractor in manager.GetUnassignedContractors())
            {                                  
                Combobox_selectContractor.Items.Add($"{contractor.FirstName} {contractor.LastName}");            
            }
            
        }

        private void RefreshJobs()
        {
            Combobox_assignJob.Items.Clear();
            Combobox_assignJob.Items.Add(Combobox_item_assignJobDefault);
            Combobox_item_assignJobDefault.IsSelected = true;
            foreach(Job job in manager.GetJob())
            {
                Combobox_assignJob.Items.Add(job.JobTitle);
            }
        }





        // ----------------- Contractor Side of Window ----------------------------------

        private void Button_SearchContractors_Click(object sender, RoutedEventArgs e)
        {
            Listbox_Contractors.Items.Clear();


            if (Comboboxitem_contractorAll.IsSelected)
            {
                foreach (Contractor contractor in manager.GetContractor())
                {
                    Listbox_Contractors.Items.Add(contractor);
                }
            }
            else if (Comboboxitem_contractorAssigned.IsSelected)
            {
                foreach (Contractor availablecontractor in manager.GetAssignedContractors())
                {
                    Listbox_Contractors.Items.Add(availablecontractor);
                }
            }
            else if (Comboboxitem_contractorUnassigned.IsSelected)
            {
                foreach (Contractor busycontractor in manager.GetUnassignedContractors())
                {
                    Listbox_Contractors.Items.Add(busycontractor);
                }
            }            
        }


        private void Button_AddContractor_Click(object sender, RoutedEventArgs e)
        {
            if (Datepicker_StartDate.SelectedDate == null)
            {
                MessageBox.Show("Contractor must have a valid Start Date","Invalid Date",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }

            if (Textbox_LastName.Text == "")
            {
                MessageBox.Show("Last Name is a required Field", "Invalid Name", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Textbox_Wage.Text == "")
            {
                MessageBox.Show("Pay Field is empty!", "Invalid Name", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int id = manager.GetID();
            bool assignedToJob = false;
            string jobAssigned = "UNASSIGNED";


            Contractor contractor = new Contractor(
                id,
                Textbox_FirstName.Text,
                Textbox_LastName.Text,
                assignedToJob,
                jobAssigned,
                (DateTime)Datepicker_StartDate.SelectedDate,
                double.Parse(Textbox_Wage.Text));
            manager.AddContractor(contractor);

            Textbox_FirstName.Clear();
            Textbox_LastName.Clear();
            Textbox_Wage.Clear();
            Datepicker_StartDate.SelectedDate = null;

            MessageBox.Show("Contractor Succefully Added", "Success", MessageBoxButton.OK);

            RefreshAvailableJobContractors();
        }

        private void Button_RemoveContractor_Click(object sender, RoutedEventArgs e)
        {
            Contractor oldworker = (Contractor) Listbox_Contractors.SelectedItem;
            Listbox_Contractors.Items.Remove(oldworker);
            manager.RemoveContractor(oldworker);
            RefreshAvailableJobContractors();
        }





        // ------------ Assign/Remove buttons Mid-Window ------------------------

        private void Button_Assign_Click(object sender, RoutedEventArgs e)
        {

            string name = Combobox_assignContractor.SelectedItem.ToString();
            string jobTitle = Combobox_assignJob.SelectedItem.ToString();

            if (name == "Select Contractor" || jobTitle == "Select Job")
            {
                MessageBox.Show("Please Select a Contractor AND a Job", "Error", MessageBoxButton.OK);
            }
            else if (manager.ValidateContractorAssignment(name) == false || manager.ValidateJobAssignement(jobTitle) == false)
            {
                MessageBox.Show("Contractor or Job is already ASSIGNED", "Error", MessageBoxButton.OK);
            }
            else
            {
                manager.AssignJobWithButton(name, jobTitle);

                RefreshAvailableJobContractors();
                RefreshJobs();
                MessageBox.Show("Contractor has been Succefully Added to Job", "Success", MessageBoxButton.OK);
            }    
        }

        private void Button_Unassign_Click(object sender, RoutedEventArgs e)
        {
            string name = Combobox_assignContractor.SelectedItem.ToString();
            string jobTitle = Combobox_assignJob.SelectedItem.ToString();

            if (name == "Select Contractor" || jobTitle == "Select Job")
            {
                MessageBox.Show("Please Select a Contractor AND a Job", "Error", MessageBoxButton.OK);
            }
            else if (manager.ValidateContractorAssignment(name) == true || manager.ValidateJobAssignement(jobTitle) == true)
            {
                MessageBox.Show("Contractor or Job is already UNASSIGNED", "Error", MessageBoxButton.OK);
            }
            else
            {
                manager.RemoveJobWithButton(name, jobTitle);

                RefreshAvailableJobContractors();
                RefreshJobs();
                MessageBox.Show("Contractor has been Succefully Removed From Job", "Success", MessageBoxButton.OK);
            }           
        }





        // ---------------------- Jobs Side of Window ---------------------

        private void Button_SearchJobs_Click(object sender, RoutedEventArgs e)
        {
            Listbox_jobs.Items.Clear();

            if (Combobox_jobsAll.IsSelected && Textbox_JobTitle.Text=="")
            {
                foreach (Job job in manager.GetJob())
                {
                    Listbox_jobs.Items.Add(job);
                }
                Textbox_minCost.Clear();
                Textbox_maxCost.Clear();
            }
            else if (Combobox_jobsAssigned.IsSelected)
            {
                foreach (Job assignedJobs in manager.GetAssignedJob())
                {
                    Listbox_jobs.Items.Add(assignedJobs);
                }
                Textbox_minCost.Clear();
                Textbox_maxCost.Clear();
            }
            else if (Combobox_jobsUnassigned.IsSelected)
            {
                foreach (Job unassignedJobs in manager.GetUnassignedJob())
                {
                    Listbox_jobs.Items.Add(unassignedJobs);
                }
                Textbox_minCost.Clear();
                Textbox_maxCost.Clear();
            }
            else if (Combobox_jobsByCost.IsSelected)
            {
                // Deals with one or more box's left empty
                if (Textbox_minCost.Text == "" && Textbox_maxCost.Text == "")
                {
                    foreach (Job job in manager.GetJob())
                    {
                        Listbox_jobs.Items.Add(job);
                    }
                }
                else if (Textbox_minCost.Text == "")
                {
                    Textbox_minCost.Text = "0";
                    int minCost = int.Parse(Textbox_minCost.Text.Replace(",", ""));
                    int maxCost = int.Parse(Textbox_maxCost.Text.Replace(",", ""));

                    foreach (Job jobByCostRange in manager.GetJobByCostRange(minCost, maxCost))
                    {
                        Listbox_jobs.Items.Add(jobByCostRange);
                    }

                }
                else if (Textbox_maxCost.Text == "")
                {
                    int minCost = int.Parse(Textbox_minCost.Text.Replace(",", ""));
                    foreach (Job job in manager.GetJob())
                    {
                        if (job.Cost >= minCost)
                        {
                            Listbox_jobs.Items.Add(job);
                        }

                    }
                }  
                else
                {
                    int minCost = int.Parse(Textbox_minCost.Text.Replace(",", ""));
                    int maxCost = int.Parse(Textbox_maxCost.Text.Replace(",", ""));

                    foreach (Job jobByCostRange in manager.GetJobByCostRange(minCost, maxCost))
                    {
                        Listbox_jobs.Items.Add(jobByCostRange);
                    }
                }
            }
            else if (Combobox_CompletedJobs.IsSelected)
            {
                foreach (Job completedJob in manager.GetCompletedJob())
                {
                    Listbox_jobs.Items.Add(completedJob);
                }
            }            
        }

        private void Button_AddJob_Click(object sender, RoutedEventArgs e)
        {
            if (Textbox_JobTitle.Text == "")
            {
                MessageBox.Show("Contract Name is a required Field", "Invalid Job Title", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (DatePicker_JobStartDate.SelectedDate == null)
            {
                MessageBox.Show("Contract must have a valid Start Date", "Invalid Date", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (Textbox_JobCost.Text == "")
            {
                MessageBox.Show("Job cannot be added without an Estimated Cost", "Error", MessageBoxButton.OK);
            }
            string assignedContractor;
            bool assignmentStatus;
            if (Combobox_selectContractor.SelectedItem == Combobox_item_default)
            {
               // MessageBox.Show("You must have an assigned Contractor for Job", "Error", MessageBoxButton.OK);
                // - To fix up later so a contractor doesn't have to be assigned at job at point of job creation.
                assignmentStatus = false;
                assignedContractor = "UNASSIGNED";
            }
            else
            {
                assignmentStatus = true;
                assignedContractor = Combobox_selectContractor.SelectionBoxItem.ToString();
                
            }

            int jobNum = manager.GetJobNumber();
            bool completionStatus = false;
            Job job = new Job(
                jobNum,
                Textbox_JobTitle.Text,
                double.Parse(Textbox_JobCost.Text),
                assignmentStatus,
                completionStatus,
                assignedContractor,
                (DateTime)DatePicker_JobStartDate.SelectedDate);
            manager.AddJob(job);
            manager.AssignJobToContractor(assignedContractor, job);
            manager.AssignContractorToJob(assignedContractor, job.JobTitle);

            Textbox_JobTitle.Clear();
            Textbox_JobCost.Clear();
            DatePicker_JobStartDate.SelectedDate = null;

            RefreshJobs();
            RefreshAvailableJobContractors();
            MessageBox.Show("Contract has been Succefully Added", "Success", MessageBoxButton.OK);
        }

        // Uses list box selection to remove job
        private void Button_RemoveJob_Click(object sender, RoutedEventArgs e)
        {
            Job oldJob = (Job)Listbox_jobs.SelectedItem;
            Listbox_jobs.Items.Remove(oldJob);
            manager.RemoveJob(oldJob);
            manager.RemoveJobFromContractor(oldJob);
            RefreshAvailableJobContractors();
            RefreshJobs();
        }

        // Uses list box selection to complete job
        private void Button_CompleteJob_Click(object sender, RoutedEventArgs e)
        {
            Job completeJob = (Job)Listbox_jobs.SelectedItem;
            Listbox_jobs.Items.Remove(completeJob);
            manager.CompleteJobs(completeJob);
            RefreshJobs();
            RefreshAvailableJobContractors();
        }
    }
}
