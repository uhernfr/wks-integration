using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wks_integracao.Model
{
    public class IntegrationProcessConfig
    {
        public string Nome { get; set; }
        public TimeSpan Periodicidade { get; set; }
        public string DestinatarioEmail { get; set; }
        public Func<Task> AcaoIntegracao { get; set; } // Delegado para a ação a ser executada
    }
}
