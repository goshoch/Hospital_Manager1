using Data_Hospital_Manager;
using Data_Hospital_Manager.Entities;
using Hospital_Manager.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Manager.Core.Services
{
    public class HospitalService :IHospitalService
    {
        private readonly HospitalDbContext context;

        public HospitalService(HospitalDbContext context)
        {
            this.context = context;
        }

        public Task<Hospital> CreateHospitalAsync(Hospital hospital)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteHospitalAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Hospital>> GetAllHospitalsAsync()
        {
            return await context.Hospitals.Where(x=>x.IsDeleted==false).ToListAsync();
        }

        public Task<Hospital> GetHospitalByIdAsync(int id)
        {
            return context.Hospitals.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted==false);
        }

        public Task<Hospital> UpdateHospitalAsync(Hospital hospital)
        {
            throw new NotImplementedException();
        }
    }
}
