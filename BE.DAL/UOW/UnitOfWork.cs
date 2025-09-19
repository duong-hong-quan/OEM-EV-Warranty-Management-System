using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.DAL.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _context;

        public UnitOfWork(IDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task ExecuteInTransaction(Func<Task> operation)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            // Execute the operation with retry logic in case of transient errors
            await strategy.ExecuteAsync(async () =>
            {
                // Begin a transaction on the DbContext
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    // Execute the passed-in operation (database operations)
                    await operation();

                    // Commit the transaction if everything is successful
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    // If an error occurs, log it or handle it, and then roll back the transaction
                    Console.WriteLine($"Transaction failed: {ex.Message}");
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }
    }
}
