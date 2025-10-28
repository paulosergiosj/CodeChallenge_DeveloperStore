# 🛠️ Guia de Desenvolvimento - Ambev Developer Evaluation

## 📋 Índice

- [Padrões de Código](#-padrões-de-código)
- [Estrutura de Commits](#-estrutura-de-commits)
- [Convenções de Nomenclatura](#-convenções-de-nomenclatura)
- [Boas Práticas](#-boas-práticas)
- [Debugging](#-debugging)
- [Troubleshooting](#-troubleshooting)
- [FAQ](#-faq)

## 📝 Padrões de Código

### Clean Code Principles

#### 1. Nomes Descritivos
```csharp
// ❌ Ruim
public void ProcessData(string d, int q, decimal p) { }

// ✅ Bom
public void ProcessOrderData(string description, int quantity, decimal price) { }
```

#### 2. Funções Pequenas e Focadas
```csharp
// ❌ Ruim
public async Task<CreateUserResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
{
    // Validação
    var validator = new CreateUserValidator();
    var validationResult = await validator.ValidateAsync(request, cancellationToken);
    if (!validationResult.IsValid)
        throw new ValidationException(validationResult.Errors);
    
    // Verificar se usuário existe
    var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
    if (existingUser != null)
        throw new InvalidOperationException("User with this email already exists");
    
    // Hash da senha
    var hashedPassword = _passwordHasher.HashPassword(request.Password);
    
    // Criar usuário
    var user = User.Create(request.Username, request.Email, request.Phone, hashedPassword, request.Role);
    
    // Salvar
    await _userRepository.AddAsync(user, cancellationToken);
    await _unitOfWork.CommitAsync(cancellationToken);
    
    // Mapear resultado
    var result = _mapper.Map<CreateUserResult>(user);
    return result;
}

// ✅ Bom
public async Task<CreateUserResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
{
    await ValidateRequest(request, cancellationToken);
    await EnsureUserDoesNotExist(request.Email, cancellationToken);
    
    var user = await CreateUser(request);
    await SaveUser(user, cancellationToken);
    
    return MapToResult(user);
}

private async Task ValidateRequest(CreateUserCommand request, CancellationToken cancellationToken)
{
    var validator = new CreateUserValidator();
    var validationResult = await validator.ValidateAsync(request, cancellationToken);
    if (!validationResult.IsValid)
        throw new ValidationException(validationResult.Errors);
}

private async Task EnsureUserDoesNotExist(string email, CancellationToken cancellationToken)
{
    var existingUser = await _userRepository.GetByEmailAsync(email, cancellationToken);
    if (existingUser != null)
        throw new InvalidOperationException("User with this email already exists");
}

private async Task<User> CreateUser(CreateUserCommand request)
{
    var hashedPassword = _passwordHasher.HashPassword(request.Password);
    return User.Create(request.Username, request.Email, request.Phone, hashedPassword, request.Role);
}

private async Task SaveUser(User user, CancellationToken cancellationToken)
{
    await _userRepository.AddAsync(user, cancellationToken);
    await _unitOfWork.CommitAsync(cancellationToken);
}

private CreateUserResult MapToResult(User user)
{
    return _mapper.Map<CreateUserResult>(user);
}
```

#### 3. Evitar Comentários Desnecessários
```csharp
// ❌ Ruim
// Incrementa o contador
counter++;

// ✅ Bom
// O código deve ser autoexplicativo
counter++;
```

### SOLID Principles

#### 1. Single Responsibility Principle (SRP)
```csharp
// ❌ Ruim - Classe com múltiplas responsabilidades
public class UserService
{
    public void CreateUser(User user) { }
    public void SendEmail(string email) { }
    public void LogActivity(string activity) { }
}

// ✅ Bom - Responsabilidades separadas
public class UserService
{
    public void CreateUser(User user) { }
}

public class EmailService
{
    public void SendEmail(string email) { }
}

public class LoggingService
{
    public void LogActivity(string activity) { }
}
```

#### 2. Open/Closed Principle (OCP)
```csharp
// ✅ Bom - Extensível sem modificação
public abstract class DiscountCalculator
{
    public abstract decimal CalculateDiscount(decimal amount);
}

public class PercentageDiscountCalculator : DiscountCalculator
{
    public override decimal CalculateDiscount(decimal amount)
    {
        return amount * 0.1m; // 10% discount
    }
}

public class FixedAmountDiscountCalculator : DiscountCalculator
{
    public override decimal CalculateDiscount(decimal amount)
    {
        return Math.Min(amount, 50m); // Max $50 discount
    }
}
```

#### 3. Dependency Inversion Principle (DIP)
```csharp
// ❌ Ruim - Dependência de implementação concreta
public class OrderService
{
    private readonly SqlOrderRepository _orderRepository;
    
    public OrderService()
    {
        _orderRepository = new SqlOrderRepository();
    }
}

// ✅ Bom - Dependência de abstração
public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    
    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
}
```

## 📝 Estrutura de Commits

### Padrão de Mensagens

```
<type>(<scope>): <description>

[optional body]

[optional footer(s)]
```

### Tipos de Commit

- **feat**: Nova funcionalidade
- **fix**: Correção de bug
- **docs**: Documentação
- **style**: Formatação, ponto e vírgula, etc.
- **refactor**: Refatoração de código
- **test**: Adição ou correção de testes
- **chore**: Tarefas de manutenção

### Exemplos

```bash
# Nova funcionalidade
git commit -m "feat(products): add product category filtering"

# Correção de bug
git commit -m "fix(cart): resolve cart total calculation issue"

# Documentação
git commit -m "docs(api): update authentication guide"

# Refatoração
git commit -m "refactor(domain): extract user validation logic"

# Testes
git commit -m "test(handlers): add unit tests for CreateUserHandler"

# Manutenção
git commit -m "chore(deps): update Entity Framework to 8.0.10"
```

### Commits com Corpo

```bash
git commit -m "feat(orders): implement order creation from cart

- Add CartCheckedOutEventHandler
- Implement order validation logic
- Add branch selection for orders
- Update cart status to finalized

Closes #123"
```

## 🏷️ Convenções de Nomenclatura

### Classes e Interfaces

```csharp
// Classes - PascalCase
public class UserService { }
public class ProductRepository { }

// Interfaces - PascalCase com I prefix
public interface IUserService { }
public interface IProductRepository { }

// Enums - PascalCase
public enum UserStatus { Active, Inactive, Suspended }
public enum OrderStatus { Pending, Confirmed, Cancelled }
```

### Métodos e Propriedades

```csharp
// Métodos - PascalCase
public void CreateUser() { }
public async Task<User> GetUserByIdAsync(Guid id) { }

// Propriedades - PascalCase
public string Username { get; set; }
public DateTime CreatedAt { get; private set; }

// Campos privados - camelCase com _ prefix
private readonly IUserRepository _userRepository;
private readonly IMapper _mapper;
```

### Variáveis e Parâmetros

```csharp
// Variáveis locais - camelCase
var userName = "john.doe";
var userCount = 0;
var isValid = true;

// Parâmetros - camelCase
public void ProcessUser(string userName, int userCount, bool isValid) { }
```

### Constantes

```csharp
// Constantes - PascalCase
public const int MaxRetryAttempts = 3;
public const string DefaultConnectionString = "Server=localhost;Database=DefaultDb;";

// Enum values - PascalCase
public enum UserRole
{
    Customer,
    Admin,
    Moderator
}
```

## ✅ Boas Práticas

### 1. Async/Await

```csharp
// ✅ Bom - Async em toda a cadeia
public async Task<User> GetUserAsync(Guid id)
{
    return await _userRepository.GetByIdAsync(id);
}

// ❌ Ruim - Mistura sync/async
public User GetUser(Guid id)
{
    return _userRepository.GetByIdAsync(id).Result; // Deadlock risk
}
```

### 2. Error Handling

```csharp
// ✅ Bom - Exceptions específicas
public async Task<Product> GetProductAsync(int productNumber)
{
    var product = await _productRepository.GetByProductNumberAsync(productNumber);
    if (product == null)
        throw new KeyNotFoundException($"Product with number {productNumber} not found");
    
    return product;
}

// ❌ Ruim - Exceptions genéricas
public async Task<Product> GetProductAsync(int productNumber)
{
    try
    {
        return await _productRepository.GetByProductNumberAsync(productNumber);
    }
    catch (Exception ex)
    {
        throw new Exception("Something went wrong"); // Perde informação importante
    }
}
```

### 3. Validation

```csharp
// ✅ Bom - Validação no início do método
public async Task<CreateUserResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
{
    // Validar entrada primeiro
    var validator = new CreateUserValidator();
    var validationResult = await validator.ValidateAsync(request, cancellationToken);
    if (!validationResult.IsValid)
        throw new ValidationException(validationResult.Errors);
    
    // Lógica de negócio
    // ...
}

// ❌ Ruim - Validação espalhada
public async Task<CreateUserResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
{
    // Lógica de negócio
    if (string.IsNullOrEmpty(request.Email))
        throw new ArgumentException("Email is required");
    
    // Mais lógica
    if (request.Password.Length < 8)
        throw new ArgumentException("Password too short");
    
    // Mais lógica...
}
```

### 4. Dependency Injection

```csharp
// ✅ Bom - Constructor injection
public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    
    public UserService(IUserRepository userRepository, IEmailService emailService)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    }
}

// ❌ Ruim - Service locator pattern
public class UserService
{
    public void CreateUser(User user)
    {
        var userRepository = ServiceLocator.GetService<IUserRepository>();
        var emailService = ServiceLocator.GetService<IEmailService>();
        // ...
    }
}
```

### 5. Configuration

```csharp
// ✅ Bom - Strongly typed configuration
public class DatabaseSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public int CommandTimeout { get; set; } = 30;
    public bool EnableRetryOnFailure { get; set; } = true;
}

// Registro no DI
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("Database"));

// Uso
public class UserRepository
{
    private readonly DatabaseSettings _settings;
    
    public UserRepository(IOptions<DatabaseSettings> settings)
    {
        _settings = settings.Value;
    }
}
```

## 🐛 Debugging

### 1. Logging Estruturado

```csharp
// ✅ Bom - Logging estruturado
public async Task<User> CreateUserAsync(CreateUserCommand command)
{
    _logger.LogInformation("Creating user with email {Email}", command.Email);
    
    try
    {
        var user = User.Create(command.Username, command.Email, command.Phone, hashedPassword, command.Role);
        await _userRepository.AddAsync(user);
        
        _logger.LogInformation("User created successfully with ID {UserId}", user.Id);
        return user;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed to create user with email {Email}", command.Email);
        throw;
    }
}

// ❌ Ruim - Logging não estruturado
public async Task<User> CreateUserAsync(CreateUserCommand command)
{
    _logger.LogInformation($"Creating user with email {command.Email}");
    // ...
}
```

### 2. Breakpoints Estratégicos

```csharp
public async Task<CreateUserResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
{
    // Breakpoint aqui para verificar entrada
    await ValidateRequest(request, cancellationToken);
    
    // Breakpoint aqui para verificar validação
    await EnsureUserDoesNotExist(request.Email, cancellationToken);
    
    // Breakpoint aqui para verificar criação
    var user = await CreateUser(request);
    
    // Breakpoint aqui para verificar salvamento
    await SaveUser(user, cancellationToken);
    
    // Breakpoint aqui para verificar resultado
    return MapToResult(user);
}
```

### 3. Debugging com Visual Studio

1. **Immediate Window**: Para executar código durante debug
2. **Watch Window**: Para monitorar variáveis específicas
3. **Call Stack**: Para entender o fluxo de execução
4. **Exception Settings**: Para configurar quais exceções quebrar

## 🔧 Troubleshooting

### Problemas Comuns

#### 1. Migration Issues

```bash
# Problema: Migration não aplicada
# Solução: Verificar connection string e aplicar manualmente
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi

# Problema: Migration conflitante
# Solução: Resetar migrations
dotnet ef database drop --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi
```

#### 2. Dependency Injection Issues

```csharp
// Problema: Service não registrado
// Solução: Verificar registro no ModuleInitializer
public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
    }
}
```

#### 3. MongoDB Connection Issues

```csharp
// Problema: MongoDB não conecta
// Solução: Verificar connection string e status do serviço
// Connection string: mongodb://localhost:27017
// Verificar se MongoDB está rodando: docker ps
```

#### 4. JWT Token Issues

```csharp
// Problema: Token inválido
// Solução: Verificar secret key e expiração
// Secret key deve ter pelo menos 32 caracteres
// Token expira em 8 horas por padrão
```

### Performance Issues

#### 1. Slow Queries

```csharp
// Problema: Query lenta
// Solução: Adicionar índices e otimizar queries
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Adicionar índice para busca por categoria
        builder.HasIndex(p => p.Category);
        
        // Adicionar índice composto
        builder.HasIndex(p => new { p.Category, p.Price });
    }
}
```

#### 2. Memory Leaks

```csharp
// Problema: Memory leak
// Solução: Implementar IDisposable corretamente
public class UserService : IDisposable
{
    private bool _disposed = false;
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            // Liberar recursos
        }
        _disposed = true;
    }
}
```

## ❓ FAQ

### Q: Como adicionar uma nova funcionalidade?

A: Siga estes passos:
1. Criar entidade no Domain layer
2. Criar repository interface no Domain
3. Implementar repository no Infrastructure
4. Criar command/query no Application
5. Criar handler no Application
6. Criar controller no WebApi
7. Adicionar testes unitários
8. Criar migration se necessário

### Q: Como debugar problemas de autenticação?

A: Verifique:
1. Secret key do JWT está configurada
2. Token não expirou (8 horas)
3. Usuário está ativo
4. Header Authorization está correto
5. Claims do token estão corretos

### Q: Como resolver problemas de conexão com banco?

A: Verifique:
1. Connection string está correta
2. Banco está rodando (PostgreSQL/MongoDB)
3. Credenciais estão corretas
4. Porta está acessível
5. Firewall não está bloqueando

### Q: Como adicionar novos testes?

A: Siga o padrão:
1. Criar TestData class para dados de teste
2. Criar HandlerTests class
3. Usar Given-When-Then pattern
4. Mockar dependências com NSubstitute
5. Usar FluentAssertions para asserções

### Q: Como fazer deploy em produção?

A: Use Docker:
1. Build da imagem: `docker build -t app .`
2. Configurar variáveis de ambiente
3. Usar docker-compose para orquestração
4. Configurar health checks
5. Monitorar logs e métricas

---

Este guia fornece as diretrizes e boas práticas para desenvolvimento no projeto Ambev Developer Evaluation.
