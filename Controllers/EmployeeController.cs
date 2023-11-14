using JWT.Demo.DTOs.CommandQueryDTOs.EmployeeDTOs;
using JWT.Demo.DTOs.ResponseDTOs;
using JWT.Demo.Helpers.GenericSearchFilter;
using JWT.Demo.Helpers.GenericSort;
using JWT.Demo.Helpers.Paginations;
using JWT.Demo.Services.EmployeeServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT.Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet,Route("GetALLEmployees")]
        public async Task<ActionResult<ResponseDTO>> GetEmployees([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10,
                                                                  [FromQuery] IEnumerable<SortingParams>? sortingParams = null,
                                                                  [FromQuery] IEnumerable<FilterParams>? filterParam = null,
                                                                  [FromQuery] IEnumerable<string>? groupingColumns = null)
        {
            var pagingParams = new PaginatedInputModel
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortingParams = sortingParams,
                FilterParam = filterParam,
                GroupingColumns = groupingColumns
            };

            var response = await _employeeRepository.GetEmployeesAsync(pagingParams);
            return Ok(response);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet, Route("GetEmployeeById/{id}")]
        public async Task<ActionResult<ResponseDTO>> GetEmployeeById(long id)
        {
            var response = await _employeeRepository.GetEmployeeByIdAsync(id);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost,Route("CreateEmployee")]
        public async Task<ActionResult<ResponseDTO>> CreateEmployee([FromBody] BasicEmployeeDTO employeeDTO)
        {
            var response = await _employeeRepository.CreateEmployeeAsync(employeeDTO);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut, Route("UpdateEmployee/{id}")]
        public async Task<ActionResult<ResponseDTO>> UpdateEmployee(long id, [FromBody] BasicEmployeeDTO employeeDTO)
        {
            var response = await _employeeRepository.UpdateEmployeeAsync(id, employeeDTO);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete, Route("DeleteEmployee/{id}")]
        public async Task<ActionResult<ResponseDTO>> DeleteEmployee(long id)
        {
            var response = await _employeeRepository.DeleteEmployeeAsync(id);
            return Ok(response);
        }
    }
}