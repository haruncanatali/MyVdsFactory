using AutoMapper;
using MyVdsFactory.Application.Common.Mappings;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Prayers.Queries.Dtos;

public class PrayerDto : IMapFrom<Prayer>
{
    public long Id { get; set; }
    public string Date { get; set; }
    public string Fajr { get; set; } // imsak
    public string Tulu { get; set; } // Gunes
    public string Zuhr { get; set; } // Ogle
    public string Asr { get; set; } // Ikindi
    public string Maghrib { get; set; } // Aksam
    public string Isha { get; set; } // yatsi
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Prayer, PrayerDto>()
            .ForMember(dest => dest.Date , opt => opt
                .MapFrom(c=>c.Date.ToString("dd.MM.yyyy")));
    }
}