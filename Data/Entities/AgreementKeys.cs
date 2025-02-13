using System;

namespace FinanceApp.Data.Entities
{
    public class AgreementKeys
    {
        public int Id { get; set; }
        public int AgreementId { get; set; } 
        public string Keyword { get; set; } 
        public DateTime LastModifiedDate { get; set; } 

        // anlaşma fk
        public Agreement Agreement { get; set; }
    }
}
