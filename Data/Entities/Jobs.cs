using System;

namespace FinanceApp.Data.Entities
{
    public class Jobs
    {
        public int Id { get; set; }
        public string Title { get; set; }         
        public string Description { get; set; }       
        public DateTime ReceivedDate { get; set; }     
        public string Status { get; set; }             

       //------
        public int BusinessPartnerId { get; set; }      
        public Partners BusinessPartner { get; set; }
        public int AgreementId { get; set; }            // Hangi anlaşmaya bağlı olduğunun FK'si
        public Agreement Agreement { get; set; }

        //İşin riski için fk
        public RiskAnalysis RiskAnalysis { get; set; }
    }
}
