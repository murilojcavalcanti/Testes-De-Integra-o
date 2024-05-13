using JornadaMilhas.Dados;
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
        private readonly MsSqlContainer msSqlContainer = new MsSqlBuilder().WithImage("mcr.microsoft.com/mssql/server").Build(); 

        public ContextoFixture()
        {
            
            var options = new DbContextOptionsBuilder<JornadaMilhasContext>
            ().UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JornadaMilhas;" +
            "Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;" +
            "Application Intent=ReadWrite;Multi Subnet Failover=False").Options;
            Context = new JornadaMilhasContext(options);
            

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
