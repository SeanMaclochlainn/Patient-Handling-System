using PatientHandlingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PatientHandlingSystem.DAL
{
    interface IPatientRepository
    {
        Patient GetPatient(int id);
    }
}
