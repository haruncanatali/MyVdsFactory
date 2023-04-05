namespace MyVdsFactory.Domain.Entities;

public class Prayer : BaseEntity
{
    public DateTime Date { get; set; }
    public string Fajr { get; set; } // imsak
    public string Tulu { get; set; } // Gunes
    public string Zuhr { get; set; } // Ogle
    public string Asr { get; set; } // Ikindi
    public string Maghrib { get; set; } // Aksam
    public string Isha { get; set; } // yatsi
}