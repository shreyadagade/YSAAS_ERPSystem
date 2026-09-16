namespace LeadManagement.Application.DTOs.Lead
{
    public class LeadExcelDto
    {
        public int RowNumber { get; set; }

        public string? CandidateName { get; set; }

        public string? EmailAddress { get; set; }

        public string? MobileNumber { get; set; }

        //public string? TrainingType { get; set; }

        //public string? Description { get; set; }

        //public DateTime? LeadDate { get; set; }

        public string? Status { get; set; }

        public string? SourceName { get; set; }
    }
}