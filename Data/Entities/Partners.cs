using System.Collections.Generic;

namespace FinanceApp.Data.Entities
{
    public class Partners
    {
        public int Id { get; set; }
        public string Name { get; set; }              
        public string ContactEmail { get; set; } 
        public string PhoneNumber { get; set; }      
        public string Address { get; set; }

       // İş ortaklarına gelen iş konuları için fk
        public ICollection<Jobs> Jobs { get; set; }   
    }
}
