using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_2_Final
{
    /// <summary>
    /// 
    /// </summary>
    
    
    public class Contractor
    {
        /// <summary>
        /// public int ID { get; set; }
        /// </summary>
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool AssignedStatus { get; set; }
        public string JobAssigned { get; set; }
        public DateTime DateStarted { get; set; }
        public double Wage { get; set; }

        public Contractor()
        {

        }

        public Contractor(int id, string firstName, string lastName, bool assignedStatus, string jobAssigned, DateTime dateStarted, double wage)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            AssignedStatus = assignedStatus;
            JobAssigned = jobAssigned;
            DateStarted = dateStarted;
            Wage = wage;
        }

        
        public override string ToString()
        {
            return $"{FirstName} {LastName}({ID}) - Assigned To:[{JobAssigned}] - Rate: ${Wage}/hr";
        }
    }
    
}
