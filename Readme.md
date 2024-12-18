# API MongoDB com ASP.NET Core

Este projeto é uma API desenvolvida em ASP.NET Core integrada com um banco de dados MongoDB. Ele demonstra como configurar uma aplicação utilizando contêineres Docker e interagir com MongoDB para manipular coleções e documentos.

---

## **Tecnologias Utilizadas**
- **ASP.NET Core 8.0**
- **MongoDB**
- **Docker e Docker Compose**

---

## **Configuração do Ambiente**

### **Pré-requisitos**
1. [Docker](https://www.docker.com/get-started)
2. [MongoDB Compass](https://www.mongodb.com/products/compass) (opcional, para visualizar o banco de dados)

---

## **Como Executar o Projeto**

### **1. Clonar o Repositório**
```bash
git clone https://github.com/janderson-code/ApiMongoDb.git
cd ApiMongoDb
```

### **2. Executar com Docker Compose**
```bash
docker-compose up --build
```
Isso iniciará:
- Um contêiner para a API em ASP.NET Core (porta 5000).
- Um contêiner para o MongoDB (porta 27017).

### **3. Testar Api**
```bash
http://localhost:5000/api/books
```

### **Container MongoDB**

- Entrar no MongoDB
```bash
docker exec -it mongodb bash
```
- Acessar o Mongo Shell
```bash
   mongosh
```
- Visualizar documentos
```bash
    db.Books.find().pretty()
 ```
