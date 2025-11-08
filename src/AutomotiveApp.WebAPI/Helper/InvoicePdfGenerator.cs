using AutomotiveApp.Application.Invoices;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AutomotiveApp.WebAPI.Helper
{
    public static class InvoicePdfGenerator
    {
        private const string BrandColor = "#790b0a";

        public static byte[] Generate(InvoiceDetailsDto details)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10).FontColor("#333333"));

                    // Head
                    page.Header().Column(col =>
                    {
                        col.Spacing(6); 
                        col.Item().Text("INVOICE OTOMOBIL").FontSize(16).Bold().FontColor(BrandColor);
                        col.Item().PaddingTop(4).LineHorizontal(1).LineColor(BrandColor);
                    });

                    // Body
                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Spacing(12);

                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(left =>
                            {
                                left.Spacing(2);
                                left.Item().Text("Information").Bold();
                                left.Item().Text(t =>
                                {
                                    t.Span("No Invoice: ").SemiBold();
                                    t.Span(details.InvoiceCode ?? "-");
                                });

                                left.Item().Text(t =>
                                {
                                    t.Span("Date: ").SemiBold();
                                    t.Span(details.CreatedAt.ToString("dd MMMM yyyy"));
                                });

                                left.Item().Text(t =>
                                {
                                    t.Span("Payment Method: ").SemiBold();
                                    t.Span(details.PaymentMethod ?? "-");
                                });
                            });

                            row.RelativeItem().Column(right =>
                            {
                                right.Spacing(2);
                                right.Item().Text("Customer").Bold();
                                right.Item().Text(DeriveNameFromEmail(details.CustomerEmail));
                                right.Item().Text(details.CustomerEmail ?? "-");
                            });
                        });

                        // Table
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(20);
                                columns.RelativeColumn(4);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderCell).Text("No");
                                header.Cell().Element(HeaderCell).Text("Course Name");
                                header.Cell().Element(HeaderCell).Text("Type");
                                header.Cell().Element(HeaderCell).Text("Schedule");
                                header.Cell().Element(HeaderCell).AlignRight().Text("Price");

                                static IContainer HeaderCell(IContainer c) =>
                                    c.Border(1).BorderColor(Colors.Grey.Darken2).PaddingVertical(4).PaddingHorizontal(3).DefaultTextStyle(t => t.SemiBold());
                            });

                            var index = 0;
                            foreach (var item in details.Items)
                            {
                                index++;

                                static IContainer RowCell(IContainer c) =>
                                    c.Border(1).BorderColor(Colors.Grey.Darken2).PaddingVertical(3).PaddingHorizontal(3).Background(Colors.White);

                                table.Cell().Element(RowCell).Text(index.ToString());
                                table.Cell().Element(RowCell).Text(item.CourseName);
                                table.Cell().Element(RowCell).Text(item.Type);
                                table.Cell().Element(RowCell).Text(item.Schedule.ToString("dddd, dd MMMM yyyy"));
                                table.Cell().Element(RowCell).AlignRight().Text(FormatIdr(item.Price));
                            }

                        });

                        col.Item().AlignRight().Text(t =>
                        {
                            t.Span("Total: ").SemiBold();
                            t.Span(FormatIdr(details.TotalPrice)).SemiBold();
                        });
                    });

                    // Footer
                    page.Footer().AlignCenter().Text(x =>
                        x.Span("© 2025 Otomobil. All rights reserved.").FontSize(7).FontColor(Colors.Grey.Medium)
                    );
                });
            });

            return document.GeneratePdf();
        }

        // Name Display
        private static string DeriveNameFromEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email)) return "-";
            var at = email.IndexOf('@');
            var local = at > 0 ? email[..at] : email;

            var parts = local
                .Replace('.', ' ')
                .Replace('_', ' ')
                .Replace('-', ' ')
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return string.Join(' ', parts.Select(p =>
                char.ToUpperInvariant(p[0]) + (p.Length > 1 ? p[1..].ToLowerInvariant() : "")));
        }

        // IDR Display
        private static string FormatIdr(decimal value) => $"IDR {value:N0}";
    }
}
