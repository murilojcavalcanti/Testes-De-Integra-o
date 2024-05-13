using JornadaMilhas.Dados;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Test.Integracao
{
    //Essa classe serve para criarmos apenas uma instancia do context, assim não iremos criar diversas conexões com o banco de dados.
    public class ContextoFixture
    {
        public JornadaMilhasContext Context { get; }

        public ContextoFixture()
        {
            var options = new DbContextOptionsBuilder<JornadaMilhasContext>
            ().UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JornadaMilhas;" +
            "Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;" +
            "Application Intent=ReadWrite;Multi Subnet Failover=False").Options;
            
            Context = new JornadaMilhasContext(options);

        }
    }
}
