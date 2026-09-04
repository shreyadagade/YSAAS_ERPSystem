using LeadManagement.Application.DTOs.Dashboard;


namespace LeadManagement.Application.Interfaces.Repositories.LeadDashboard
{
    public interface ILeadDashboardRepository
    {
        Task<LeadDashboardDto> GetDashboardAsync();
    }
}