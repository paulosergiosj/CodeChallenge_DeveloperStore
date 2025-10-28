# 📚 Documentação - Ambev Developer Evaluation

Bem-vindo à documentação completa do sistema Ambev Developer Evaluation. Esta documentação fornece todas as informações necessárias para entender, configurar, executar e contribuir com o projeto.

## 📋 Índice da Documentação

### 🚀 Início Rápido
- **[README.md](../README.md)** - Documentação principal do projeto
  - Visão geral do sistema
  - Instruções de instalação e configuração
  - Como executar o projeto
  - Guia de testes
  - Documentação da API
  - Fluxos principais
  - Estrutura do projeto

### 🔐 Autenticação
- **[AUTHENTICATION_GUIDE.md](../AUTHENTICATION_GUIDE.md)** - Guia completo de autenticação JWT
  - Configuração de autenticação
  - Como usar tokens JWT
  - Exemplos práticos (cURL, JavaScript, Postman)
  - Usuários padrão
  - Cenários de teste
  - Debugging de problemas
  - Integração com frontend

### 🏗️ Documentação Técnica
- **[TECHNICAL_DOCUMENTATION.md](./TECHNICAL_DOCUMENTATION.md)** - Documentação técnica detalhada
  - Arquitetura detalhada (Clean Architecture)
  - Padrões implementados (CQRS, DDD, Event Sourcing)
  - Modelo de dados e entidades
  - Configurações técnicas (EF Core, MongoDB, Rebus)
  - Estratégia de testes
  - Segurança e performance

### 🛠️ Guia de Desenvolvimento
- **[DEVELOPMENT_GUIDE.md](./DEVELOPMENT_GUIDE.md)** - Guia para desenvolvedores
  - Padrões de código (Clean Code, SOLID)
  - Convenções de nomenclatura
  - Estrutura de commits
  - Boas práticas
  - Debugging e troubleshooting
  - FAQ para desenvolvedores

## 🎯 Para Diferentes Públicos

### 👨‍💻 Desenvolvedores
Comece com o **[README.md](../README.md)** para configuração básica, depois consulte:
- **[DEVELOPMENT_GUIDE.md](./DEVELOPMENT_GUIDE.md)** - Padrões e convenções
- **[TECHNICAL_DOCUMENTATION.md](./TECHNICAL_DOCUMENTATION.md)** - Arquitetura detalhada

### 🔧 DevOps/Infraestrutura
Foque em:
- **[README.md](../README.md)** - Seção de Deploy e Docker
- **[TECHNICAL_DOCUMENTATION.md](./TECHNICAL_DOCUMENTATION.md)** - Configurações de banco e monitoramento

### 🧪 QA/Testes
Consulte:
- **[README.md](../README.md)** - Seção de Testes
- **[TECHNICAL_DOCUMENTATION.md](./TECHNICAL_DOCUMENTATION.md)** - Estratégia de testes

### 🔐 Segurança
Foque em:
- **[AUTHENTICATION_GUIDE.md](../AUTHENTICATION_GUIDE.md)** - Implementação JWT
- **[TECHNICAL_DOCUMENTATION.md](./TECHNICAL_DOCUMENTATION.md)** - Configurações de segurança

## 🚀 Quick Start

### 1. Configuração Rápida
```bash
# Clone e configure
git clone <repository-url>
cd template/backend

# Iniciar bancos de dados
docker-compose up -d ambev.developerevaluation.database ambev.developerevaluation.nosql

# Restaurar dependências
dotnet restore

# Aplicar migrações
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi

# Executar aplicação
dotnet run --project src/Ambev.DeveloperEvaluation.WebApi
```

### 2. Testar Autenticação
```bash
# Obter token
curl -X POST "https://localhost:5001/api/auth" \
  -H "Content-Type: application/json" \
  -d '{"email": "admin@ambev.com", "password": "Admin123!"}'

# Usar token
curl -X GET "https://localhost:5001/api/products" \
  -H "Authorization: Bearer SEU_TOKEN"
```

### 3. Executar Testes
```bash
# Todos os testes
dotnet test

# Testes específicos
dotnet test tests/Ambev.DeveloperEvaluation.Unit
```

## 📊 Estatísticas do Projeto

- **Total de Testes**: 170+ testes
- **Cobertura**: >90% das funcionalidades principais
- **Handlers Testados**: 15+ handlers
- **Arquitetura**: Clean Architecture + DDD
- **Padrões**: CQRS, Event Sourcing, Repository, UoW
- **Tecnologias**: .NET 8, PostgreSQL, MongoDB, JWT

## 🔗 Links Úteis

- **Swagger UI**: `https://localhost:5001/swagger`
- **Health Check**: `https://localhost:5001/health`
- **JWT Debugger**: [jwt.io](https://jwt.io/)
- **ASP.NET Core Docs**: [docs.microsoft.com](https://docs.microsoft.com/en-us/aspnet/core/)

## 📞 Suporte

Para dúvidas ou problemas:

1. **Consulte a documentação** - Comece sempre pela documentação
2. **Verifique os testes** - Os testes são exemplos de uso
3. **Abra uma issue** - Para bugs ou melhorias
4. **Entre em contato** - Com a equipe de desenvolvimento

## 📝 Contribuindo

Para contribuir com o projeto:

1. Leia o **[DEVELOPMENT_GUIDE.md](./DEVELOPMENT_GUIDE.md)**
2. Siga os padrões estabelecidos
3. Adicione testes para novas funcionalidades
4. Atualize a documentação quando necessário
5. Submeta um Pull Request

---

**Documentação mantida e atualizada pela equipe de desenvolvimento 🚀**
