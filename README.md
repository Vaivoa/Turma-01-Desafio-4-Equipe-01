![C# Badge]( https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Redis](https://img.shields.io/badge/redis-%23DD0031.svg?&style=for-the-badge&logo=redis&logoColor=white)
![Kafka](https://img.shields.io/badge/Apache_Kafka-231F20?style=for-the-badge&logo=apache-kafka&logoColor=white)
![Azure](https://img.shields.io/badge/microsoft%20azure-0089D6?style=for-the-badge&logo=microsoft-azure&logoColor=white)
![Kibana](https://img.shields.io/badge/Kibana-005571?style=for-the-badge&logo=Kibana&logoColor=white)
![GitActions](https://img.shields.io/badge/GitHub_Actions-2088FF?style=for-the-badge&logo=github-actions&logoColor=white)
![AzureFunctions](https://img.shields.io/badge/Azure_Functions-0062AD?style=for-the-badge&logo=azure-functions&logoColor=white)

## Pipeline Status
[![code-analizer-logs](https://github.com/Vaivoa/Turma-01-Desafio-4-Equipe-01/actions/workflows/analizer.yml/badge.svg)](https://github.com/Vaivoa/Turma-01-Desafio-4-Equipe-01/actions/workflows/analizer.yml)
[![code-analizer-kafka](https://github.com/Vaivoa/Turma-01-Desafio-4-Equipe-01/actions/workflows/analizerkafka.yml/badge.svg)](https://github.com/Vaivoa/Turma-01-Desafio-4-Equipe-01/actions/workflows/analizerkafka.yml)
[![Deploy DotNet project to Azure Function App](https://github.com/Vaivoa/Turma-01-Desafio-4-Equipe-01/actions/workflows/deploy.yml/badge.svg)](https://github.com/Vaivoa/Turma-01-Desafio-4-Equipe-01/actions/workflows/deploy.yml)

# Turma-01-Desafio-4-Equipe-01 - Projeto Logs

O objetivo deste desafio é criar uma função do Azure (Azure Function) para receber, através de uma requisição HTTP POST, informações de log de uma aplicação que deverão ser armazenadas em um banco de dados que posteriormente foi utilizado para criação de um dashboard na implementação de monitoramento via stack ELK.
Na segunda parte do desafio, criamos uma função de TimeTrigger e utilizamos o Kafka para ler um topico da aplicação do desafio 3, esse topico tem informações sobre alteração do cadastro do cliente. Após a leitura desse topico armazenaremos essas informações com Redis em cache para manter atualizada a informação de alteração de cadastro do cliente. 

# Fluxo

![Desafio 4@1 100000023841858x](https://user-images.githubusercontent.com/63682265/138184949-a0bc0a15-3176-4d44-ad84-ea83ea61f946.png)

## Tecnologias usadas
- C#
- Azure Functions
- Elastic Search
- Kibana
- Kafka
- Serilog
- SonarQube
- Horusec
- Redis
- xUnit

## Configurações
- [Configurar CI no GitActions](#configurar-ci-no-gitactions)
- [Configurar Azure](#configurar-azure)
- [Configurar Banco de Dados](#configurar-banco-de-dados)
- [Configurar SonarQube](#configurar-sonarqube)
- [Configurar Horusec](#configurar-horusec)

## Configurar Ci no GitActions
No portal do Git Hub, na aba ``` Actions ``` crie um ``` New Workflow ``` e escolha a opção ``` set it up your self ``` e crie uma pasta workflow para armazenar os arquivos .yml ``` Nome-Do-Projeto/.github/workflows/deploy.yml ``` e posteriormente os arquivos de analise do Horusec e Sonarqube de cada projeto, ``` Nome-Do-Projeto/.github/workflows/code-analizer-logs.yml ```.


Para a pipeline funcionar, é necessário adicionar algumas variaveis de ambiente.
Em ``` Settings ``` na aba ``` Secrets ``` adicione as variaveis.


``` AZURE_FUNCTIONAPP_PUBLISH_PROFILE ``` - O conteúdo dessa variável é encontrado dentro do portal do Azure dentro do ``` Aplicativo de Função ``` que você criou para esse projeto, clicando na opção ![image](https://user-images.githubusercontent.com/63682265/135248283-f2cc0777-142a-42d4-aad7-306591927611.png) você vai fazer download de um arquivo ``` .PublishSettings ```, abre em um editor e copie seu conteúdo para criar a variavel de ambiente.


``` AZURE_FUNCTIONAPP_NAME: logs-vaivoa-turma1 ``` - Nome da função criada no Portal do Azure


``` AZURE_FUNCTIONAPP_PACKAGE_PATH: src/LogsVaivoa ``` - Caminho da função no diretorio do projeto


## Deploy no Azure
``` name: Deploy DotNet project to Azure Function App

on:
  push:
    branches: [ main ]

env:
  AZURE_FUNCTIONAPP_NAME: logs-vaivoa-turma1  # set this to your application's name
  AZURE_FUNCTIONAPP_PACKAGE_PATH: src/LogsVaivoa    # set this to the path to your web app project, defaults to the repository root
  DOTNET_VERSION: '3.x'              # set this to the dotnet version to use
  #ROOT_SOLUTION_PATH: src

jobs:
  build-and-deploy:
    runs-on: windows-latest
    environment: dev
    steps:
    - name: 'Checkout GitHub Action'
      uses: actions/checkout@master

    - name: Setup DotNet ${{ env.DOTNET_VERSION }} Environment
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: ${{ env.DOTNET_VERSION }}

    - name: 'Resolve Project Dependencies Using Dotnet'
      shell: pwsh
      run: |
        pushd './${{ env.AZURE_FUNCTIONAPP_PACKAGE_PATH }}'
        dotnet build --configuration Release --output ./output
        popd
    - name: 'Run Azure Functions Action'
      uses: Azure/functions-action@v1
      id: fa
      with:
        app-name: ${{ env.AZURE_FUNCTIONAPP_NAME }}
        package: '${{ env.AZURE_FUNCTIONAPP_PACKAGE_PATH }}/output'
        publish-profile: ${{ secrets.AZURE_FUNCTIONAPP_PUBLISH_PROFILE }}  
        
```

## Análise de Código 

```
name: code-analizer-logs


on:
  push:
    branches: [ main, develop ]

 
  workflow_dispatch:

env:
  AZURE_FUNCTIONAPP_NAME: logs-vaivoa-turma1  
  AZURE_FUNCTIONAPP_PACKAGE_PATH: src/LogsVaivoa    
  DOTNET_VERSION: '3.x'   


jobs:
  horusec-security:
      name: horusec-security
      runs-on: ubuntu-latest
      steps:
      - name: Check out code
        uses: actions/checkout@v2
      - name: Running Horusec Security
        run: |
          curl -fsSL https://raw.githubusercontent.com/ZupIT/horusec/main/deployments/scripts/install.sh | bash -s latest
          horusec start -p="./src/" -e="true"   
  sonarqube-analizer:
    needs: [horusec-security]
    runs-on: windows-latest
    environment: dev
    steps:
    - name: 'Checkout GitHub Action'
      uses: actions/checkout@master

    - name: Setup DotNet ${{ env.DOTNET_VERSION }} Environment
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: ${{ env.DOTNET_VERSION }}
    - name: Setup DotNet 5.0 Environment
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: '5.0.x'
    - name: Setup sonarqube 
      shell: pwsh
      run: |
        dotnet tool install --global dotnet-sonarscanner
        dotnet tool install --global coverlet.console 
    - name: Run Sonarqube scanner
      shell: pwsh
      run: |
        cd "./src/LogsVaivoa"
        dotnet sonarscanner begin /k:"Logs" /d:sonar.cs.opencover.reportsPaths="opencover.xml" /d:sonar.host.url=${{secrets.SONAR_HOST}} /d:sonar.qualitygate.wait=true /d:sonar.login=${{secrets.SONAR_TOKEN}}
        dotnet build
        dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover  /p:CoverletOutput=coverage /p:Exclude=[xunit.*]* ./LogsVaivoa.sln
        move ..\..\test\Logs.Test\coverage.opencover.xml opencover.xml -force
        dotnet sonarscanner end /d:sonar.login=${{secrets.SONAR_TOKEN}}
        
```

Variaveis de Ambiente
- SONAR_HOST - Essa variável é a URL.
- SONAR_TOKEN - Quando você criar o projeto no SonarQube, será gerado um token, esse token é essa variavel de ambiente.

## Configurar Azure

- Criar uma conta no portal da ``` Azure ```
- Criar um ``` Grupo de Recursos ``` ![image](https://user-images.githubusercontent.com/63682265/135261925-76177c51-0c9b-4b61-bdf0-994ddde35570.png) Apartir de agora, todas as funções criadas dentro do azure deve ser incluido dentro deste grupo de recurso que neste projeto nomeamos ``` Desafio4-Turma1-FM ```
- Criar um ``` Aplicativo Função ``` no portal do ``` Azure ```. ![image](https://user-images.githubusercontent.com/63682265/135262083-19dfd2e9-b14e-43be-b927-580b4bc6ac5d.png) Nesse projeto chamamos de ``` logs-vaivoa-turma1 ```.


## Configurar Banco de Dados
- Criar um ``` Banco de Dados SQL ``` ![image](https://user-images.githubusercontent.com/63682265/135261822-cc10b974-35bf-466d-b434-9a1484c1136d.png) - Nesse projeto nomeamos o banco de dados como ``` DbLog ```
- Para criar uma tabela nesse banco de dados é necessário acessar o Visual Studio, e quando a função já estiver criada, vá em ![image](https://user-images.githubusercontent.com/63682265/135263582-c91133d0-bdfe-430f-b279-20750d3a8e85.png) ``` SQL SERVER EXPLORER ```, clique com o botão direito em ``` SQL SERVER ```, clique em ``` ADD SERVER ```, vá na opção ``` Azure ```, se conecte a sua conta e escolha o servidor que foi criado ``` DbLog   ```.
- Acesse o ``` DbLog ```, e clique com botão direito em ``` Tables ``` e selecione a opção ``` Add new Table ```. - Nomeamos a tabela ``` Logs ``` ![image](https://user-images.githubusercontent.com/63682265/135267534-22ef82a8-3509-412e-aced-e3249db2df0a.png)

## Configurar Horusec

Para configurar o ``` Horusec ``` na nossa pipeline, é necessário apenas colar o código abaixo no ``` jobs ``` do arquivo .yml.

``` 
jobs:
  horusec-security:
    name: horusec-security
    runs-on: ubuntu-latest
    steps:
    - name: Check out code
      uses: actions/checkout@v2
    - name: Running Horusec Security
      run: |
        curl -fsSL https://raw.githubusercontent.com/ZupIT/horusec/main/deployments/scripts/install.sh | bash -s latest
        horusec start -p="./" -e="true" 

```

## Configurar SonarQube 

Para configurar o ``` SonarQube ``` na nossa pipeline, é necessário subir o SonarQube em uma marquina virtual. E para esse projeto decidimos simplificar, e usamos a marquina virtual ``` AWS EC2 ``` e usamos um orquestrador de containers, o ``` Caphover ```. Dessa forma usamos o ``` SonarQube ``` dentro de um container e não precisamos subir ele e mais banco de dados na VM, economizando custos para esse projeto. 

Para instalar o ``` CapHover ``` é só abrir o console do AWS e colar o código abaixo.

```
sudo bash
sudo amazon-linux-extras install docker
sudo service docker start
sudo systemctl enable docker
sudo usermod -a -G docker ec2-user
sudo docker run -p 80:80 -p 443:443 -p 3000:3000 -v /var/run/docker.sock:/var/run/docker.sock -v /captain:/captain caprover/caprover
```
Ao abrir o ``` CapHover ``` você vai procurar pelo SonarQube e seguir o passo a passo para subir o container e configura-lo.

## Elastic Cloud

Para esse projeto usamos o Elastic Cloud. 

- Crie uma conta no ElasticCloud
- Pegar a ElasticSearch endpoint url - No projeto chamamos de ElkConnection
- Criar uma Interface e um Serviço para conexão com Elastic Service

## Criar um Deployment

No site do Elastic Cloud, crie um deployment, salve as credenciais.

Todos os endpoints que você venha a precisar estaram no dashboard inicial do seu Deployment. No projeto usamos o endpoint do ``` Elasticsearch ```

![image](https://user-images.githubusercontent.com/63682265/138185585-b01e3eb1-b102-4550-bb42-3856f7bdd888.png)

É possivel também trabalhar com elastic search local, usando o docker.

# Interface

```
public interface IElasticsearchService
    {
        public Task<bool> SendToElastic<T>(T log, string index) where T : class;
    }
```


# Elastic Search Service
```
public class ElasticsearchService : IElasticsearchService
    {
        private ElasticClient _elasticClient;

        public ElasticsearchService()
        {
            var uriElastic = new Uri(Environment.GetEnvironmentVariable("ElkConnection")!);
            var elasticsearchSettings = new ConnectionSettings(uriElastic);
            _elasticClient = new ElasticClient(elasticsearchSettings);
        }

        public async Task<bool> SendToElastic<T>(T log, string index) where T : class
        {
            var resultElastic = await _elasticClient
                .IndexAsync(log, idx => idx.Index(index));
            
            return resultElastic.IsValid;
        }
        
        
    }
```

## Log Service

```
public class LogService : ILogService
    {
        private static readonly string IndexLog = Environment.GetEnvironmentVariable("IndexLog");
        private readonly IElasticsearchService _elasticService;
        private readonly ILogger<LogService> _logger;
        private readonly ILogRepository _logRepository;
        public LogService(IElasticsearchService elasticService, ILogger<LogService> logger, IDbContext dbContext, ILogRepository logRepository)
        {
            _elasticService = elasticService;
            _logger = logger;
            _logRepository = logRepository;
        }

        public async Task<(bool, object)> PostLog(Log log)
        {
            if (!log.IsValid()) return (false, log.GetErrors());

            await _elasticService.SendToElastic(log, IndexLog);

            await _logRepository.InsertLogDb(log);

            return (true, log);
        }
    }

```



## Contribuidores
[![Linkedin Badge](	https://img.shields.io/badge/Victor%20Magdesian-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/victor-felippe-magdesian-7a45051a7/)
[![Linkedin Badge](	https://img.shields.io/badge/Matheus%20Paixao-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/matheuspaixao/)
[![Linkedin Badge]( https://img.shields.io/badge/Yuri%20Dias-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/yuri-dias/)
[![Linkedin Badge]( https://img.shields.io/badge/Leandro%20Gomes-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/leandro-gomes/)
[![Linkedin Badge]( https://img.shields.io/badge/Anderson%20Silva-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/anderson-marques-da-silva-62b3b1136/)
[![Linkedin Badge]( https://img.shields.io/badge/Breno%20Fortunato-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/breno-silva-fortunato/)
