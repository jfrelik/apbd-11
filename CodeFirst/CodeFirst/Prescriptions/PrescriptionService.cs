using System.Data;
using CodeFirst.Context;
using CodeFirst.Prescriptions.Models;
using CodeFirst.Prescriptions.ResponseModels;
using Microsoft.EntityFrameworkCore;
using Prescription = CodeFirst.Prescriptions.RequestModels.Prescription;

namespace CodeFirst.Prescriptions;

public class PrescriptionService: IPrescriptionService
{
    private readonly AppDbContext _context;
    
    public PrescriptionService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task AddPrescription(Prescription request)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == request.patient.IdPatient);
        
        if (patient == null)
        {
            Console.WriteLine("Patient not found. Adding new patient.");
            patient = new Prescriptions.Models.Patient
            { 
                FirstName = request.patient.FirstName, 
                LastName = request.patient.LastName,
                BirthDate = request.patient.BirthDate
            };
            _context.Patients.Add(patient);
        }
        
        var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == request.DoctorId);
        if (doctor == null)
        {
            throw new DataException("Doctor not found.");
        }

        var requestedMedicamentsIds = request.medicaments.Select(m => m.idMedicament).ToList();
        var medicaments = await _context.Medicaments.Where(m => requestedMedicamentsIds.Contains(m.Id)).ToListAsync();
        
        if (medicaments.Count != request.medicaments.Count)
        {
            throw new DataException("Some medicaments are not valid.");
        }
        
        if (medicaments.Count() > 10)
        {
            throw new DataException("Too many medicaments.");
        }
        
        if(request.DueDate <= request.Date)
        {
            throw new DataException("Due date must be after the prescription date.");
        }
        
        var prescription = new Prescriptions.Models.Prescription
        {
            Date = request.Date,
            DueDate = request.DueDate,
            DoctorId = request.DoctorId,
            Patient = patient,
            PrescriptionMedicaments = request.medicaments.Select(m => new PrescriptionMedicament
            {
                MedicamentId = m.idMedicament,
                Dose = m.Dose,
                Details = m.Details
            }).ToList()
        };
        
        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();
    }
    
    public async Task<PatientPrescriptions> GetPatientPrescriptions(int id)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);
        
        if (patient == null)
        {
            throw new DataException("Patient not found.");
        }
        
        var prescriptions = await _context.Prescriptions
            .Where(p => p.PatientId == id)
            .Include(p => p.Doctor)
            .Include(p => p.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .ToListAsync();
        
        return new PatientPrescriptions
        {
            IdPatient = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Prescriptions = prescriptions.Select(p => new ResponseModels.Prescription
            {
                IdPrescription = p.Id,
                Date = p.Date,
                DueDate = p.DueDate,
                Doctor = new ResponseModels.Doctor
                {
                    IdDoctor = p.Doctor.Id,
                    FirstName = p.Doctor.FirstName,
                },
                Medicaments = p.PrescriptionMedicaments.Select(pm => new ResponseModels.Medicament()
                {
                    IdMedicament = pm.Medicament.Id,
                    Name = pm.Medicament.Name,
                    Dose = pm.Dose,
                    Details = pm.Details
                }).ToList()
            }).ToList()
        };
    }
}