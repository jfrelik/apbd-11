namespace CodeFirst.Prescriptions.Models;

public class Patient
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public List<Prescription> Prescriptions { get; set; }
}
