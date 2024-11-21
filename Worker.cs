using wks_integracao.Model;

namespace wks_integracao
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private Dictionary<IntegrationProcessConfig, DateTime> _integracoes;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            _integracoes = new List<IntegrationProcessConfig>
                {
                    new IntegrationProcessConfig
                    {
                        Nome = "RelatorioSemanal",
                        Periodicidade = TimeSpan.FromMinutes(1),
                        DestinatarioEmail = "destinatario@exemplo.com",
                        AcaoIntegracao = GerarERelatarRelatorioSemanal
                    },
                    new IntegrationProcessConfig
                    {
                        Nome = "RelatorioSemanal2",
                        Periodicidade = TimeSpan.FromMinutes(2),
                        DestinatarioEmail = "destinatario@exemplo.com",
                        AcaoIntegracao = GerarERelatarRelatorioSemanal2
                    },
                    // Outras integrações podem ser adicionadas aqui
                }.ToDictionary(integracao => integracao, integracao => DateTime.Now);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                foreach (var (integracao, proximaExecucao) in _integracoes)
                {
                    if (DateTime.Now >= proximaExecucao)
                    {
                        await integracao.AcaoIntegracao();
                        _integracoes[integracao] = DateTime.Now.Add(integracao.Periodicidade);
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task GerarERelatarRelatorioSemanal()
        {
            _logger.LogInformation("GerarERelatarRelatorioSemanal - running at: {time}", DateTimeOffset.Now);
            //var caminhoRelatorio = await GerarRelatorioExcel();
            //await EnviarEmailComAnexo("destinatario@exemplo.com", caminhoRelatorio);
        }


        private async Task GerarERelatarRelatorioSemanal2()
        {
            _logger.LogInformation("Segundo GerarERelatarRelatorioSemanal - running at: {time}", DateTimeOffset.Now);
            //var caminhoRelatorio = await GerarRelatorioExcel();
            //await EnviarEmailComAnexo("destinatario@exemplo.com", caminhoRelatorio);
        }


        private List<string> getIntegrationList()
        {
            var list = new List<string>() { "censusExcelConhecimento" };

            return list;
        }


        //private async Task<string> GerarRelatorioExcel()
        //{
        //    using var package = new ExcelPackage();
        //    var worksheet = package.Workbook.Worksheets.Add("Relatório Semanal");
        //    worksheet.Cells["A1"].Value = "Coluna1";
        //    worksheet.Cells["B1"].Value = "Coluna2";
        //    // Preencha os dados do relatório
        //    var filePath = Path.Combine(Path.GetTempPath(), "RelatorioSemanal.xlsx");
        //    await package.SaveAsAsync(new FileInfo(filePath));
        //    return filePath;
        //}

        //private async Task EnviarEmailComAnexo(string destinatario, string caminhoArquivo)
        //{
        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("Sistema", "sistema@exemplo.com"));
        //    message.To.Add(MailboxAddress.Parse(destinatario));
        //    message.Subject = "Relatório Semanal";

        //    var body = new TextPart("plain") { Text = "Segue o relatório semanal em anexo." };
        //    var attachment = new MimePart("application", "vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        //    {
        //        Content = new MimeContent(File.OpenRead(caminhoArquivo)),
        //        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
        //        FileName = Path.GetFileName(caminhoArquivo)
        //    };
        //    var multipart = new Multipart("mixed") { body, attachment };

        //    message.Body = multipart;

        //    using var client = new SmtpClient();
        //    await client.ConnectAsync("smtp.exemplo.com", 587, SecureSocketOptions.StartTls);
        //    await client.AuthenticateAsync("usuario", "senha");
        //    await client.SendAsync(message);
        //    await client.DisconnectAsync(true);
        //}

    }
}