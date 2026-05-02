using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Wa7at_ElDr3yah_API.Data;
using Wa7at_ElDr3yah_API.DTOs.Report;
using Wa7at_ElDr3yah_API.Models;
using Wa7at_ElDr3yah_API.Services.Interfaces;

namespace Wa7at_ElDr3yah_API.Services.Implementation
{
    public class MonthlyReportService : IMonthlyReportService
    {
        private readonly AppDbContext _context;

        public MonthlyReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MonthlyReportResponseDto>> GetAllAsync()
        {
            return await _context.MonthlyReports
                .Include(r => r.GeneratedByUser)
                .OrderByDescending(r => r.GeneratedAt)
                .Select(r => new MonthlyReportResponseDto
                {
                    Id = r.Id,
                    Month = r.Month,
                    Year = r.Year,
                    TotalRevenue = r.TotalRevenue,
                    TotalExpenses = r.TotalExpenses,
                    NetProfitOrLoss = r.NetProfitOrLoss,
                    ProfitLossPercentage = r.ProfitLossPercentage,
                    PdfFilePath = r.PdfFilePath,
                    GeneratedByUserName = r.GeneratedByUser.FullName,
                    GeneratedAt = r.GeneratedAt
                })
                .ToListAsync();
        }

        public async Task<MonthlyReportResponseDto?> GetByIdAsync(int id)
        {
            var report = await _context.MonthlyReports
                .Include(r => r.GeneratedByUser)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (report == null)
                return null;

            return MapToDto(report);
        }

        public async Task<MonthlyReportResponseDto> GenerateMonthlyReportAsync(int month, int year, int userId)
        {
            if (month < 1 || month > 12)
                throw new Exception("Month must be between 1 and 12");

            var totalRevenue = await _context.Bookings
                .Where(b => b.BookingDate.Month == month && b.BookingDate.Year == year)
                .SumAsync(b => (decimal?)b.PaidAmount) ?? 0;

            var totalExpenses = await _context.Expenses
                .Where(e => e.ExpenseDate.Month == month && e.ExpenseDate.Year == year)
                .SumAsync(e => (decimal?)e.Amount) ?? 0;

            var netProfitOrLoss = totalRevenue - totalExpenses;

            var profitLossPercentage = totalExpenses == 0
                ? 0
                : (netProfitOrLoss / totalExpenses) * 100;

            var report = new MonthlyReport
            {
                Month = month,
                Year = year,
                TotalRevenue = totalRevenue,
                TotalExpenses = totalExpenses,
                NetProfitOrLoss = netProfitOrLoss,
                ProfitLossPercentage = profitLossPercentage,
                GeneratedByUserId = userId,
                GeneratedAt = DateTime.UtcNow
            };

            _context.MonthlyReports.Add(report);
            await _context.SaveChangesAsync();

            var pdfBytes = await ExportMonthlyReportPdfAsync(month, year);

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"monthly-report-{year}-{month}-{report.Id}.pdf";
            var filePath = Path.Combine(folderPath, fileName);

            await File.WriteAllBytesAsync(filePath, pdfBytes);

            report.PdfFilePath = $"Reports/{fileName}";
            await _context.SaveChangesAsync();

            var savedReport = await _context.MonthlyReports
                .Include(r => r.GeneratedByUser)
                .FirstAsync(r => r.Id == report.Id);

            return MapToDto(savedReport);
        }

        public async Task<byte[]> ExportMonthlyReportPdfAsync(int month, int year)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var capitals = await _context.Capitals
                .Where(c => c.StartDate.Month == month && c.StartDate.Year == year)
                .OrderBy(c => c.StartDate)
                .ToListAsync();

            var bookings = await _context.Bookings
                .Include(b => b.CreatedByUser)
                .Where(b => b.BookingDate.Month == month && b.BookingDate.Year == year)
                .OrderBy(b => b.BookingDate)
                .ToListAsync();

            var expenses = await _context.Expenses
                .Where(e => e.ExpenseDate.Month == month && e.ExpenseDate.Year == year)
                .OrderBy(e => e.ExpenseDate)
                .ToListAsync();

            var totalCapital = capitals.Sum(c => c.Amount);
            var totalRevenue = bookings.Sum(b => b.PaidAmount);
            var updatedCapital = totalCapital + totalRevenue;

            var totalExpenses = expenses.Sum(e => e.Amount);
            var finalBalance = updatedCapital - totalExpenses;

            var netProfitOrLoss = totalRevenue - totalExpenses;

            var profitLossPercentage = totalExpenses == 0
                ? 0
                : (netProfitOrLoss / totalExpenses) * 100;

            var reportTitle = $"التقرير الشهري لواحة الدرعية - {month}/{year}";

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header()
                        .Column(header =>
                        {
                            header.Item()
                                .AlignCenter()
                                .Text("واحة الدرعية")
                                .FontSize(24)
                                .Bold();

                            header.Item()
                                .AlignCenter()
                                .Text(reportTitle)
                                .FontSize(16);

                            header.Item().PaddingTop(10).LineHorizontal(1);
                        });

                    page.Content()
                        .PaddingVertical(20)
                        .Column(col =>
                        {
                            col.Spacing(20);

                            col.Item()
                                .Background("#F3F4F6")
                                .Padding(15)
                                .Column(summary =>
                                {
                                    summary.Item()
                                        .Text("الملخص المالي")
                                        .FontSize(16)
                                        .Bold();

                                    summary.Item().PaddingTop(10).Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                        });

                                        table.Cell().Text("رأس المال المضاف").Bold();
                                        table.Cell().Text($"{totalCapital:N2}");

                                        table.Cell().Text("إيرادات الحجوزات").Bold();
                                        table.Cell().Text($"{totalRevenue:N2}");

                                        table.Cell().Text("رأس المال بعد التحديث").Bold();
                                        table.Cell().Text($"{updatedCapital:N2}");

                                        table.Cell().Text("إجمالي المصروفات").Bold();
                                        table.Cell().Text($"{totalExpenses:N2}");

                                        table.Cell().Text("الرصيد النهائي").Bold();
                                        table.Cell().Text($"{finalBalance:N2}");

                                        table.Cell().Text("صافي الربح أو الخسارة").Bold();
                                        table.Cell().Text($"{netProfitOrLoss:N2}");

                                        table.Cell().Text("نسبة الربح أو الخسارة").Bold();
                                        table.Cell().Text($"{profitLossPercentage:N2}%");
                                    });
                                });

                            col.Item().Text("رأس المال").FontSize(15).Bold();

                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background("#111827").Padding(5).Text("الملاحظات").FontColor("#FFFFFF").Bold();
                                    header.Cell().Background("#111827").Padding(5).Text("التاريخ").FontColor("#FFFFFF").Bold();
                                    header.Cell().Background("#111827").Padding(5).Text("المبلغ").FontColor("#FFFFFF").Bold();
                                });

                                foreach (var c in capitals)
                                {
                                    table.Cell().Padding(5).Text(c.Notes ?? "-");
                                    table.Cell().Padding(5).Text(c.StartDate.ToString("yyyy-MM-dd"));
                                    table.Cell().Padding(5).Text($"{c.Amount:N2}");
                                }
                            });

                            col.Item().Text("الحجوزات").FontSize(15).Bold();

                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background("#111827").Padding(5).Text("العميل").FontColor("#FFFFFF").Bold();
                                    header.Cell().Background("#111827").Padding(5).Text("التاريخ").FontColor("#FFFFFF").Bold();
                                    header.Cell().Background("#111827").Padding(5).Text("دفع").FontColor("#FFFFFF").Bold();
                                    header.Cell().Background("#111827").Padding(5).Text("متبقي").FontColor("#FFFFFF").Bold();
                                    header.Cell().Background("#111827").Padding(5).Text("تم الحجز بواسطة").FontColor("#FFFFFF").Bold();
                                });

                                foreach (var b in bookings)
                                {
                                    table.Cell().Padding(5).Text(b.CustomerName);
                                    table.Cell().Padding(5).Text(b.BookingDate.ToString("yyyy-MM-dd"));
                                    table.Cell().Padding(5).Text($"{b.PaidAmount:N2}");
                                    table.Cell().Padding(5).Text($"{(b.TotalPrice - b.PaidAmount):N2}");
                                    table.Cell().Padding(5).Text(b.CreatedByUser?.FullName?? "Unknown");
                                }
                            });

                            col.Item().Text("المصروفات").FontSize(15).Bold();

                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Background("#111827").Padding(5).Text("تم الانفاق في").FontColor("#FFFFFF").Bold();
                                    header.Cell().Background("#111827").Padding(5).Text("التاريخ").FontColor("#FFFFFF").Bold();
                                    header.Cell().Background("#111827").Padding(5).Text("تم دفع").FontColor("#FFFFFF").Bold();
                                });

                                foreach (var e in expenses)
                                {
                                    table.Cell().Padding(5).Text(e.Title);
                                    table.Cell().Padding(5).Text(e.ExpenseDate.ToString("yyyy-MM-dd"));
                                    table.Cell().Padding(5).Text($"{e.Amount:N2}");
                                }
                            });
                        });

                    page.Footer()
                        .Column(footer =>
                        {
                            footer.Item().LineHorizontal(1);

                            footer.Item()
                                .AlignCenter()
                                .Text($"Generated at {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC")
                                .FontSize(9);
                        });
                });
            });

            return document.GeneratePdf();
        }

        private static MonthlyReportResponseDto MapToDto(MonthlyReport report)
        {
            return new MonthlyReportResponseDto
            {
                Id = report.Id,
                Month = report.Month,
                Year = report.Year,
                TotalRevenue = report.TotalRevenue,
                TotalExpenses = report.TotalExpenses,
                NetProfitOrLoss = report.NetProfitOrLoss,
                ProfitLossPercentage = report.ProfitLossPercentage,
                PdfFilePath = report.PdfFilePath,
                GeneratedByUserName = report.GeneratedByUser.FullName,
                GeneratedAt = report.GeneratedAt
            };
        }
    }
}