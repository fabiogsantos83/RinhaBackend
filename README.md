# Rinha de Backends
Este projeto foi criado a partir de uma "competição" proposta pelo [zanfranceschi](https://github.com/zanfranceschi) em 2023 chamada Rinha de Backend!

Fiquei sabendo dessa "rinha" só depois que a competição já tinha sido encerrada mas resolvi desenvolver minha solução para aprendizado e estudos.

Consegui atingir os resultados sem nenhuma perda de requests e inserções com um projeto simples apenas administrando os recursos disponiveis nas regras da rinha e criando indice Full Text Search no postgres que era o gargalo. 

Para os testes de carga, foi utilizado a ferramenta [Gatling](https://gatling.io/) com script desenvolvido pelo zanfranceschi [neste repositorio](https://github.com/zanfranceschi/rinha-de-backend-2023-q3/tree/main/stress-test)

Segue [aqui](https://github.com/zanfranceschi/rinha-de-backend-2023-q3/blob/main/INSTRUCOES.md) as instruções da rinha.

Segue [aqui](https://github.com/zanfranceschi/rinha-de-backend-2023-q3) o repositório da rinha com as instruções, o código dos participantes e os resultados.

## Instruções para rodar
Na pasta raiz do projeto rodar o comando abaixo para subir as duas instancias aplicação, o nginx e o postegres em containers do docker: 

**docker-compose up -d**

O comando abaixo remove todos os containers com suas respectivas imagens: 

**docker-compose down --rmi all -v**

## Tecnologias

* Dot Net 7
* Nginx
* Postegres
* Docker

## Reultado

![This is an alt text.](/resultado.png "This is a sample image.")