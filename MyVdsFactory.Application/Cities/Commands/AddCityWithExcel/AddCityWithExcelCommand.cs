using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Cities.Commands.AddCityWithExcel;

public class AddCityWithExcelCommand : IRequest<Result<long>>
{
    public IFormFile DataExcelFile { get; set; }
    
    public class Handler : IRequestHandler<AddCityWithExcelCommand, Result<long>>
    {
        private readonly IApplicationContext _context;

        public Handler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<Result<long>> Handle(AddCityWithExcelCommand request, CancellationToken cancellationToken)
        {
            using (var workbook = new XLWorkbook(request.DataExcelFile.OpenReadStream()))
            {
                var cityWorksheet = workbook.Worksheets.First();
                var cityRows = cityWorksheet.Rows();
                List<City> cities = new List<City>();
                List<District> districts = new List<District>();
                int cityCounter = 1;

                foreach (var cityRow in cityRows)
                {
                    if (!cityRow.Cell(2).Value.ToString().IsNullOrEmpty())
                    {
                        var districtWorksheet = workbook.Worksheets.Skip(1).First();
                        var districtRows = districtWorksheet.Rows();
                        
                        foreach (var districtRow in districtRows)
                        {
                            if (!districtRow.Cell(3).Value.ToString().IsNullOrEmpty() && int.Parse(districtRow.Cell(3).Value.ToString()) == cityCounter)
                            {
                                districts.Add(new District
                                {
                                    Name = districtRow.Cell(2).Value.ToString()
                                });
                            }
                        }

                        var entityResult = await _context.Cities.AddAsync(new City
                        {
                            Name = cityRow.Cell(2).Value.ToString(),
                        }, cancellationToken);
                        await _context.SaveChangesAsync(cancellationToken);
                        
                        foreach (var district in districts)
                        {
                            district.CityId = entityResult.Entity.Id;
                        }

                        await _context.Districts.AddRangeAsync(districts, cancellationToken);

                        await _context.SaveChangesAsync(cancellationToken);

                        districts = new List<District>();
                        cityCounter++;
                    }
                }
            }
            
            return Result<long>.Success(1,"Veritabanına kayıt başarılı oldu.");
        }
    }
}