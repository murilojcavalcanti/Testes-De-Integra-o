using Bogus;
using JornadaMilhasV1.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Test.Integracao
{
    public class PeriodoDataBuilder : Faker<Periodo>
    {
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public PeriodoDataBuilder()
        {
            CustomInstantiator(f =>
                  {
                      DateTime dataInicio = DataInicial ?? f.Date.Soon();
                      DateTime dataFim = DataFinal ?? f.Date.Soon();

                      return new Periodo(dataInicio, dataFim);
                  });
        }
        public Periodo Build() => Generate();
    }
}
