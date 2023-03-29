using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Terrors.Commands.AddTerrorWithExcelCommand;

public class AddTerrorWithExcelCommand : IRequest<Result<long>>
{
    public IFormFile ExcelDataFile { get; set; }
    public class Handler : IRequestHandler<AddTerrorWithExcelCommand, Result<long>>
    {
        private readonly IApplicationContext _context;

        public Handler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<Result<long>> Handle(AddTerrorWithExcelCommand request, CancellationToken cancellationToken)
        {
            using (var workbook = new XLWorkbook(request.ExcelDataFile.OpenReadStream()))
            {
                var terrorWorksheet = workbook.Worksheets.First();
                var terrorRows = terrorWorksheet.Rows();
                int rowCounter = 0;
                List<Terror> terrors = new List<Terror>();

                foreach (var terrorRow in terrorRows)
                {
                    if (rowCounter > 0)
                    {
                        terrors.Add(new Terror
                        {
                            Year = Convert.ToInt32(TextValueConverter(terrorRow.Cell(1)?.Value.ToString(),true,false,2000)),
                            Month = Convert.ToInt32(TextValueConverter(terrorRow.Cell(2)?.Value.ToString(),true,false,1)),
                            Day = Convert.ToInt32(TextValueConverter(terrorRow.Cell(3)?.Value.ToString(),true,false,1)),
                            Date = new DateTime(Convert.ToInt32(terrorRow.Cell(1)?.Value.ToString() ?? "2000"),
                                Convert.ToInt32(terrorRow.Cell(2)?.Value.ToString() ?? "1"),
                                Convert.ToInt32(terrorRow.Cell(3)?.Value.ToString() ?? "1")),
                            Country = terrorRow.Cell(4)?.Value.ToString() ?? String.Empty,
                            Region = terrorRow.Cell(5)?.Value.ToString() ?? String.Empty,
                            City = terrorRow.Cell(6)?.Value.ToString() ?? String.Empty,
                            Location = terrorRow.Cell(7)?.Value.ToString() ?? String.Empty,
                            Latitude = Convert.ToDouble(terrorRow.Cell(8)?.Value.ToString() ?? "0.0"),
                            Longitude = Convert.ToDouble(terrorRow.Cell(9)?.Value.ToString() ?? "0.0"),
                            Summary = terrorRow.Cell(10)?.Value.ToString() ?? String.Empty,
                            Alternative = terrorRow.Cell(11)?.Value.ToString() ?? String.Empty,
                            Success = terrorRow.Cell(12)?.Value.ToString() == "1",
                            Suicide = terrorRow.Cell(13)?.Value.ToString() == "1",
                            AttackType = terrorRow.Cell(14)?.Value.ToString() ?? String.Empty,
                            TargetType = terrorRow.Cell(15)?.Value.ToString() ?? string.Empty,
                            TargetSubType = terrorRow.Cell(16)?.Value.ToString() ?? String.Empty,
                            GroupName = terrorRow.Cell(17)?.Value.ToString() ?? String.Empty,
                            GroupSubName = terrorRow.Cell(18)?.Value.ToString() ?? String.Empty,
                            WeaponType = terrorRow.Cell(19)?.Value.ToString() ?? String.Empty,
                            WeaponSubType = terrorRow.Cell(20)?.Value.ToString() ?? String.Empty,
                            WeaponDetail = terrorRow.Cell(21)?.Value.ToString() ?? String.Empty,
                            Kill = Convert.ToInt32(terrorRow.Cell(22)?.Value.ToString() ?? "0"),
                            DbSource = terrorRow.Cell(23)?.Value.ToString() ?? String.Empty,
                            CityLatitude = Convert.ToDouble(terrorRow.Cell(24)?.Value.ToString() ?? "0.0"),
                            CityLongitude = Convert.ToDouble(terrorRow.Cell(25)?.Value.ToString() ?? "0.0"),
                            CountryLatitude = Convert.ToDouble(terrorRow.Cell(26)?.Value.ToString() ?? "0.0"),
                            CountryLongitude = Convert.ToDouble(terrorRow.Cell(27)?.Value.ToString() ?? "0.0")
                        });
                    }
                    else
                    {
                        rowCounter++;
                    }
                }

                await _context.Terrors.AddRangeAsync(terrors, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            
            return Result<long>.Success(1,"Terör dataları başarıyla eklendi.");
        }

        private string TextValueConverter(string? value, bool isNumeric = false, bool isDouble = false,object defaultValue = null)
        {
            if (isNumeric)
            {
                try
                {
                    if (isDouble)
                    {
                        if (value.IsNullOrEmpty())
                        {
                            return defaultValue.ToString();
                        }

                        return value;
                    }
                    if (value.IsNullOrEmpty())
                    {
                        return "0";
                    }
                }
                catch (Exception e)
                {
                    return "0";
                }
            }
            else
            {
                try
                {
                    if (value.IsNullOrEmpty())
                    {
                        return defaultValue.ToString();
                    }
                    else
                    {
                        return value;
                    }
                }
                catch (Exception e)
                {
                    return defaultValue.ToString();
                }
            }

            return defaultValue.ToString();
        }
    }
}