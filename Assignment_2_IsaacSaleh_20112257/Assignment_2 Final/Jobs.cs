using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Controls;

namespace Assignment_2_Final
{
    public class Job
    {
        public int JobNumber { get; set; }
        public string JobTitle { get; set; }
        public double Cost { get; set; }
        public bool AssignmentStatus { get; set; }
        public bool CompletionStatus { get; set; }
          
        public string AssignedContractor { get; set; }
         public DateTime StartOfJob { get; set; }

    
        public Job()
        {
            
        }
     
        public Job(int jobNum, string jobTitle, double cost, bool assignmentStatus, bool completionStatus, string assignedContractor, DateTime startDateOfJob) 
        {
            JobNumber = jobNum;
            JobTitle = jobTitle;
            Cost = cost;
            AssignmentStatus = assignmentStatus;
            CompletionStatus = completionStatus;
            AssignedContractor = assignedContractor;
            StartOfJob = startDateOfJob;
        }

        public override string ToString()
        {
            return $"Client: {JobTitle} - COST: ${Cost} - Assigned to: {AssignedContractor} - Ref#: {JobNumber}";
        }
    }
}
