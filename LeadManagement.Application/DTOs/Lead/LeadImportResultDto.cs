namespace LeadManagement.Application.DTOs.Lead
{
    public class LeadImportResultDto
    {
        public bool Success { get; set; }

        public int TotalRows { get; set; }

        public int ImportedRows { get; set; }

        public List<string> Errors { get; set; } = new();
    }
}