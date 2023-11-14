using JWT.Demo.Context;
using JWT.Demo.DTOs.CommandQueryDTOs.EmployeeDTOs;
using JWT.Demo.DTOs.ResponseDTOs;
using JWT.Demo.Helpers.Paginations;
using JWT.Demo.Models.Entities;
using Microsoft.EntityFrameworkCore;
using static JWT.Demo.Helpers.Enums.Enums;

namespace JWT.Demo.Services.EmployeeServices
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseDTO> GetEmployeesAsync(PaginatedInputModel pagingParams)
        {
            try
            {
                var query = _context.Employees
                    .Select(e => new EmployeeDTO
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Age = e.Age,
                        Position = e.Position,
                        PhoneNumber = e.PhoneNumber
                    });

                var paginatedResult = await PaginationUtility.Paging(pagingParams, query.ToList());

                return new ResponseDTO
                {
                    Result = paginatedResult,
                    PageIndex = paginatedResult.PageIndex,
                    TotalPages = paginatedResult.TotalPages,
                    TotalItems = paginatedResult.TotalItems,
                    PageSize = paginatedResult.PageSize,
                    Message = "Employees retrieved successfully.",
                    StatusEnum = StatusEnum.Success
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Result = null,
                    Message = $"Failed to retrieve employees. Exception: {ex.Message}",
                    StatusEnum = StatusEnum.Exception
                };
            }
        }

        public async Task<ResponseDTO> GetEmployeeByIdAsync(long id)
        {
            try
            {
                var employee = await _context.Employees
                    .Where(e => e.Id == id)
                    .Select(e => new EmployeeDTO
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Age = e.Age,
                        Position = e.Position,
                        PhoneNumber = e.PhoneNumber
                    })
                    .FirstOrDefaultAsync();

                if (employee != null)
                {
                    return new ResponseDTO
                    {
                        Result = employee,
                        Message = "Employee retrieved successfully.",
                        StatusEnum = StatusEnum.Success
                    };
                }
                else
                {
                    return new ResponseDTO
                    {
                        Result = null,
                        Message = "Failed to find the employee.",
                        StatusEnum = StatusEnum.FailedToFindTheObject
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Result = null,
                    Message = $"Failed to retrieve employee. Exception: {ex.Message}",
                    StatusEnum = StatusEnum.Exception
                };
            }
        }

        public async Task<ResponseDTO> CreateEmployeeAsync(BasicEmployeeDTO employeeDTO)
        {
            try
            {
                var employee = new Employee
                {
                    Name = employeeDTO.Name,
                    Age = employeeDTO.Age,
                    Position = employeeDTO.Position,
                    PhoneNumber = employeeDTO.PhoneNumber
                };

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                return new ResponseDTO
                {
                    Result = employee.Id,
                    Message = "Employee created successfully.",
                    StatusEnum = StatusEnum.SavedSuccessfully
                };
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Result = null,
                    Message = $"Failed to create employee. Exception: {ex.Message}",
                    StatusEnum = StatusEnum.FailedToSave
                };
            }
        }

        public async Task<ResponseDTO> UpdateEmployeeAsync(long id, BasicEmployeeDTO employeeDTO)
        {
            try
            {
                var existingEmployee = await _context.Employees.FindAsync(id);

                if (existingEmployee != null)
                {
                    existingEmployee.Name = employeeDTO.Name;
                    existingEmployee.Age = employeeDTO.Age;
                    existingEmployee.Position = employeeDTO.Position;
                    existingEmployee.PhoneNumber = employeeDTO.PhoneNumber;

                    await _context.SaveChangesAsync();

                    return new ResponseDTO
                    {
                        Result = id,
                        Message = "Employee updated successfully.",
                        StatusEnum = StatusEnum.SavedSuccessfully
                    };
                }
                else
                {
                    return new ResponseDTO
                    {
                        Result = null,
                        Message = "Failed to find the employee for update.",
                        StatusEnum = StatusEnum.FailedToFindTheObject
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Result = null,
                    Message = $"Failed to update employee. Exception: {ex.Message}",
                    StatusEnum = StatusEnum.FailedToSave
                };
            }
        }

        public async Task<ResponseDTO> DeleteEmployeeAsync(long id)
        {
            try
            {
                var existingEmployee = await _context.Employees.FindAsync(id);

                if (existingEmployee != null)
                {
                    _context.Employees.Remove(existingEmployee);
                    await _context.SaveChangesAsync();

                    return new ResponseDTO
                    {
                        Result = id,
                        Message = "Employee deleted successfully.",
                        StatusEnum = StatusEnum.SavedSuccessfully
                    };
                }
                else
                {
                    return new ResponseDTO
                    {
                        Result = null,
                        Message = "Failed to find the employee for delete.",
                        StatusEnum = StatusEnum.FailedToFindTheObject
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Result = null,
                    Message = $"Failed to delete employee. Exception: {ex.Message}",
                    StatusEnum = StatusEnum.FailedToSave
                };
            }
        }
    }
}
