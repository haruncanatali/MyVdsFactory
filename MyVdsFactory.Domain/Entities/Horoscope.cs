namespace MyVdsFactory.Domain.Entities;

public class Horoscope : BaseEntity
{
    public Horoscope()
    {
        HoroscopeCommentaries = new List<HoroscopeCommentary>();
    }
    
    public string Name { get; set; }
    public string PhotoName { get; set; }
    public string DateRange { get; set; }
    public string Planet { get; set; }
    public string Description { get; set; }
    public string Group { get; set; }
    public string NormalizedName { get; set; }

    public List<HoroscopeCommentary> HoroscopeCommentaries { get; set; }
}