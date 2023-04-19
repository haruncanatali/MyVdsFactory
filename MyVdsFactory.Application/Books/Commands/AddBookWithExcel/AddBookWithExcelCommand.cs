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

namespace MyVdsFactory.Application.Books.Commands.AddBookWithExcel;

public class AddBookWithExcelCommand : IRequest<Result<long>>
{
    public IFormFile ExcelDataFile { get; set; }
    
    public class Handler : IRequestHandler<AddBookWithExcelCommand, Result<long>>
    {
        private readonly IApplicationContext _context;
        private readonly ILogger<AddBookWithExcelCommand> _logger;

        public Handler(IApplicationContext context, ILogger<AddBookWithExcelCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<long>> Handle(AddBookWithExcelCommand request, CancellationToken cancellationToken)
        {
            using (var workbook = new XLWorkbook(request.ExcelDataFile.OpenReadStream()))
            {
                foreach (var worksheet in workbook.Worksheets)
                {
                    var bookRows = worksheet.Rows();
                    _logger.LogInformation($"{worksheet.Name} excel sayfası açıldı.");
                    foreach (var row in bookRows)
                    {
                        if (!row.Cell(2).Value.ToString().IsNullOrEmpty())
                        {
                            var authorName = row.Cell(1).Value.ToString().SafeTrim();
                            var bookName = row.Cell(2).Value.ToString().SafeTrim();
                            var author = await _context.Authors
                                .FirstOrDefaultAsync(c => c.FullName == authorName,cancellationToken);

                            var isBookAlreadyAdded = await _context.Books
                                .AnyAsync(c => c.Name == bookName,cancellationToken);
                            
                            if (author == null && !isBookAlreadyAdded)
                            {
                                author = new Author
                                {
                                    FullName = authorName
                                };
                                await _context.Authors.AddAsync(author, cancellationToken);
                                await _context.SaveChangesAsync(cancellationToken);
                            }

                            if (!isBookAlreadyAdded)
                            {
                                await _context.Books.AddAsync(new Book
                                {
                                    Name = bookName,
                                    AurhorId = author.Id
                                }, cancellationToken);
                                await _context.SaveChangesAsync(cancellationToken);
                                _logger.LogInformation($"{bookName} - {authorName} nesnesi eklendi.");
                            }
                            else
                            {
                                _logger.LogInformation($"Mükerrer kayıt ekleme girişimi - {bookName}");
                            }
                            
                        }
                    }
                }
                
            }
            
            _logger.LogInformation($"Yazar-Kitap exceli okundu ve veritabanına kaydedildi.");
            
            return Result<long>.Success(1,"Yazar-Kitap exceli okundu ve veritabanına kaydedildi.");
        }
    }
}