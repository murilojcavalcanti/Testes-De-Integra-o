using JornadaMilhas.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Test.Integracao
{
    [Collection(nameof(ContextoCollection))]
    public class OfertaViagemDalRecuperaTodos
    {
        private JornadaMilhasContext Context;

        public OfertaViagemDalRecuperaTodos(JornadaMilhasContext context)
        {
            Context = context;
        }

        [Fact]
        public void RecuperaTodasAsOfertas()
        {
            //arrange
            var dal = new OfertaViagemDAL(Context);
            //act
            var ofertasRecuperadas = dal.RecuperarTodas();
            //assert
            Assert.NotEmpty(ofertasRecuperadas);
        }
    }
}
