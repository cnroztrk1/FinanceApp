namespace FinanceApp.API.Models
{
    public class JobRequestModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int BusinessPartnerId { get; set; }
        public int AgreementId { get; set; }
    }
}
