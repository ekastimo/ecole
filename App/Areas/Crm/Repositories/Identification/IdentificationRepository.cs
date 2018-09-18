using System;
using System.Threading.Tasks;
using App.Data;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace App.Areas.Crm.Repositories.Identification
{
    public class IdentificationRepository : GenericRepository<Models.Identification>, IIdentificationRepository
    {
        private readonly ApplicationDbContext _context;

        public IdentificationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IdentificationExistsAsync(string nationalIdNumber)
        {
            try
            {
                var data = await _context.Identifications.FirstOrDefaultAsync(it => it.Number.Equals(nationalIdNumber));
                return data != null;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public async Task<Models.Identification> GetByIndentificationNumberAsync(string indentificationNo)
        {
            try
            {
                var data = await _context.Identifications
                    .FirstOrDefaultAsync(it => it.Number.Equals(indentificationNo));
                return data;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}