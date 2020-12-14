using System;
using System.Threading;
using System.Threading.Tasks;
using IronPdf;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerService
{
    public class PdfService : IHostedService
    {
        private readonly ILogger<PdfService> _logger;

        public PdfService(ILogger<PdfService> logger)
        {
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Warming up IronPdf");
                var document = await Generate("<h1>Hello World<h1>");
                _logger.LogInformation("IronPdf wormed up: {PdfLength}", document.Length);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception while worming up Pdf");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task<byte[]> Generate(string template)
        {
            var generator = new HtmlToPdf(new PdfPrintOptions
            {
                MarginBottom = 1,
                MarginLeft = 1,
                MarginRight = 1,
                MarginTop = 1,
                PaperSize = PdfPrintOptions.PdfPaperSize.A4
            });

            var document = await generator.RenderHtmlAsPdfAsync(template);

            return document.BinaryData;
        }
    }
}
