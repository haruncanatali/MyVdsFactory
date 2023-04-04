using System.Globalization;
using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Earthquakes.Commands.AddEarthquakeWithExcel
{
    public class AddEarthquakeWithExcelCommand : IRequest<Result<long>>
    {
        public IFormFile ExcelFile { get; set; }
        public class Handler : IRequestHandler<AddEarthquakeWithExcelCommand, Result<long>>
        {
            private readonly IApplicationContext _context;

            public Handler(IApplicationContext context)
            {
                _context = context;
            }

            public async Task<Result<long>> Handle(AddEarthquakeWithExcelCommand request, CancellationToken cancellationToken)
            {
                using (var workbook = new XLWorkbook(request.ExcelFile.OpenReadStream()))
                {
                    var worksheet = workbook.Worksheets.First();
                    var rows = worksheet.Rows();
                    int rowItemIndex = 0;
                    List<Earthquake> earthquakes = new List<Earthquake>();

                    foreach (var row in rows)
                    {
                        if (rowItemIndex > 0)
                        {
                            var date = DateTime.Parse(row.Cell(9)?.Value.ToString() ??
                                                      DateTime.Now.ToString(CultureInfo.InvariantCulture));
                            earthquakes.Add(new Earthquake
                            {
                                Rms = Convert.ToDouble(row.Cell(1)?.Value.ToString() ?? "0"),
                                Latitude = Convert.ToDouble(row.Cell(3)?.Value.ToString() ?? "0"),
                                Longitude = Convert.ToDouble(row.Cell(4)?.Value.ToString() ?? "0"),
                                Magnitude = Convert.ToDouble(row.Cell(5)?.Value.ToString() ?? "0"),
                                Location = row.Cell(2)?.Value.ToString() ?? "Uluslararası Sular",
                                Country = row.Cell(6)?.Value.ToString() ?? "Uluslararası Sular",
                                Province = row.Cell(7)?.Value.ToString() ?? "Uluslararası Sular",
                                District = row.Cell(8)?.Value.ToString() ?? "Uluslararası Sular",
                                Date = date,
                                Year = date.Date.Year,
                                Month = date.Date.Month,
                                Day = date.Date.Day
                            });
                        }
                        rowItemIndex++;
                    }

                    await _context.Earthquakes.AddRangeAsync(earthquakes, cancellationToken);

                    await _context.SaveChangesAsync(cancellationToken);
                }

                return Result<long>.Success(1,"Deprem verileri veritabanına başarıyla kaydedildi.");
            }
        }
    }
}
