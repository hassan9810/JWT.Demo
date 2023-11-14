using JWT.Demo.DTOs.CommandQueryDTOs.EmployeeDTOs;
using JWT.Demo.DTOs.ResponseDTOs;
using JWT.Demo.Helpers.Paginations;

namespace JWT.Demo.Services.EmployeeServices
{
    public interface IEmployeeRepository
    {
        Task<ResponseDTO> GetEmployeesAsync(PaginatedInputModel pagingParams);
        Task<ResponseDTO> GetEmployeeByIdAsync(long id);
        Task<ResponseDTO> CreateEmployeeAsync(BasicEmployeeDTO employeeDTO);
        Task<ResponseDTO> UpdateEmployeeAsync(long id, BasicEmployeeDTO employeeDTO);
        Task<ResponseDTO> DeleteEmployeeAsync(long id);
    }
}
