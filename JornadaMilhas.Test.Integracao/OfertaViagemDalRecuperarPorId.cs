using JornadaMilhas.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace JornadaMilhas.Test.Integracao
{
    [Collection(nameof(ContextoCollection))]
    public class OfertaViagemDalRecuperarPorId
    {
        private JornadaMilhasContext Context;
        public OfertaViagemDalRecuperarPorId(ITestOutputHelper output, ContextoFixture fixture)
        {
            Context = fixture.Context;
            output.WriteLine(Context.GetHashCode().ToString());
        }
        [Fact]
        public void RetornaNuloQuandoIdInexistente()
        {
            //arrange
            var dal = new OfertaViagemDAL(Context);
            //act
            var ofertaRecuperada = dal.RecuperarPorId(0);
            //assert
            Assert.Null(ofertaRecuperada);

        }
    }
}
