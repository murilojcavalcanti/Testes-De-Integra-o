using JornadaMilhas.Dados;
using JornadaMilhasV1.Modelos;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace JornadaMilhas.Test.Integracao;
[Collection(nameof(ContextoCollection))]
public class OfertaViagemDalAdicionar
{
    private JornadaMilhasContext context;

    public OfertaViagemDalAdicionar(ITestOutputHelper output,ContextoFixture fixture)
    {
        context = fixture.Context;
        output.WriteLine(context.GetHashCode().ToString());
    }
    //Setup
    private OfertaViagem CriaViagem()
    {
        Rota rota = new Rota("S�o paulo", "Fortaleza");
        Periodo periodo = new Periodo(new DateTime(2024, 8, 20), new DateTime(2024, 8, 30));
        double preco = 350;
        return new OfertaViagem(rota, periodo, preco);
    }



    [Fact]
    public void RegistrarOfertaNoBanco()
    {
        //arrange
        

        var oferta = CriaViagem();
        var dal = new OfertaViagemDAL(context);

        //act
        dal.Adicionar(oferta);

        //assert
        var ofertaIncluida = dal.RecuperarPorId(oferta.Id);

        Assert.NotNull(ofertaIncluida);
        Assert.Equal(ofertaIncluida.Preco,oferta.Preco,0.001);
    }

    [Fact]
    public void RegistrarOfertaNoBancoComInformaoesCorretas()
    {
        //arrange

        var oferta = CriaViagem();
        var dal = new OfertaViagemDAL(context);

        //act
        dal.Adicionar(oferta);

        //assert
        var ofertaIncluida = dal.RecuperarPorId(oferta.Id);

        Assert.Equal(ofertaIncluida.Rota.Origem, oferta.Rota.Origem);
        Assert.Equal(ofertaIncluida.Rota.Destino, oferta.Rota.Destino);
        Assert.Equal(ofertaIncluida.Periodo.DataInicial, oferta.Periodo.DataInicial);
        Assert.Equal(ofertaIncluida.Periodo.DataFinal, oferta.Periodo.DataFinal);
        Assert.Equal(ofertaIncluida.Preco, oferta.Preco, 0.001);
    }
}