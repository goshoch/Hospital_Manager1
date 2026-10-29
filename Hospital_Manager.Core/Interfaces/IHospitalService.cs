using Data_Hospital_Manager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Manager.Core.Interfaces
{
    public interface IHospitalService
    {
        Task<List<Hospital>> GetAllHospitalsAsync();
        Task<Hospital> GetHospitalByIdAsync(int id);
        Task<Hospital> CreateHospitalAsync(Hospital hospital);
        Task<Hospital> UpdateHospitalAsync(Hospital hospital);
        Task<bool> DeleteHospitalAsync(int id);

    }
}
