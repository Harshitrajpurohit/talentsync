using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Application.DTOs.Employees;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Application.Interfaces.Services;
using TalentSync.Domain.Entities.HumanResources;

namespace TalentSync.Application.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<EmployeeResponseDto>> GetEmployeesAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            int totalRecords =
                await _employeeRepository.CountAsync(cancellationToken);

            List<Employee> employees =
                await _employeeRepository.GetPagedAsync(
                    paginationRequest,
                    cancellationToken);

            List<EmployeeResponseDto> response =
                _mapper.Map<List<EmployeeResponseDto>>(employees);

            return new PaginationResponse<EmployeeResponseDto>(
                paginationRequest.PageNumber,
                paginationRequest.PageSize,
                totalRecords,
                response);
        }
    }
}
