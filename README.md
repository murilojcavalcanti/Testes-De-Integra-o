# Ementa detalhada com códigos por vídeo
# Commits da Branch aula01-video1.4

# eddf8cc - Video 1.2 - Iniciando em testes de integração

* Nossa aplicação do Jornada Milhas agora tem um componente externo que é a integração com o banco de dados e precisamos testar nossa aplicação nesse novo cenário
* Já temos aqui então o nosso projeto de testes de integração que você criou na atividade anterior e vamos começar a trabalhar com os testes
* Agora como temos essa integração com o banco de dados, é a classe DAL que é responsável pela manipulação das ofertas, então podemos iniciar os testes por ela
* Temos então nosso primeiro método que é o `Adicionar` responsável por cadastrar uma oferta no banco, vamos começar testando ele

* Primeiro passo então vai ser renomear a classe de teste com base no padrão que já aprendemos no curso anterior, identificando a classe e o método que será testado - OfertaViagemDALAdicionar
* E podemos criar o primeiro teste pra verificar se a oferta está sendo adicionada corretamente no banco:

 ## Added JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
```diff --git a/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs b/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
new file mode 100644
index 0000000..8f716b8
--- /dev/null
+++ b/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
@@ -0,0 +1,27 @@
+using JornadaMilhasV1.Modelos;
+using JornadaMilhas.Dados;
+
+namespace JornadaMilhas.Test.Integracao;
+
+public class OfertaViagemDALAdicionar
+{
+    [Fact]
+    public void RegistraOfertaNoBanco()
+    {
+        // arrange
+        Rota rota = new Rota("S�o Paulo", "Fortaleza");
+        Periodo periodo = new Periodo(new DateTime(2024, 8, 20), new DateTime(2024, 8, 30));
+        double preco = 350;
+
+        var oferta = new OfertaViagem(rota, periodo, preco);
+        var dal = new OfertaViagemDAL();
+
+        // act
+        dal.Adicionar(oferta);
+
+        // assert
+        var ofertaIncluida = dal.RecuperarPorId(oferta.Id);
+        Assert.NotNull(ofertaIncluida);
+        Assert.Equal(ofertaIncluida.Preco, oferta.Preco, 0.001);
+    }
+}
\ No newline at end of file
```

 ## Deleted JornadaMilhas.Test.Integracao/UnitTest1.cs
```diff --git a/JornadaMilhas.Test.Integracao/UnitTest1.cs b/JornadaMilhas.Test.Integracao/UnitTest1.cs
deleted file mode 100644
index d58caae..0000000
--- a/JornadaMilhas.Test.Integracao/UnitTest1.cs
+++ /dev/null
@@ -1,10 +0,0 @@
-namespace JornadaMilhas.Test.Integracao;
-
-public class UnitTest1
-{
-    [Fact]
-    public void Test1()
-    {
-
-    }
-}
\ No newline at end of file
```

 ## Modified src/JornadaMilhasV1/Dados/OfertaViagemDAL.cs
```diff --git a/src/JornadaMilhasV1/Dados/OfertaViagemDAL.cs b/src/JornadaMilhasV1/Dados/OfertaViagemDAL.cs
index 6d56a2b..cdac0f1 100644
--- a/src/JornadaMilhasV1/Dados/OfertaViagemDAL.cs
+++ b/src/JornadaMilhasV1/Dados/OfertaViagemDAL.cs
@@ -7,7 +7,7 @@ using System.Text;
 using System.Threading.Tasks;
 
 namespace JornadaMilhas.Dados;
-internal class OfertaViagemDAL
+public class OfertaViagemDAL
 {
     private readonly JornadaMilhasContext context;
 
```

# afe6515 - Video 1.2 - Iniciando em testes de integração [Parte 2]

* Executando nosso teste dá tudo certo e ele passa, então a viagem está sendo cadastrada no banco.
* Porém, se a gente analisar o DAL, ela faz referência a um context que tem o endereço do nosso banco - que é um componente externo.
* Com isso nossos testes dependem diretamente do funcionamento desse componente externo para funcionar corretamente

* E o que aconteceria com nossos testes se o endereço desse banco fosse alterado, por exemplo?
* Vamos na classe context e alterar o nome do banco - inserir Teste
* Agora vamos executar novamente o teste

* Com essa alteração de identificação do banco nosso teste parou de funcionar pois não consegue mais se conectar com o banco.
* Isso é um grande problema pois torna os nossos testes frágeis, a gente nunca sabe se nossos testes vão funcionar ou não porque estamos dependendo de um componente externo para que funcione corretamente. E isso fere um dos pontos principais do testes de integração que é o determinismo - que é justamente essa capacidade de prever e garantir que os resultados obtidos nos testes serão sempre os mesmos, sem variação.

* Então no proximo vídeo vamos entender mais sobre testes de integração e como garantir maior controle sobre eles.

 ## Modified src/JornadaMilhasV1/Dados/JornadaMilhasContext.cs
```diff --git a/src/JornadaMilhasV1/Dados/JornadaMilhasContext.cs b/src/JornadaMilhasV1/Dados/JornadaMilhasContext.cs
index 762079c..d29137a 100644
--- a/src/JornadaMilhasV1/Dados/JornadaMilhasContext.cs
+++ b/src/JornadaMilhasV1/Dados/JornadaMilhasContext.cs
@@ -21,7 +21,7 @@ internal class JornadaMilhasContext: DbContext
     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
     {
         optionsBuilder
-            .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JornadaMilhasEnsaio;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
+            .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JornadaMilhasEnsaioTeste;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
     }
 
     protected override void OnModelCreating(ModelBuilder modelBuilder)
```

# 3978c07 - Video 1.3 - Baixo acoplamento

* Temos um problema para resolver aqui então - precisamos garantir que nosso teste seja determinístico, ou seja, funcione independente da situação.
* Pra isso a gente precisa ter mais controle sobre o componente externo que estamos utilizando - que nesse caso é o banco

* Então o que precisamos fazer aqui é trazer aquela informação de conexão pra dentro do nosso teste pra que ele tenha esse controle do ambiente e informar pro dal qual contexto ele vai utilizar sempre que for instanciado
* E adicionar também uma condição para utilização do context padrão

* Feitas essas alterações quando executamos novamente o teste ele funciona como o esperado pois agora está buscando os dados de conexão que passamos na configuração de cenário do teste.
* Então independente do estado do banco original, temos nosso banco de testes com controle total nos testes e com isso garantimos que eles estarão sempre com o mesmo retorno.

* O que a gente fez durante esse vídeo é chamado de baixo acoplamento, que é a independência dos componentes que estão sendo testados em relação aos componentes externos. Ou seja, quando isolamos a conexão com o banco na nossa classe de testes conseguimos criar testes de integração de baixo acoplamento, garantindo maior controle sobre ele e também que nossos testes sempre terão o mesmo resultado independente da condição do componente externo que está sendo testado.

* No próximo vídeo vamos analisar o que fizemos e verificar se podemos melhorar esse controle.

 ## Modified JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
```diff --git a/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs b/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
index 8f716b8..4044149 100644
--- a/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
+++ b/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
@@ -1,5 +1,6 @@
 using JornadaMilhasV1.Modelos;
 using JornadaMilhas.Dados;
+using Microsoft.EntityFrameworkCore;
 
 namespace JornadaMilhas.Test.Integracao;
 
@@ -13,8 +14,14 @@ public class OfertaViagemDALAdicionar
         Periodo periodo = new Periodo(new DateTime(2024, 8, 20), new DateTime(2024, 8, 30));
         double preco = 350;
 
+        var options = new DbContextOptionsBuilder<JornadaMilhasContext>()
+            .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JornadaMilhasEnsaio;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False")
+            .Options;
+
+        var context = new JornadaMilhasContext(options);
+
         var oferta = new OfertaViagem(rota, periodo, preco);
-        var dal = new OfertaViagemDAL();
+        var dal = new OfertaViagemDAL(context);
 
         // act
         dal.Adicionar(oferta);
```

 ## Modified src/JornadaMilhasV1/Dados/JornadaMilhasContext.cs
```diff --git a/src/JornadaMilhasV1/Dados/JornadaMilhasContext.cs b/src/JornadaMilhasV1/Dados/JornadaMilhasContext.cs
index d29137a..1b26cd0 100644
--- a/src/JornadaMilhasV1/Dados/JornadaMilhasContext.cs
+++ b/src/JornadaMilhasV1/Dados/JornadaMilhasContext.cs
@@ -9,7 +9,7 @@ using static System.Net.Mime.MediaTypeNames;
 using static System.Runtime.InteropServices.JavaScript.JSType;
 
 namespace JornadaMilhas.Dados;
-internal class JornadaMilhasContext: DbContext
+public class JornadaMilhasContext: DbContext
 {
     public DbSet<OfertaViagem> OfertasViagem { get; set; }
     public DbSet<Rota> Rotas { get; set; }
@@ -20,8 +20,8 @@ internal class JornadaMilhasContext: DbContext
 
     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
     {
-        optionsBuilder
-            .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JornadaMilhasEnsaioTeste;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
+        if(!optionsBuilder.IsConfigured)
+            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JornadaMilhasEnsaioTeste;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
     }
 
     protected override void OnModelCreating(ModelBuilder modelBuilder)
```

 ## Modified src/JornadaMilhasV1/Dados/OfertaViagemDAL.cs
```diff --git a/src/JornadaMilhasV1/Dados/OfertaViagemDAL.cs b/src/JornadaMilhasV1/Dados/OfertaViagemDAL.cs
index cdac0f1..8d88af7 100644
--- a/src/JornadaMilhasV1/Dados/OfertaViagemDAL.cs
+++ b/src/JornadaMilhasV1/Dados/OfertaViagemDAL.cs
@@ -11,9 +11,9 @@ public class OfertaViagemDAL
 {
     private readonly JornadaMilhasContext context;
 
-    public OfertaViagemDAL()
+    public OfertaViagemDAL(JornadaMilhasContext context)
     {
-        context = new JornadaMilhasContext();
+        this.context = context;
     }
 
     public void Adicionar(OfertaViagem oferta)
```

 ## Modified src/JornadaMilhasV1/Gerenciador/GerenciadorDeOfertas.cs
```diff --git a/src/JornadaMilhasV1/Gerenciador/GerenciadorDeOfertas.cs b/src/JornadaMilhasV1/Gerenciador/GerenciadorDeOfertas.cs
index e848ce9..860d09b 100644
--- a/src/JornadaMilhasV1/Gerenciador/GerenciadorDeOfertas.cs
+++ b/src/JornadaMilhasV1/Gerenciador/GerenciadorDeOfertas.cs
@@ -10,7 +10,7 @@ namespace JornadaMilhasV1.Gerenciador;
 public class GerenciadorDeOfertas
 {
     private List<OfertaViagem> ofertaViagem = new List<OfertaViagem>();
-    OfertaViagemDAL ofertaViagemDAL = new OfertaViagemDAL();
+    OfertaViagemDAL ofertaViagemDAL = new OfertaViagemDAL(new JornadaMilhasContext());
 
     public GerenciadorDeOfertas(List<OfertaViagem> ofertaViagem)
     {
```


# 354790a - Video 1.4 - Aproveitando recursos

* Vamos criar um novo teste então pra continuar verificando o método adicionar
* Nosso teste agora vai verificar se as informações estão sendo inseridas corretamente no banco

* Mas aqui no arrange gente já se depara com a repetição na conexão do banco, a gente está escrevendo duas vezes o mesmo código, então como podemos aproveitar esse código?
* Se todos os métodos de teste aqui estão utilizando essa conexão, podemos levar esse código para o construtor.

* Existem várias maneiras de fazer esse aproveitamento na classe de testes, e o que a gente fez foi preparar o nosso cenário para a execução dos teste. Então toda vez que executarmos os testes nossa classe ira executar esse código do construtor gerando a conexão que a gente precisa com o banco.

* Aqui no XUnit a gente chama essa preparação de Setup e ajuda a garantir que o ambiente de testes estará configurado com as informações iniciais necessárias para os testes, sem precisar fazer essa mesma configuração em cada um dos testes.

* No próximo vídeo vamos continuar avançando em testes de integração.

 ## Modified JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
```diff --git a/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs b/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
index 4044149..72ea02b 100644
--- a/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
+++ b/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
@@ -6,6 +6,17 @@ namespace JornadaMilhas.Test.Integracao;
 
 public class OfertaViagemDALAdicionar
 {
+    private readonly OfertaViagemDAL dal;
+    public OfertaViagemDALAdicionar()
+    {
+        var options = new DbContextOptionsBuilder<JornadaMilhasContext>()
+            .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JornadaMilhasEnsaio;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False")
+            .Options;
+
+        var context = new JornadaMilhasContext(options);
+        dal = new OfertaViagemDAL(context);
+    }
+
     [Fact]
     public void RegistraOfertaNoBanco()
     {
@@ -14,14 +25,8 @@ public class OfertaViagemDALAdicionar
         Periodo periodo = new Periodo(new DateTime(2024, 8, 20), new DateTime(2024, 8, 30));
         double preco = 350;
 
-        var options = new DbContextOptionsBuilder<JornadaMilhasContext>()
-            .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JornadaMilhasEnsaio;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False")
-            .Options;
-
-        var context = new JornadaMilhasContext(options);
-
         var oferta = new OfertaViagem(rota, periodo, preco);
-        var dal = new OfertaViagemDAL(context);
+        
 
         // act
         dal.Adicionar(oferta);
@@ -31,4 +36,26 @@ public class OfertaViagemDALAdicionar
         Assert.NotNull(ofertaIncluida);
         Assert.Equal(ofertaIncluida.Preco, oferta.Preco, 0.001);
     }
-}
\ No newline at end of file
+
+    [Fact]
+    public void RegistraOfertaNoBancoComInformacoesCorretas()
+    {
+        // Arrange
+        Rota rota = new Rota("Origem1", "Destino1");
+        Periodo periodo = new Periodo(new DateTime(2024, 8, 20), new DateTime(2024, 8, 30));
+        double preco = 350;
+
+        var oferta = new OfertaViagem(rota, periodo, preco);
+
+        // Act
+        dal.Adicionar(oferta);
+
+        // Assert
+        var ofertaIncluida = dal.RecuperarPorId(oferta.Id);
+        Assert.Equal(ofertaIncluida.Rota.Origem, oferta.Rota.Origem);
+        Assert.Equal(ofertaIncluida.Rota.Destino, oferta.Rota.Destino);
+        Assert.Equal(ofertaIncluida.Periodo.DataInicial, oferta.Periodo.DataInicial);
+        Assert.Equal(ofertaIncluida.Periodo.DataFinal, oferta.Periodo.DataFinal);
+        Assert.Equal(ofertaIncluida.Preco, oferta.Preco, 0.001);
+    }
+}
```

# 737f643 - Video 2.1 - Compartilhando a conexão

* Temos aqui então na nossa classe de teste dois métodos que estão testando registro de ofertas no banco de dados e passamos os dados de conexão para o construtor da classe
* Pra gente conseguir analisar melhor como a execução desses métodos funciona por debaixo dos panos e verificar possíveis melhorias, vamos adicionar uma saída que mostre pra gente os objetos que são criados ao executar os testes.
* Para isso vamos adicionar um output pra exibir uma mensagem pra gente quando criar a conexão

* Depois de executar os testes, se a gente reparar nos detalhes percebemos que para cada um dos testes foi criada uma conexão.
* E se a gente voltar lá nos conceitos de OO, esse comportamento acaba sendo contraintuitivo, pois o que faria sentido aqui seria criar um único objeto e todos os métodos utilizaram o mesmo

* Então no próximo vídeo vamos entender melhor como solucionar essa questão e deixar nossos testes de integração mais otimizados

 ## Modified JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
```diff --git a/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs b/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
index 72ea02b..42684d8 100644
--- a/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
+++ b/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
@@ -1,13 +1,15 @@
 using JornadaMilhasV1.Modelos;
 using JornadaMilhas.Dados;
 using Microsoft.EntityFrameworkCore;
+using Xunit.Abstractions;
 
 namespace JornadaMilhas.Test.Integracao;
 
 public class OfertaViagemDALAdicionar
 {
     private readonly OfertaViagemDAL dal;
-    public OfertaViagemDALAdicionar()
+    private readonly ITestOutputHelper output;
+    public OfertaViagemDALAdicionar(ITestOutputHelper output)
     {
         var options = new DbContextOptionsBuilder<JornadaMilhasContext>()
             .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JornadaMilhasEnsaio;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False")
@@ -15,6 +17,9 @@ public class OfertaViagemDALAdicionar
 
         var context = new JornadaMilhasContext(options);
         dal = new OfertaViagemDAL(context);
+        this.output = output;
+
+        output.WriteLine("Criou a conex�o");
     }
 
     [Fact]
```


# dae427d - Video 2.2 - ClassFixture

* Estamos então com um problema para resolver: o XUnit cria um objeto para cada método de testes e isso, além de ser contraintuituvo com OO é também um gasto a mais de recursos
* Para resolver essa questão existem algumas alternativas, e a que escolhemos aplicar aqui é o ClassFixture.

* Primeiro passo para aplicar esta solução é criar uma classe que será responsável pela nossa conexão, o nosso contexto  - ContextoFixture
* Depois de passar todos os dados da conexão para a classe `ContextoFixture`, vamos adicionar uma interface à classe de testes para utilizar essa fixture e passar como parâmetro do construtor também
* Para garantir que estamos utilizando o mesmo objeto de conexão agora, ao invés daquela mensagem padrão que escrevemos anteriormente, podemos buscar o hashCode do objeto criado - para isso vamos alterar o output.WriteLine

* Então o que fizemos aqui foi garantir que nossos métodos de teste utilizassem o mesmo objeto de conexão criado e assim agora conseguimos otimizar os recursos utilizados nos nossos testes.

* No próximo vídeo vamos continuar entendendo sobre o compartilhamento das conexões.

 ## Added JornadaMilhas.Test.Integracao/ContextoFixture.cs
```diff --git a/JornadaMilhas.Test.Integracao/ContextoFixture.cs b/JornadaMilhas.Test.Integracao/ContextoFixture.cs
new file mode 100644
index 0000000..357482d
--- /dev/null
+++ b/JornadaMilhas.Test.Integracao/ContextoFixture.cs
@@ -0,0 +1,22 @@
+﻿using JornadaMilhas.Dados;
+using Microsoft.EntityFrameworkCore;
+using System;
+using System.Collections.Generic;
+using System.Linq;
+using System.Text;
+using System.Threading.Tasks;
+
+namespace JornadaMilhas.Test.Integracao;
+public class ContextoFixture
+{
+    public JornadaMilhasContext Context { get; }
+    public ContextoFixture()
+    {
+        var options = new DbContextOptionsBuilder<JornadaMilhasContext>()
+        .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JornadaMilhasEnsaio;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False")
+        .Options;
+
+        Context = new JornadaMilhasContext(options);
+    }
+    
+}
```

 ## Modified JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
```diff --git a/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs b/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
index 42684d8..26b8e95 100644
--- a/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
+++ b/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
@@ -5,21 +5,16 @@ using Xunit.Abstractions;
 
 namespace JornadaMilhas.Test.Integracao;
 
-public class OfertaViagemDALAdicionar
+public class OfertaViagemDALAdicionar: IClassFixture<ContextoFixture>
 {
     private readonly OfertaViagemDAL dal;
     private readonly ITestOutputHelper output;
-    public OfertaViagemDALAdicionar(ITestOutputHelper output)
+    public OfertaViagemDALAdicionar(ITestOutputHelper output, ContextoFixture fixture)
     {
-        var options = new DbContextOptionsBuilder<JornadaMilhasContext>()
-            .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JornadaMilhasEnsaio;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False")
-            .Options;
-
-        var context = new JornadaMilhasContext(options);
-        dal = new OfertaViagemDAL(context);
+        dal = new OfertaViagemDAL(fixture.Context);
         this.output = output;
 
-        output.WriteLine("Criou a conex�o");
+        output.WriteLine(fixture.Context.GetHashCode().ToString());
     }
 
     [Fact]
```


# deeb45f - Video 2.3 - CollectionFixture

* Já estamos compartilhando a conexão com os métodos da nossa classe de teste
* Agora podemos continuar avançando nos testes do DAL - próximo método que vamos testar é o RecuperarPorId

* Vamos criar então a nova classe de testes
* E ela vai seguir o mesmo fluxo de utilização do ContextoFixture

* Se a gente reparar, o contexto que estamos utilizando é exatamente o mesmo, porém o XUnit cria um objeto para cada classe de teste que executamos. Será que existe uma maneira de aproveitar esse objeto de conexão entre as classes?
* Pra isso podemos utilizar um outro conceito que é o CollectionFixture

* Vamos criar uma nova classe chamada `ContextoCollection` que vai implementar a interface ICollectionFixture da nossa ContextoFixture

Feito isso agora a gente consegue utilizar o mesmo contexto para duas ou mais classes de testes diferentes que utilizem essa mesma conexão, otimizando assim bastante os recursos pois agora não precisamos criar e descruir uma conexão com o banco a cada teste executado.

* Na próxima aula vamos continuar avançando em testes de integração

 ## Added JornadaMilhas.Test.Integracao/ContextoCollection.cs
```diff --git a/JornadaMilhas.Test.Integracao/ContextoCollection.cs b/JornadaMilhas.Test.Integracao/ContextoCollection.cs
new file mode 100644
index 0000000..32a2ec5
--- /dev/null
+++ b/JornadaMilhas.Test.Integracao/ContextoCollection.cs
@@ -0,0 +1,11 @@
+﻿using System;
+using System.Collections.Generic;
+using System.Linq;
+using System.Text;
+using System.Threading.Tasks;
+
+namespace JornadaMilhas.Test.Integracao;
+[CollectionDefinition(nameof(ContextoCollection))]
+public class ContextoCollection: ICollectionFixture<ContextoFixture>
+{
+}
```

 ## Modified JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
```diff --git a/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs b/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
index 26b8e95..8a3fa43 100644
--- a/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
+++ b/JornadaMilhas.Test.Integracao/OfertaViagemDALAdicionar.cs
@@ -5,7 +5,8 @@ using Xunit.Abstractions;
 
 namespace JornadaMilhas.Test.Integracao;
 
-public class OfertaViagemDALAdicionar: IClassFixture<ContextoFixture>
+[Collection(nameof(ContextoCollection))]
+public class OfertaViagemDALAdicionar
 {
     private readonly OfertaViagemDAL dal;
     private readonly ITestOutputHelper output;
```

 ## Added JornadaMilhas.Test.Integracao/OfertaViagemDALRecuperarPorId.cs
```diff --git a/JornadaMilhas.Test.Integracao/OfertaViagemDALRecuperarPorId.cs b/JornadaMilhas.Test.Integracao/OfertaViagemDALRecuperarPorId.cs
new file mode 100644
index 0000000..e2fd50e
--- /dev/null
+++ b/JornadaMilhas.Test.Integracao/OfertaViagemDALRecuperarPorId.cs
@@ -0,0 +1,38 @@
+﻿using JornadaMilhas.Dados;
+using System;
+using System.Collections.Generic;
+using System.Linq;
+using System.Text;
+using System.Threading.Tasks;
+using Xunit.Abstractions;
+
+namespace JornadaMilhas.Test.Integracao;
+[Collection(nameof(ContextoCollection))]
+public class OfertaViagemDALRecuperarPorId
+{
+    private readonly OfertaViagemDAL dal;
+    private readonly ITestOutputHelper output;
+
+    public OfertaViagemDALRecuperarPorId(ITestOutputHelper output, ContextoFixture fixture)
+    {
+        dal = new OfertaViagemDAL(fixture.Context);
+        this.output = output;
+
+
+        output.WriteLine(fixture.Context.GetHashCode().ToString());
+    }
+
+    [Fact]
+    public void RetornaNuloQuandoIdInexistente()
+    {
+        // Arrange
+        // Act
+        var ofertaRecuperada = dal.RecuperarPorId(-2);
+
+        // Assert
+
+        Assert.Null(ofertaRecuperada);
+
+
+    }
+}
```
