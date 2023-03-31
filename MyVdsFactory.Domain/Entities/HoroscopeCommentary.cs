namespace MyVdsFactory.Domain.Entities;

public class HoroscopeCommentary : BaseEntity
{
    public string Commentary { get; set; }
    public DateTime Date { get; set; }

    public long HoroscopeId { get; set; }

    public Horoscope Horoscope { get; set; }
}