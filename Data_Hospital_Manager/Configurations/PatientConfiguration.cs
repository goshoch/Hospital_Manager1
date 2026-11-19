using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Hospital_Manager.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data_Hospital_Manager.Configurations
{
    internal class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.Property(h => h.FirstName)
                .IsRequired()
                .HasMaxLength(30);
            builder.Property(h => h.LastName)
                .IsRequired()
                .HasMaxLength(30);
            builder.Property(h => h.Email)
                .IsRequired();
            builder.Property(h => h.PhoneNumber)
                .IsRequired();
            builder.HasData(
                new Patient
                {
                    Id = 1,
                    FirstName = "Gosho",
                    LastName="Ch",
                    Email = "patient@example.com",
                    PhoneNumber = "123-456-7890",
                    
                },
                new Patient
                {
                    Id = 2,
                    FirstName = "Ivan",
                    LastName = "Petrov",
                    Email = "patient2@example.com",
                    PhoneNumber = "987-654-3210",
                });
        }
    }
}
