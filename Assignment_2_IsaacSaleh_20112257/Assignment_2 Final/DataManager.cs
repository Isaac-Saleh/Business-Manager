using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Assignment_2_Final
{

    public class DataManager
    {
        List<Job> jobs = new List<Job>();

        List<Job> completedJobs = new List<Job>();

        List<Contractor> contractors = new List<Contractor>();
        public DataManager()
        {
            //Job List
            jobs.Add(new Job(64914, "Perkins Build", 45000, false, false, "UNASSIGNED", new DateTime(2022, 12, 06)));
            jobs.Add(new Job(78328, "Smith Family", 290000, true, false, "Egg, Gus", new DateTime(2023, 02, 20)));
            jobs.Add(new Job(26483, "Yanchep Rental", 481000, false, false, "UNASSIGNED", new DateTime(2022, 06, 23)));
            jobs.Add(new Job(87256, "Butterworth Renovation", 93472, false, false, "UNASSIGNED", new DateTime(2022, 08, 14)));
            jobs.Add(new Job(56482, "BackYard Shed", 26000, false, false, "UNASSIGNED", new DateTime(2022, 11, 01)));
            jobs.Add(new Job(95684, "Enterprise Refit", 80900, true, false, "William Boimler", new DateTime(2023, 12, 10)));
            jobs.Add(new Job(75684, "Cresent Lane DriveWay", 8000, true, false, "Smiggle De'Liggle", new DateTime(2023, 04, 06)));
            jobs.Add(new Job(23684, "Hamptons DiningRoom", 17000, false, false, "UNASSIGNED", new DateTime(2023, 03, 12)));
            jobs.Add(new Job(65684, "Jedi Temple Skylight", 12000, true, false, "Ahsoka Tano", new DateTime(2022, 11, 02)));
            jobs.Add(new Job(90684, "Mountain Homestead", 862500, true, false,"Neva Umind", new DateTime(2023, 05, 08)));
            jobs.Add(new Job(19684, "Pickering Pool", 14000, false, false, "UNASSIGNED", new DateTime(2023, 03, 06)));

            //Completed Job List
            completedJobs.Add(new Job(26445, "Smegsville rental", 481340, false, true, "Egg Gus", new DateTime(2022, 06, 23)));
            completedJobs.Add(new Job(87232, "TropicThunder Renovation", 92172, false, true, "Betty White", new DateTime(2022, 08, 14)));
            completedJobs.Add(new Job(56434, "Gattaca Shed", 26345, false, true, "Jean-Luc Picard", new DateTime(2022, 11, 01)));
            completedJobs.Add(new Job(15653, "Lower Deck", 89200, true, true, "William Boimler", new DateTime(2023, 12, 10)));
            completedJobs.Add(new Job(75674, "Thrift-store demolition", 8040, true, true, "Smiggle De'Liggle", new DateTime(2023, 04, 06)));
            completedJobs.Add(new Job(65674, "Bike-Shop Remodel", 16800, false, true, "Jax Anarchy", new DateTime(2023, 03, 12)));

            //Contractor List
            contractors.Add(new Contractor(3356, "Egg", "Gus", true, "Smith Family", new DateTime(2005, 04, 05), 43.92));
            contractors.Add(new Contractor(8615, "Betty", "White", false, "UNASSIGNED", new DateTime(1920, 01, 25), 76.83));
            contractors.Add(new Contractor(2549, "Ragnar", "Lothbrok", false, "UNASSIGNED", new DateTime(1985, 01, 25), 56.48));
            contractors.Add(new Contractor(3613, "William", "Boimler", true, "Enterprise Refit", new DateTime(1962, 01, 25), 84.23));
            contractors.Add(new Contractor(4332, "Jean-Luc", "Picard", false, "UNASSIGNED", new DateTime(1979, 01, 25), 95.67));
            contractors.Add(new Contractor(5845, "Jax", "Anarchy", false, "UNASSIGNED", new DateTime(2000, 01, 25), 37.49));
            contractors.Add(new Contractor(6985, "Smiggle", "De'liggle", true, "Cresent Lane DriveWay", new DateTime(1954, 01, 25), 62.89));
            contractors.Add(new Contractor(7126, "Vernon", "Smith", false, "UNASSIGNED", new DateTime(1999, 01, 25), 28.52));
            contractors.Add(new Contractor(8546, "Asohka", "Tano", true, "Jedi Temple Skylight", new DateTime(1983, 01, 25), 35.36));
            contractors.Add(new Contractor(9528, "Neva", "Umind", true, "Mountain Homestead", new DateTime(1976, 01, 25), 46.78));
            contractors.Add(new Contractor(1035, "Anada", "Kname", false, "UNASSIGNED", new DateTime(1995, 01, 25), 39.58));
        }


        
        //------------------- Validation Functions ----------------------------

        public bool ValidateContractorAssignment(string name)
        {
            string[] fullName = name.Split(" ");
            string firstName = fullName[0].ToString();
            string lastName = fullName[1].ToString();

            foreach (Contractor contractor in contractors)
            {
                if ((contractor.FirstName == firstName && contractor.LastName == lastName))
                {
                    if (contractor.JobAssigned != "UNASSIGNED")
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool ValidateJobAssignement(string jobTitle)
        {
            Job foundJob = jobs.Find(job => job.JobTitle == jobTitle);

            foreach (Job job in jobs)
            {
                if (foundJob.AssignedContractor != "UNASSIGNED")
                {
                    return false;
                }
            }
            return true;
        }



        // ------------------------ Assign/Remove Functionalities --------------------------

        public void AssignJobWithButton(string contractorName, string jobTitle)
        {
            Job foundJob = jobs.Find(job => job.JobTitle == jobTitle);
            AssignJobToContractor(contractorName, foundJob);
            AssignContractorToJob(contractorName, jobTitle);
        }
        
        public void RemoveJobWithButton(string contractorName, string jobTitle)
        {
            Job foundJob = jobs.Find(job => job.JobTitle == jobTitle);
            RemoveContractorFromJob(contractorName, foundJob);
            RemoveJobFromContractor(foundJob);
        }

        public void AssignContractorToJob(string contractorName, string jobTitle)
        {
            foreach (Job job in jobs)
            {
                if(job.JobTitle == jobTitle)
                {
                    job.AssignedContractor = contractorName;
                    job.AssignmentStatus = true;
                }
            }
        }

        public void AssignJobToContractor(string name, Job job)
        {
            if (name == "UNASSIGNED")
            {   
                foreach(Contractor contractor in contractors)
                {
                    job.AssignmentStatus = false;
                    job.AssignedContractor = "UNASSIGNED";
                }
                
            }
            else
            {
                string[] fullName = name.Split(" ");
                string firstName = fullName[0].ToString();
                string lastName = fullName[1].ToString();

                foreach (Contractor contractor in contractors)
                {
                    if (contractor.FirstName == firstName && contractor.LastName == lastName)
                    {
                        contractor.AssignedStatus = true;
                        contractor.JobAssigned = job.JobTitle;
                    }
                }
            }
            
        }

        public void RemoveJobFromContractor(Job job)
        {           
            foreach (Contractor contractor in contractors)
            {
                if (job.JobTitle == contractor.JobAssigned)
                {
                    contractor.AssignedStatus = false;
                    contractor.JobAssigned = "UNASSIGNED";
                }
            }
        }


        public void RemoveContractorFromJob(string name, Job assignedJob)
        {
            string[] fullName = name.Split(" ");
            string firstName = fullName[0].ToString();
            string lastName = fullName[1].ToString();

            foreach (Job job in jobs)
            {
                if (assignedJob.AssignedContractor == name) 
                {
                    job.AssignmentStatus = false;
                    job.AssignedContractor = "UNASSIGNED";
                }
            }


        }


        //----------------------- Contractor List Functionality ------------------------

        public List<Contractor> GetContractor()
        {
            return contractors;
        }

        public List<Contractor> GetAssignedContractors()
        {
            List<Contractor> assignedContractors = contractors.Where(Contractor => Contractor.AssignedStatus == true).ToList();
            return assignedContractors;
        }

        public List<Contractor> GetUnassignedContractors()
        {
            List<Contractor> unassignedContractors = contractors.Where(Contractor => Contractor.AssignedStatus == false).ToList();
            return unassignedContractors;
        }

        

        // -------------- Job List Functionality ---------------

        public List<Job> GetJob()
        {
            return jobs;
        }

        public List<Job> GetAssignedJob()
        {
            List<Job> assignedJobs = jobs.Where(Job => Job.AssignmentStatus == true).ToList();
            return assignedJobs;
        }

        public List<Job> GetUnassignedJob()
        {
            List<Job> unassignedJobs = jobs.Where(Job => Job.AssignmentStatus == false).ToList();
            return unassignedJobs;
        }

        public List<Job> GetCompletedJob()
        {
           return completedJobs;
        }

        public List<Job> GetJobByCostRange(int minCost, int maxCost)
        {
            List<Job> jobByCostRange = jobs.Where(Job => Job.Cost >= minCost && Job.Cost <= maxCost).ToList();
            jobByCostRange.Sort((job1, job2) => job1.Cost.CompareTo(job2.Cost));
            return jobByCostRange;

        }


        // ----------------------- ID Generation -----------------------
        public int GetID() //Generates the ID for Contractors
        {
            Random random = new Random();
            int IDcontractor = random.Next(1000, 9999);
            foreach (Contractor contractor in contractors)
            {
                if (contractor.ID == IDcontractor)
                    continue;
            }
            return IDcontractor;
        }

        public int GetJobNumber() //Generates the JobNumber for JOBS
        {
            Random random = new Random();
            int RefNumber = random.Next(10000, 99999);
            foreach (Job job in jobs)
            {
                if (job.JobNumber == RefNumber)
                    continue;
            }
            return RefNumber;
        }


        // ------------------------- ADD/REMOVE functionalities ----------------------------
        public void AddContractor(Contractor newPerson)
        {
            newPerson.ID = GetID();
            contractors.Add(newPerson);
            
        }

        public void AddJob(Job newJob)
        {
            newJob.JobNumber = GetJobNumber();
            jobs.Add(newJob);
        }

        public void RemoveContractor(Contractor oldWorker)
        {
            contractors.Remove(oldWorker);
        }

        public void RemoveJob(Job oldJob)
        {
            jobs.Remove(oldJob);
            completedJobs.Remove(oldJob);
        }

        public void CompleteJobs(Job completedJob)
        {
            RemoveJobFromContractor(completedJob);
            completedJobs.Add(completedJob);
            jobs.Remove(completedJob);                  
        }              
    }
}
