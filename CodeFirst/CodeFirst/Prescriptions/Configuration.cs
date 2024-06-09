using System.Data;
using CodeFirst.Prescriptions.RequestModels;
using Microsoft.AspNetCore.Authorization;

namespace CodeFirst.Prescriptions;

public static class Configuration
{
    public static void RegisterEndpointsForPrescriptions(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/prescriptions", [Authorize] async (IPrescriptionService service, Prescription request) =>
        {
            await service.AddPrescription(request);
            return Results.Created();
        });

        app.MapGet("api/prescriptions/patient/{id:int}", [Authorize] async (IPrescriptionService service, int id) =>
        {
            var result = await service.GetPatientPrescriptions(id);
            return Results.Ok(result);
        });
    }
}