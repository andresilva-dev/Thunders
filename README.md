﻿# Introdução  

Bem-vindo ao teste técnico da Thunders! 🚀 

Estamos empolgados por você estar participando deste desafio e animados para conhecer melhor suas habilidades e seu potencial. Aproveite este momento para demonstrar sua criatividade, conhecimento técnico e capacidade de resolver problemas. 

Lembre-se: você não está sozinho nessa jornada! Caso tenha qualquer dúvida ou precise de suporte, sinta-se à vontade para entrar em contato com o nosso time. Estamos aqui para ajudar e garantir que você tenha a melhor experiência possível. 

Boa sorte e mãos à obra! Estamos ansiosos para ver o que você pode criar. 

# Requisitos Funcionais 

O governo anunciou a abertura de uma licitação para o desenvolvimento e implementação de um sistema informatizado voltado à geração de relatórios detalhados de faturamento das unidades de pedágio do país. Como vencedor dessa licitação, você será responsável por projetar e implementar uma solução eficiente e escalável, 
capaz de receber dados sobre as utilizações de cada unidade e consolidá-los em um relatório no formato especificado pelo edital. De acordo com informações do UOL, o Brasil conta com mais de 1.800 praças de pedágio distribuídas pelas 27 unidades federativas, o que evidencia a magnitude e a importância do projeto. Este software deverá não apenas atender aos requisitos técnicos, 
mas também ser capaz de lidar como grande volume de dados gerado diariamente, garantindo a precisão e a agilidade necessárias para a tomada de decisões administrativas e estratégicas. 

Os dados de utilização devem ser unitários e conter minimamente os atributos a seguir: 

- Data e hora de utilização 
- Praça 
- Cidade 
- Estado 
- Valor pago 
- Tipo de veículo (Moto, Carro ou Caminhão) 

 

Os relatórios a seguir foram solicitados: 

- Valor total por hora por cidade 
- As praças que mais faturaram por mês (a quantidade a ser processada deve ser configurável) 
- Quantos tipos de veículos passaram em uma determinada praça 


# Requisitos Técnicos 

 
A solução deve utilizar o template já estruturado disponível neste repositório, basta criar um fork ou clonar para começar.

- Toda implementação deve ser feita dentro do projeto ApiService encontrado no template. Recomenda-se não alterar o código dos outros projetos, porém, caso julgue necessário, alterações podem ser realizadas. 
- A solução deverá fornecer uma API para que as empresas dos pedágios possam enviar os dados.  
- O gatilho para processamento dos relatórios deve ser via API, simulando um agendamento. 
- Persistir os dados de utilização e os resultados dos relatórios. 
- O Timeout padrão é de 10 segundos e não pode ser alterado. 
- A solução utiliza .NET Aspire, então serviços externos como RabbitMQ, SQL Server e outros estão disponíveis de antemão. Para iniciar a aplicação basta manter o projeto AppHost como startup project. 
- Para facilitar o uso do broker a biblioteca Rebus está disponível, bastando apenas a criação de mensagens e seus respectivos “ouvintes”. 
- A implementação de testes para demonstrar o potencial da solução garantirá pontos extras. 
- A solução fornece suporte para OpenTelemetry 
- Considerar que milhões de registros serão ingeridos pela aplicação. 
- Os componentes existentes podem ser alterados, por exemplo SQL Server -> Postgres ou RabbitMQ -> Kafka. 
- Novos componentes podem ser agregados a solução, caso seja necessário.

 

Alguns componentes foram criados e disponibilizados para facilitar a implementação do teste: 

- Interface ‘IMessageSender’ do projeto OutOfBox: permite o envio de mensagens para o broker. 
- Features: para habilitar o uso de Mensageria ou Entity Framework através do padrão de configurações do .NET

# Sobre a Solução Implementada 🚧

Como solução do projeto, foram desenvolvidas as entidades **State**, **City**, **TollStation** e **RegisterUse**. Para estas entidades, um conjunto de APIs foi disponibilizado para permitir o **cadastro** e a **consulta** das informações.

Para a entidade `State`, foi disponibilizado apenas o endpoint para **listagem de todos os estados**, os quais são **inseridos automaticamente via migrations** ao iniciar a aplicação, contemplando os **27 estados da federação**.

Cada `RegisterUse` deve ser vinculado a uma `TollStation`, que por sua vez está associada a uma `City`, e esta, por fim, possui vínculo com um `State`.

---

## VehicleType 🚗🚛🏍️

Foi criado também um enumerador chamado `VehicleType`, com os seguintes valores:

- `0` – Moto
- `1` – Carro
- `2` – Caminhão

Este enumerador deve ser utilizado na criação de registros `RegisterUse`, os quais representam os **registros de utilização das praças de pedágio**.

---

## Endpoints para Processamento de Relatórios 📊

Para a geração dos relatórios, foram disponibilizados **endpoints específicos** que atuam como **gatilhos de processamento**. Cada requisição a um desses endpoints retorna um **ticket (Guid)** que identifica a solicitação feita. Com esse ticket, é possível consultar o resultado posteriormente.

Todos os endpoints estão **documentados e publicados via Swagger**, acessível em:

https://localhost:porta/

> Substitua `porta` pela porta onde a aplicação **ApiService** estiver sendo executada.

### Endpoints de Geração de Relatório

- `api/Reports/generate/total-hour-city`  
  > Gera relatório de valor total por hora por cidade

- `api/Reports/generate/top-stations-month`  
  > Gera relatório das praças que mais faturaram no mês

- `api/Reports/generate/vehicle-types-by-station`  
  > Gera relatório de tipos de veículos que passaram por uma praça

### Endpoint para Consulta de Resultado

- `api/Report/result`  
  > Consulta o resultado do relatório a partir do ticket gerado

