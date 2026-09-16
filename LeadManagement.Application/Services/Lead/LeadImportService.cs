using ClosedXML.Excel;
using LeadManagement.Application.DTOs.Lead;
using LeadManagement.Application.Interfaces.Repositories;
using LeadManagement.Application.Interfaces.Services;

namespace LeadManagement.Infrastructure.Services
{
    public class LeadImportService : ILeadImportService
    {
        private readonly ILeadImportRepository _repository;

        private static readonly string[] AllowedStatuses =
        {
            "Hot",
            "Cold",
            "Warm",
            "Completed",
            "Cancelled",
            "Rescheduled",
            "Pending",
            "Call Back",
            "Contacted",
            "Not Reachable",
            "Interested",
            "Not Interested",
            "Demo Scheduled",
            "Demo Completed",
            "Negotiation",
            "Payment Issue",
            "Converted",
            "Other"
        };

        public LeadImportService(
            ILeadImportRepository repository)
        {
            _repository = repository;
        }

        public async Task<LeadImportResultDto>
            ImportExcelAsync(
                Stream excelStream,
                string fileName)
        {
            var result =
                new LeadImportResultDto();

            if (string.IsNullOrWhiteSpace(fileName) ||
                !fileName.EndsWith(
                    ".xlsx",
                    StringComparison.OrdinalIgnoreCase))
            {
                result.Errors.Add(
                    "Only .xlsx Excel files are supported.");

                return result;
            }

            var leads =
                ReadExcel(excelStream);

            result.TotalRows =
                leads.Count;

            if (!leads.Any())
            {
                result.Errors.Add(
                    "Excel file does not contain any data.");

                return result;
            }

            ValidateRows(
                leads,
                result);

            if (result.Errors.Any())
                return result;

            var sourceMap =
                await _repository
                    .GetActiveSourcesAsync();

            ValidateSources(
                leads,
                sourceMap,
                result);

            if (result.Errors.Any())
                return result;

            var existingEmails =
                await _repository
                    .GetExistingEmailsAsync(
                        leads
                            .Where(x =>
                                !string.IsNullOrWhiteSpace(
                                    x.EmailAddress))
                            .Select(x =>
                                x.EmailAddress!));

            var existingMobiles =
                await _repository
                    .GetExistingMobilesAsync(
                        leads
                            .Where(x =>
                                !string.IsNullOrWhiteSpace(
                                    x.MobileNumber))
                            .Select(x =>
                                x.MobileNumber!));

            ValidateDuplicates(
                leads,
                existingEmails,
                existingMobiles,
                result);

            if (result.Errors.Any())
                return result;

            await _repository.BulkInsertAsync(
                leads,
                sourceMap);

            result.Success = true;

            result.ImportedRows =
                leads.Count;

            return result;
        }

        private static List<LeadExcelDto>
            ReadExcel(Stream stream)
        {
            var leads =
                new List<LeadExcelDto>();

            using var workbook =
                new XLWorkbook(stream);

            var worksheet =
                workbook.Worksheets.FirstOrDefault();

            if (worksheet == null)
                return leads;

            var headerMap =
                new Dictionary<string, int>(
                    StringComparer.OrdinalIgnoreCase);

            var firstRow =
                worksheet.FirstRowUsed();

            if (firstRow == null)
                return leads;

            foreach (var cell in firstRow.CellsUsed())
            {
                var header =
                    cell.GetString()
                        .Trim()
                        .ToLowerInvariant();

                if (!string.IsNullOrWhiteSpace(header))
                {
                    headerMap[header] =
                        cell.Address.ColumnNumber;
                }
            }

            var requiredHeaders =
                new[]
                {
                    "candidate_name",
                    "email_address",
                    "mobile_number",
                    "status",
                    "source_name"
                };

            foreach (var header in requiredHeaders)
            {
                if (!headerMap.ContainsKey(header))
                {
                    throw new InvalidOperationException(
                        $"Required Excel column '{header}' is missing.");
                }
            }

            var lastRow =
                worksheet.LastRowUsed()
                    ?.RowNumber() ?? 1;

            for (int rowNumber = 2;
                 rowNumber <= lastRow;
                 rowNumber++)
            {
                var row =
                    worksheet.Row(rowNumber);

                if (row.IsEmpty())
                    continue;

                var lead =
                    new LeadExcelDto
                    {
                        RowNumber = rowNumber,

                        CandidateName =
                            GetString(
                                row,
                                headerMap,
                                "candidate_name"),

                        EmailAddress =
                            GetString(
                                row,
                                headerMap,
                                "email_address"),

                        MobileNumber =
                            GetString(
                                row,
                                headerMap,
                                "mobile_number"),

                        Status =
                            GetString(
                                row,
                                headerMap,
                                "status"),

                        SourceName =
                            GetString(
                                row,
                                headerMap,
                                "source_name")
                    };

                leads.Add(lead);
            }

            return leads;
        }

        private static string? GetString(
            IXLRow row,
            Dictionary<string, int> headerMap,
            string header)
        {
            var value =
                row.Cell(headerMap[header])
                    .GetString()
                    .Trim();

            return string.IsNullOrWhiteSpace(value)
                ? null
                : value;
        }

        private static void ValidateRows(
            List<LeadExcelDto> leads,
            LeadImportResultDto result)
        {
            foreach (var lead in leads)
            {
                if (string.IsNullOrWhiteSpace(
                    lead.CandidateName))
                {
                    result.Errors.Add(
                        $"Row {lead.RowNumber}: Candidate name is required.");
                }

                if (string.IsNullOrWhiteSpace(
                    lead.EmailAddress))
                {
                    result.Errors.Add(
                        $"Row {lead.RowNumber}: Email is required.");
                }

                if (string.IsNullOrWhiteSpace(
                    lead.MobileNumber))
                {
                    result.Errors.Add(
                        $"Row {lead.RowNumber}: Mobile number is required.");
                }

                if (string.IsNullOrWhiteSpace(
                    lead.Status))
                {
                    result.Errors.Add(
                        $"Row {lead.RowNumber}: Status is required.");
                }

                if (string.IsNullOrWhiteSpace(
                    lead.SourceName))
                {
                    result.Errors.Add(
                        $"Row {lead.RowNumber}: Source name is required.");
                }

                if (!string.IsNullOrWhiteSpace(
                    lead.EmailAddress) &&
                    !IsValidEmail(
                        lead.EmailAddress))
                {
                    result.Errors.Add(
                        $"Row {lead.RowNumber}: Invalid email address.");
                }

                if (!string.IsNullOrWhiteSpace(
                    lead.MobileNumber) &&
                    (lead.MobileNumber.Length != 10 ||
                     !lead.MobileNumber.All(char.IsDigit)))
                {
                    result.Errors.Add(
                        $"Row {lead.RowNumber}: Mobile number must contain exactly 10 digits.");
                }

                if (!string.IsNullOrWhiteSpace(
                    lead.CandidateName) &&
                    lead.CandidateName.Length > 100)
                {
                    result.Errors.Add(
                        $"Row {lead.RowNumber}: Candidate name cannot exceed 100 characters.");
                }

                if (!string.IsNullOrWhiteSpace(
                    lead.EmailAddress) &&
                    lead.EmailAddress.Length > 200)
                {
                    result.Errors.Add(
                        $"Row {lead.RowNumber}: Email cannot exceed 200 characters.");
                }

                if (!string.IsNullOrWhiteSpace(
                    lead.MobileNumber) &&
                    lead.MobileNumber.Length > 20)
                {
                    result.Errors.Add(
                        $"Row {lead.RowNumber}: Mobile number cannot exceed 20 characters.");
                }

                if (!string.IsNullOrWhiteSpace(
                    lead.Status))
                {
                    var status =
                        lead.Status.Trim();

                    if (!AllowedStatuses.Any(
                        x => x.Equals(
                            status,
                            StringComparison.OrdinalIgnoreCase)))
                    {
                        result.Errors.Add(
                            $"Row {lead.RowNumber}: Invalid status '{lead.Status}'.");
                    }
                }
            }
        }

        private static void ValidateSources(
            List<LeadExcelDto> leads,
            Dictionary<string, int> sourceMap,
            LeadImportResultDto result)
        {
            foreach (var lead in leads)
            {
                if (string.IsNullOrWhiteSpace(
                    lead.SourceName))
                {
                    continue;
                }

                if (!sourceMap.ContainsKey(
                    lead.SourceName.Trim()))
                {
                    result.Errors.Add(
                        $"Row {lead.RowNumber}: Source '{lead.SourceName}' does not exist in tbllead_sources.");
                }
            }
        }

        private static void ValidateDuplicates(
            List<LeadExcelDto> leads,
            HashSet<string> existingEmails,
            HashSet<string> existingMobiles,
            LeadImportResultDto result)
        {
            var excelEmails =
                new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

            var excelMobiles =
                new HashSet<string>();

            foreach (var lead in leads)
            {
                if (!string.IsNullOrWhiteSpace(
                    lead.EmailAddress))
                {
                    var email =
                        lead.EmailAddress.Trim();

                    if (existingEmails.Contains(
                        email))
                    {
                        result.Errors.Add(
                            $"Row {lead.RowNumber}: Email '{email}' already exists in database.");
                    }
                    else if (!excelEmails.Add(
                        email))
                    {
                        result.Errors.Add(
                            $"Row {lead.RowNumber}: Duplicate email '{email}' found in Excel.");
                    }
                }

                if (!string.IsNullOrWhiteSpace(
                    lead.MobileNumber))
                {
                    var mobile =
                        lead.MobileNumber.Trim();

                    if (existingMobiles.Contains(
                        mobile))
                    {
                        result.Errors.Add(
                            $"Row {lead.RowNumber}: Mobile '{mobile}' already exists in database.");
                    }
                    else if (!excelMobiles.Add(
                        mobile))
                    {
                        result.Errors.Add(
                            $"Row {lead.RowNumber}: Duplicate mobile '{mobile}' found in Excel.");
                    }
                }
            }
        }

        private static bool IsValidEmail(
            string email)
        {
            try
            {
                var address =
                    new System.Net.Mail.MailAddress(
                        email);

                return address.Address.Equals(
                    email,
                    StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}