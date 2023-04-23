using System.Globalization;
using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyVdsFactory.Application.Common.Extensions;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Cities.Commands.AddCityLatLongPopWithExcel;

public class AddCityLatLongPopWithExcelCommand : IRequest<Result<long>>
{
    public IFormFile ExcelDataFile { get; set; }

    public class Handler : IRequestHandler<AddCityLatLongPopWithExcelCommand, Result<long>>
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<AddCityLatLongPopWithExcelCommand> _logger;

        public Handler(IApplicationContext context, ILogger<AddCityLatLongPopWithExcelCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<long>> Handle(AddCityLatLongPopWithExcelCommand request, CancellationToken cancellationToken)
        {
            using (var workbook = new XLWorkbook(request.ExcelDataFile.OpenReadStream()))
            {
                var cityWorksheet = workbook.Worksheets.First();
                var cityRows = cityWorksheet.Rows();
                
                List<CityComplexExcelData> citiesComplexData = new List<CityComplexExcelData>();
                int counter = 1;

                List<string> cities = await _context.Cities.Select(c => 
                    c.Name.ReplaceTurkishCharacters(true)).ToListAsync(cancellationToken);
                
                _logger.LogInformation("Şehir isimleri veritabanından çekildi.");

                foreach (var cityRow in cityRows)
                {
                    if (counter == 1)
                    {
                        counter++;
                        continue;
                    }
                    
                    if (!cityRow.Cell(1).Value.ToString().IsNullOrEmpty())
                    {

                        try
                        {
                            var city = new CityComplexExcelData
                            {
                                CityName = cityRow.Cell(1).Value.ToString().ReplaceTurkishCharacters(),
                                Latitude = decimal.Parse(cityRow.Cell(2).Value.ToString()),
                                Longitude = decimal.Parse(cityRow.Cell(3).Value.ToString()),
                                Population = long.Parse(cityRow.Cell(4).Value.ToString())
                            };

                            if (cities.Contains(city.CityName) && !citiesComplexData.Contains(city))
                            {
                                citiesComplexData.Add(city);
                            }
                        }
                        catch (Exception e)
                        {
                            continue;
                        }
                    }
                    
                    else
                    {
                        break;
                    }
                }
                
                _logger.LogInformation("Şehir öznitelik hesaplamaları yapıldı.");

                var _cities = await _context.Cities.ToListAsync(cancellationToken);
                
                foreach (var data in citiesComplexData)
                {
                    var city = _cities.FirstOrDefault(
                        c=>c.Name.ReplaceTurkishCharacters() == data.CityName);

                    if (city != null)
                    {
                        city.Latitude = data.Latitude;
                        city.Longitude = data.Longitude;
                        city.Population = data.Population;

                        _context.Cities.Update(city);
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                }
            }
            
            _logger.LogInformation("Şehir öznitelikleri veritabanında güncellendi.");
            
            return Result<long>.Success(1,"Şehir özel nitelikleri başarıyla eklendi.");
        }
    }
}

public class CityComplexExcelData
{
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }
    public long Population { get; set; }
    public string CityName { get; set; }
}