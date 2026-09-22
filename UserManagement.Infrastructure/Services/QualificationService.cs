//using UserManagement.Application.Contracts;
//using UserManagement.Application.DTOs.Qualification;
//using UserManagement.Application.Interfaces;

//namespace UserManagement.Infrastructure.Services
//{
//    public class QualificationService : IQualificationService
//    {
//        private readonly IGenericRepository _repository;

//        private const string StoredProcedure =
//            "erpsystem.sp_tblqualifications";

//        public QualificationService(IGenericRepository repository)
//        {
//            _repository = repository;
//        }

//        public async Task<List<QualificationResponseDto>> GetAllAsync()
//        {
//            var result =
//                await _repository.ExecuteQueryAsync<QualificationResponseDto>(
//                    StoredProcedure,
//                    new StoredProcedureParameter
//                    {
//                        Name = "@Type",
//                        Value = "GetAll"
//                    });

//            return result.ToList();
//        }
//    }
//}