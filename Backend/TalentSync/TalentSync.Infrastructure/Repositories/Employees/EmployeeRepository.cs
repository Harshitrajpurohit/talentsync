using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TalentSync.Application.Interfaces.Repositories;
using TalentSync.Domain.Entities.HumanResources;
using TalentSync.Infrastructure.Persistence;

namespace TalentSync.Infrastructure.Repositories.Employees
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(Employee employee, CancellationToken cancellationToken)
        {
            await _context.AddAsync(employee, cancellationToken);
        }

        public async Task<Employee?> GetById(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Employees.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        }

        public async Task<Employee?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Employees.FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
