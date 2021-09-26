# Turma-01-Desafio-4-Equipe-01 - Projeto Logs

O objetivo deste desafio é criar uma função do Azure (Azure Function) para receber, através de uma requisição HTTP POST, informações de log de uma aplicação que deverão ser armazenadas em um banco de dados que posteriormente foi utilizado para criação de um dashboard na implementação de monitoramento via stack ELK.
Na segunda parte do desafio, criamos uma função de TimeTrigger e utilizamos o Kafka para ler um topico da aplicação do desafio 3, esse topico tem informações sobre alteração do cadastro do cliente. Após a leitura desse topico armazenaremos essas informações com Redis em cache para manter atualizada a informação de alteração de cadastro do cliente. 

## Tecnologias usadas

- C#
- Azure Functions
- Elastic Search
- Kibana
- Kafka
- Serilog
- SonarCube
- Horusec

## License
[MIT](https://choosealicense.com/licenses/mit/)

[![Quality Gate Status](http://18.230.61.125:9000/api/project_badges/measure?project=Logs&metric=alert_status)](http://18.230.61.125:9000/dashboard?id=Logs)
