# 🔐 Guia de Autenticação JWT - Ambev Developer Evaluation

## ✅ Configuração Implementada

A autenticação JWT foi configurada com sucesso em todos os endpoints da API, exceto o endpoint de autenticação.

### **Endpoints Protegidos (Requerem Token JWT):**
- ✅ `/api/products/*` - Todos os endpoints de produtos
- ✅ `/api/users/*` - Todos os endpoints de usuários  
- ✅ `/api/carts/*` - Todos os endpoints de carrinho
- ✅ `/api/branches/*` - Todos os endpoints de filiais

### **Endpoints Públicos (Não requerem autenticação):**
- ✅ `/api/auth` - Endpoint de autenticação
- ✅ `/health` - Health check endpoint

## 🚀 Como Usar

### **1. Obter Token de Autenticação**

**Endpoint:** `POST /api/auth`

**Request:**
```json
{
  "email": "admin@ambev.com",
  "password": "Admin123!"
}
```

**Response:**
```json
{
  "success": true,
  "message": "User authenticated successfully",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "name": "Admin User",
    "email": "admin@ambev.com",
    "phone": "+5511999999999",
    "role": "Admin"
  }
}
```

### **2. Usar Token em Requisições**

**Header obrigatório:**
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### **3. Exemplo de Uso com cURL**

```bash
# 1. Autenticar e obter token
curl -X POST "https://localhost:5001/api/auth" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@ambev.com",
    "password": "Admin123!"
  }'

# 2. Usar token para acessar endpoint protegido
curl -X GET "https://localhost:5001/api/products" \
  -H "Authorization: Bearer SEU_TOKEN_AQUI"
```

### **4. Exemplo de Uso com JavaScript/Fetch**

```javascript
// 1. Autenticar
const authResponse = await fetch('/api/auth', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
  },
  body: JSON.stringify({
    email: 'admin@ambev.com',
    password: 'Admin123!'
  })
});

const authData = await authResponse.json();
const token = authData.data.token;

// 2. Usar token em outras requisições
const productsResponse = await fetch('/api/products', {
  headers: {
    'Authorization': `Bearer ${token}`
  }
});
```

### **5. Exemplo de Uso com Postman**

1. **Criar Request para Auth:**
   - Method: `POST`
   - URL: `https://localhost:5001/api/auth`
   - Body (raw JSON):
   ```json
   {
     "email": "admin@ambev.com",
     "password": "Admin123!"
   }
   ```

2. **Copiar Token da Response**

3. **Configurar Authorization:**
   - Type: `Bearer Token`
   - Token: `SEU_TOKEN_AQUI`

4. **Usar em outras requests**

## 🔧 Configuração Técnica

### **JWT Configuration:**
- **Algoritmo:** HMAC SHA256
- **Expiração:** 8 horas
- **Claims incluídos:** UserId, Username, Role, Email
- **Secret Key:** Configurada em `appsettings.json`

### **Swagger UI:**
- ✅ Configurado para suportar autenticação JWT
- ✅ Botão "Authorize" disponível no Swagger UI
- ✅ Inserir token no formato: `Bearer SEU_TOKEN`

### **Configuração no appsettings.json:**
```json
{
  "Jwt": {
    "SecretKey": "YourSuperSecretKeyForJwtTokenGenerationThatShouldBeAtLeast32BytesLong"
  }
}
```

## 🛡️ Segurança

### **Proteções Implementadas:**
- ✅ Todos os endpoints protegidos por padrão
- ✅ Endpoint de auth com `[AllowAnonymous]`
- ✅ Validação de token JWT em todas as requisições
- ✅ Verificação de usuário ativo
- ✅ Hash de senha seguro com BCrypt
- ✅ Validação de formato de senha (complexidade)

### **Headers de Segurança:**
- ✅ `Authorization: Bearer {token}` obrigatório
- ✅ Validação automática pelo ASP.NET Core
- ✅ Retorno 401 Unauthorized para tokens inválidos

### **Validação de Senha:**
- Mínimo 8 caracteres
- Pelo menos 1 letra maiúscula
- Pelo menos 1 letra minúscula
- Pelo menos 1 número
- Pelo menos 1 caractere especial

## 📝 Usuários Padrão

Após executar as migrações, os seguintes usuários estarão disponíveis:

### **Administrador:**
- **Email:** `admin@ambev.com`
- **Senha:** `Admin123!`
- **Role:** `Admin`
- **Status:** `Active`

### **Cliente:**
- **Email:** `customer@ambev.com`
- **Senha:** `Customer123!`
- **Role:** `Customer`
- **Status:** `Active`

## 🧪 Testando a Autenticação

### **Cenários de Teste:**

1. **✅ Acesso sem token:** Deve retornar 401 Unauthorized
2. **✅ Token inválido:** Deve retornar 401 Unauthorized  
3. **✅ Token válido:** Deve permitir acesso aos endpoints
4. **✅ Endpoint de auth:** Deve funcionar sem token
5. **✅ Token expirado:** Deve retornar 401 Unauthorized
6. **✅ Usuário inativo:** Deve retornar 401 Unauthorized
7. **✅ Senha incorreta:** Deve retornar 401 Unauthorized

### **Comandos de Teste:**

```bash
# Teste 1: Acesso sem token (deve falhar)
curl -X GET "https://localhost:5001/api/products"

# Teste 2: Acesso com token válido (deve funcionar)
curl -X GET "https://localhost:5001/api/products" \
  -H "Authorization: Bearer SEU_TOKEN_VALIDO"

# Teste 3: Autenticação (deve funcionar)
curl -X POST "https://localhost:5001/api/auth" \
  -H "Content-Type: application/json" \
  -d '{"email": "admin@ambev.com", "password": "Admin123!"}'

# Teste 4: Token inválido (deve falhar)
curl -X GET "https://localhost:5001/api/products" \
  -H "Authorization: Bearer token_invalido"

# Teste 5: Usuário inativo (deve falhar)
curl -X POST "https://localhost:5001/api/auth" \
  -H "Content-Type: application/json" \
  -d '{"email": "inactive@ambev.com", "password": "Inactive123!"}'
```

## 🔍 Debugging de Autenticação

### **Problemas Comuns:**

#### 1. Token Inválido
```bash
# Verificar se o token está correto
echo "SEU_TOKEN" | base64 -d
```

#### 2. Token Expirado
```bash
# Verificar expiração no jwt.io
# Ou gerar novo token
```

#### 3. Usuário Inativo
```sql
-- Verificar status do usuário no banco
SELECT * FROM "Users" WHERE "Email" = 'admin@ambev.com';
```

#### 4. Secret Key Incorreta
```json
// Verificar em appsettings.json
{
  "Jwt": {
    "SecretKey": "YourSuperSecretKeyForJwtTokenGenerationThatShouldBeAtLeast32BytesLong"
  }
}
```

## 📊 Monitoramento

### **Logs de Autenticação:**
- ✅ Tentativas de login bem-sucedidas
- ✅ Tentativas de login falhadas
- ✅ Tokens expirados
- ✅ Acessos negados

### **Métricas:**
- ✅ Número de autenticações por minuto
- ✅ Taxa de sucesso de autenticação
- ✅ Tempo de resposta do endpoint de auth

## 🚀 Integração com Frontend

### **React/Next.js:**
```javascript
// Hook para autenticação
const useAuth = () => {
  const [token, setToken] = useState(localStorage.getItem('token'));
  
  const login = async (email, password) => {
    const response = await fetch('/api/auth', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password })
    });
    
    const data = await response.json();
    if (data.success) {
      setToken(data.data.token);
      localStorage.setItem('token', data.data.token);
    }
  };
  
  const logout = () => {
    setToken(null);
    localStorage.removeItem('token');
  };
  
  return { token, login, logout };
};
```

### **Angular:**
```typescript
// Service de autenticação
@Injectable()
export class AuthService {
  private token: string | null = null;
  
  async login(email: string, password: string): Promise<boolean> {
    const response = await this.http.post('/api/auth', { email, password }).toPromise();
    
    if (response.success) {
      this.token = response.data.token;
      localStorage.setItem('token', this.token);
      return true;
    }
    
    return false;
  }
  
  getToken(): string | null {
    return this.token || localStorage.getItem('token');
  }
}
```

## 📚 Recursos Adicionais

- [JWT.io](https://jwt.io/) - Debugger de tokens JWT
- [ASP.NET Core Authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/)
- [JWT Best Practices](https://tools.ietf.org/html/rfc8725)

## 📞 Suporte

Para problemas de autenticação:

1. Verifique os logs da aplicação
2. Teste com usuários padrão
3. Verifique configuração do JWT
4. Consulte a documentação técnica
5. Entre em contato com a equipe de desenvolvimento

---

**Configuração de autenticação JWT implementada com sucesso! 🔐**
