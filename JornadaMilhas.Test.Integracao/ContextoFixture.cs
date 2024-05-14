using Bogus;
using JornadaMilhas.Dados;
using JornadaMilhasV1.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;

namespace JornadaMilhas.Test.Integracao
{
    //Essa classe serve para criarmos apenas uma instancia do context, assim não iremos criar diversas conexões com o banco de dados.
    public class ContextoFixture //: IAsyncLifetime<- Interface usada para conseguir usar o docker nos tests
    {
        public JornadaMilhasContext Context;
        //private readonly MsSqlContainer msSqlContainer = new MsSqlBuilder().WithImage("mcr.microsoft.com/mssql/server").Build(); 

        public ContextoFixture()
        {
            
            var options = new DbContextOptionsBuilder<JornadaMilhasContext>
            ().UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JornadaMilhas;" +
            "Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;" +
            "Application Intent=ReadWrite;Multi Subnet Failover=False").Options;
            
            Context = new JornadaMilhasContext(options);
            Context.Database.Migrate();
        }

        public void CriaDadosFake()
        {
            Periodo periodo = new PeriodoDataBuilder().Build();

            var rota = new Rota("Curitiba", "São Paulo");

            var fakerOferta = new Faker<OfertaViagem>()
                .CustomInstantiator(f => new OfertaViagem(
                    rota,
                    new PeriodoDataBuilder().Build(),
                    100 * f.Random.Int(1, 100))
                )
                .RuleFor(o => o.Desconto, f => 40)
                .RuleFor(o => o.Ativa, f => true);

            var lista = fakerOferta.Generate(200);

            if (lista is null)
            {
                Console.WriteLine("lista nula"); 
               
            }
            
            Context.OfertasViagem.AddRange(lista);
            Context.SaveChanges();
        }

        public async Task LimpaDadosDoBanco()
        {
            /*
            Context.OfertasViagem.RemoveRange(Context.OfertasViagem);
            Context.Rotas.RemoveRange(Context.Rotas);
            await Context.SaveChangesAsync();*/

            Context.Database.ExecuteSqlRaw("DELETE FROM OfertasViagem" );
            Context.Database.ExecuteSqlRaw("DELETE FROM Rotas" );
        }
        /*
         * usando o docker
        public async Task InitializeAsync()
        {
            await msSqlContainer.StartAsync();
            var options = new DbContextOptionsBuilder<JornadaMilhasContext>
           ().UseSqlServer(msSqlContainer.GetConnectionString()).Options;

            
            Context = new JornadaMilhasContext(options);
            Context.Database.Migrate();
            
        }

        public async Task DisposeAsync()
        {
            await msSqlContainer.StopAsync();
        }
        */
    }
}
