using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Domain.Entities.HumanResources;

namespace TalentSync.Application.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee employee, CancellationToken cancellationToken);
        Task<Employee?> GetById(Guid id, CancellationToken cancellationToken);
        Task<Employee?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
