using CodeFirst.Prescriptions.ResponseModels;
using Prescription = CodeFirst.Prescriptions.RequestModels.Prescription;

namespace CodeFirst.Prescriptions;

public interface IPrescriptionService
{
    Task AddPrescription(Prescription request);
    
    Task<PatientPrescriptions> GetPatientPrescriptions(int id);
}