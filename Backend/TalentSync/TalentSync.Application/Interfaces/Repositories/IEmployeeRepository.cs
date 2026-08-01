using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Common.Pagination;
using TalentSync.Domain.Entities.HumanResources;

namespace TalentSync.Application.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee employee, CancellationToken cancellationToken);
        Task<Employee?> GetById(Guid id, CancellationToken cancellationToken);
        Task<Employee?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);

        Task<int> CountAsync(CancellationToken cancellationToken);

        Task<List<Employee>> GetPagedAsync( PaginationRequest paginationRequest, CancellationToken cancellationToken);

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
