using System.Globalization;
using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using MyVdsFactory.Application.Common.Extensions;
using MyVdsFactory.Application.Common.Interfaces;
using MyVdsFactory.Application.Common.Models;
using MyVdsFactory.Domain.Entities;

namespace MyVdsFactory.Application.Horoscopes.Commands.AddHoroscopeWithExcelCommand;

public class AddHoroscopeWithExcelCommand : IRequest<Result<long>>
{
    public IFormFile ExcelFile { get; set; }
    public class Handler : IRequestHandler<AddHoroscopeWithExcelCommand, Result<long>>
    {
        private readonly IApplicationContext _context;

        public Handler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<Result<long>> Handle(AddHoroscopeWithExcelCommand request, CancellationToken cancellationToken)
        {
            List<Horoscope> horoscopes = new List<Horoscope>();
            
            using (var workbook = new XLWorkbook(request.ExcelFile.OpenReadStream()))
            {
                var worksheet = workbook.Worksheets.First();
                var rows = worksheet.Rows();
                int rowItemIndex = 0;

                foreach (var row in rows)
                {
                    if (rowItemIndex > 0)
                    {
                        horoscopes.Add(new Horoscope
                        {
                            Name = row.Cell(1)?.Value.ToString()!,
                            PhotoName = row.Cell(2)?.Value.ToString()!,
                            DateRange = row.Cell(3)?.Value.ToString()!,
                            NormalizedName = row.Cell(1)?.Value.ToString()!.ReplaceTurkishCharacters()!,
                            Planet = row.Cell(4)?.Value.ToString()!,
                            Group = row.Cell(5)?.Value.ToString()!,
                            Description = row.Cell(6)?.Value.ToString()!,
                        });
                    }
                    rowItemIndex++;
                }
            }
            
            await _context.Horoscopes.AddRangeAsync(horoscopes, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<long>.Success(1,"Burc bilgileri veritabanına başarıyla kaydedildi.");
        }
    }
}