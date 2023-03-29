using AutoMapper;
using MyVdsFactory.Application.Common.Mappings;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Terrors.Queries.Dtos;

public class TerrorDto : IMapFrom<Terror>
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime Date { get; set; }
    public string Country { get; set; }
    public string Region { get; set; }
    public string City { get; set; }
    public string Location { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Summary { get; set; }
    public string Alternative { get; set; }
    public bool Success { get; set; }
    public bool Suicide { get; set; }
    public string AttackType { get; set; }
    public string TargetType { get; set; }
    public string TargetSubType { get; set; }
    public string GroupName { get; set; }
    public string GroupSubName { get; set; }
    public string WeaponType { get; set; }
    public string WeaponSubType { get; set; }
    public string WeaponDetail { get; set; }
    public int Kill { get; set; }
    public string DbSource { get; set; }
    public double CityLatitude { get; set; }
    public double CityLongitude { get; set; }
    public double CountryLatitude { get; set; }
    public double CountryLongitude { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Terror, TerrorDto>();
    }
}