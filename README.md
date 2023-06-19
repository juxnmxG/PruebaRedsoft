# Prueba Técnica - .NET 6
1. Construya un servicio RESTful que radique pólizas de seguro de autos en una base de datos
MongoDB La póliza debe contener la siguiente información

- Numero de póliza
- Nombre cliente
- Identificación del cliente
- Fecha de nacimiento del cliente
- Fecha en que se tomó la póliza
- Coberturas cubiertas por la póliza
- Valor máximo cubierto por la póliza
- Nombre del plan de la póliza
- Ciudad de residencia del ciente
- Dirección de residencia del cliente
- Placa Automotor
- Modelo del automotor
- ¿El vehículo tiene inspección?

2. Simule una persistencia con pólizas de seguros que indiquen fecha de inicio y fin de vigencia de la
póliza, cree además una validación de negocio para que no se puedan crear pólizas que no estén
vigentes.

3. Cree en el servicio una consulta que, a partir de la placa del vehículo o número de póliza, retorne la
información de la póliza.

Consideraciones técnicas:

- Debe ser desarrollada utilizando .NET 6 (Obligatorio)
- Debe incluir al menos 3 pruebas unitarias, framework a elección del candidato
- Se debe usar un ORM para la conexión a base de datos
- El API debe estar asegurada utilizando JWT
- Se validarán patrones de arquitectura utilizados
- La prueba se debe sustentar en el periodo de fechas acordado
________
Solución
--------
1. Para Nuestra API RESTful tenemos tenemos los siguientes modelos:

- Cliente

| Referencia  | Tipo        | Nombre de la Variable |
| ------------|------------ |-----------------------|
| nombre del cliente  |  string | Name |
| Identificacín del cliente  |  string | Dni |
| Fecha de nacimiento del cliente  |  DateTime | BirthDate |
| Ciudad de residencia del cliente  |  string | CityOfResidence |
| Identificación  |  string | AddresOfResidence |
--------
- Automotor

| Referencia  | Tipo| Nombre de la Variable |
| ------------|-----|-----------------------|
| Placa Automotor |  string | Plate |
| Modelo del automotor  |  string | Model |
| ¿El vehículo tiene inspección?  | bool | BirthDate |
---------
- Poliza

| Referencia  | Tipo        | Nombre de la Variable |
| ------------|-------------|-----------------------|
| Numero de póliza  |  string | NumberPolicy |
| Fecha en que se tomó la póliza  | DateTime | Date |
|Coberturas cubiertas por la póliza  |  Lista de strings | Coverages |
| Valor máximo cubierto por la póliza  |  decimal | ValueMax |
| Nombre del plan de la póliza  |  string | Plan |
| Fecha de inicio de vigencia  | DateTime | DateInit |
| Fecha de fin de vigencia  | DateTime | DateEnd |
| Cliente  |  Client | ClientPolicy |
| Automotor  | Automotive | AutomotivePolicy |
------
Donde el control principal para gestionar las polizas lo he definido en la clase PolizaController que recibira las solicitudes 
hechas por el cliente. Para cada solicitud se hace una validación con JWT con el objetivo de tener control de las personas que
entren a nuestro sistema.

    namespace PruebaRedsoft.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class PolicyController : ControllerBase
        {
            private readonly IPolicyService policyService;
            private readonly IJwtService jwtService;
            
            public PolicyController(IPolicyService policyService, IJwtService jwtService) {
                this.policyService = policyService;
                this.jwtService = jwtService;
            }
            
            //Punto de acceso para obtener una lista de polizas registradas en el sistema
            [HttpGet]
            public ActionResult<List<Policy>> Get()
            {
                ClaimsIdentity? indentity = HttpContext.User.Identity as ClaimsIdentity;
                dynamic resultToken = jwtService.ValidateToken(indentity);
    
                if (!resultToken.success)
                {
                    return NotFound("Error en la autenticacion");
                }
    
                return policyService.Get();
            }
            
            //Punto de acceso para obtener una Poliza enviando un id, en dado caso que no exista se devolvera un mesaje de error
            [HttpGet("{id}")]
            public ActionResult<Policy> Get(string id)
            {
                ClaimsIdentity? indentity = HttpContext.User.Identity as ClaimsIdentity;
                dynamic resultToken = jwtService.ValidateToken(indentity);
    
                if (!resultToken.success)
                {
                    return NotFound("Error en la autenticacion");
                }
    
                var existingPolicy = policyService.Get(id);
    
                if (existingPolicy == null)
                {
                    return NotFound($"Poliza con el id = {id}");
                }
                return existingPolicy;
            }
            
            //Punto de acceso para crear una poliza
            [HttpPost]
            public ActionResult<Policy> Post([FromBody] Policy policy)
            {
                ClaimsIdentity? indentity = HttpContext.User.Identity as ClaimsIdentity;
                dynamic resultToken = jwtService.ValidateToken(indentity);
    
                if (!resultToken.success)
                {
                    return NotFound("Error en la autenticacion");
                }
    
                policyService.Create(policy);
    
                return CreatedAtAction(nameof(Get), new { id = policy.Id }, policy);
            }
            
            //Punto de acceso para modificar una poliza existente
            [HttpPut("{id}")]
            public ActionResult Put(string id, [FromBody] Policy policy)
            {
                ClaimsIdentity? indentity = HttpContext.User.Identity as ClaimsIdentity;
                dynamic resultToken = jwtService.ValidateToken(indentity);
    
                if (!resultToken.success)
                {
                    return NotFound("Error en la autenticacion");
                }
    
                var existingPolicy = policyService.Get(id);
    
                if (existingPolicy == null)
                {
                    return NotFound($"Poliza con el id = {id}");
                }
    
                policyService.Update(id, policy);
    
                return NoContent();
            }
            
            //Punto de acceso para Eliminar una poliza existente
            [HttpDelete("{id}")]
            public ActionResult Delete(string id)
            {
                ClaimsIdentity? indentity = HttpContext.User.Identity as ClaimsIdentity;
                dynamic resultToken = jwtService.ValidateToken(indentity);
    
                if (!resultToken.success) {
                    return NotFound("Error en la autenticacion");
                }
    
                var existingPolicy = policyService.Get(id);
    
                if (existingPolicy == null)
                {
                    return NotFound($"Poliza con el id = {id} no existe");
                }
    
                policyService.Delete(id);
    
                return NoContent();
            }
            
            //Punto de acceso para obtener una poliza de acuerdo a su numero de poliza o por la placa del automor
            [HttpGet("search")]
            public async Task<ActionResult<Policy>> GetPolicy(int policyNumber, string licensePlate)
            {
                ClaimsIdentity? indentity = HttpContext.User.Identity as ClaimsIdentity;
                dynamic resultToken = jwtService.ValidateToken(indentity);
    
                if (!resultToken.success)
                {
                    return NotFound("Error en la autenticacion");
                }
    
                var policy = await policyService.GetByPolicyNumberOrLicensePlate(policyNumber, licensePlate);
    
                if (policy == null)
                {
                    return NotFound();
                }
    
                return policy;
            }
        }
    }
 ------------------------------
Pruebas de consumo
 ------------------------------
 ![image](https://github.com/juxnmxG/PruebaRedsoft/assets/61563571/2bb11740-9cb5-4857-80e6-95b97d74742c)
 Si hacemos una petición sin que se haya autenticado el resultado es el de la imagen anterior
 
 ![image](https://github.com/juxnmxG/PruebaRedsoft/assets/61563571/a20da0e5-6095-4d50-a0cf-6076becddafb)
  El Resultado al hacer una petición get a /api/policy es el que se evidencia en la imagen anteriror

 ------------------------------
 2. Para crear una poliza utilizaremos una peticipón Post a /api/Policy la cual utiliza la funcion post de controlador
    y activa la funcion del servicio de polizas Create
    
        public Policy Create(Policy policy)
        {
        //Validación de fechas para cuando se crea, si algunas de las dos no cumple, no se crea la poliza.
            if (policy.DateInit > DateTime.Now || policy.DateEnd < DateTime.Now)
            {
                throw new ApplicationException("La póliza no está en vigencia");
            }

            _policys.InsertOne(policy);

            return policy;
        }
  
 -----------
Pruebas de crear
 -----------
 ![image](https://github.com/juxnmxG/PruebaRedsoft/assets/61563571/be597005-a155-43d6-9a78-88848a27cf48)
  El resultado de crear una poliza se evidencia en la imagen anterior.
 -----------
 3. Para nuestro tercer punto he desarrollado el endpoint GetPolicy el cual recibe dos parametros que son el numero de poliza
    y  la placa del automotor para poder hacer nuestro filtrado en la base de datos que cumpla con las condiciones pedidas que son
    traer los datos que se relacionen con esos parametros. Al ejecutarse GetPolicy se hace una validacion JWT para validar si el usuario
    posee permisos para interactuar con el sistema, si el resultado de la validacion es positivo se hace un llamado al servicio 
    GetByPolicyNumberOrLicensePlate que nos traera los datos pertinentes.
    
        public async Task<ActionResult<Policy>> GetPolicy(int policyNumber, string licensePlate)
            {
                ClaimsIdentity? indentity = HttpContext.User.Identity as ClaimsIdentity;
                dynamic resultToken = jwtService.ValidateToken(indentity);
    
                if (!resultToken.success)
                {
                    return NotFound("Error en la autenticacion");
                }
    
                var policy = await policyService.GetByPolicyNumberOrLicensePlate(policyNumber, licensePlate);
    
                if (policy == null)
                {
                    return NotFound();
                }
    
                return policy;
            }
            
            //codigo del servicio de busqueda
            public async Task<Policy> GetByPolicyNumberOrLicensePlate(int policyNumber, string licensePlate)
              {
                 FilterDefinition<Policy> filter = Builders<Policy>.Filter.Eq(p => p.NumberPolicy, policyNumber)
                   | Builders<Policy>.Filter.Eq(p => p.AutomotivePolicy.Plate, licensePlate);

                 return await _policys.Find(filter).FirstOrDefaultAsync();
              }
            
    --------------
   Prueba de busqueda
    ---------------
    la consulta hecha al endpoint /api/Policy/search?policyNumber=12&licensePlate=AMD123 da los siguientes resultaods
    
    ![image](https://github.com/juxnmxG/PruebaRedsoft/assets/61563571/d432478b-e95c-4131-b007-87ed8f810362)
    para una busqueda fallida el resultado es el de la imagen anterior.
    
    ![image](https://github.com/juxnmxG/PruebaRedsoft/assets/61563571/4846a346-3452-49d9-ba54-a3775e430522)
    Resultado de una consulta conresultado positivo.
-----------------
Pruebas Unitarias 
------------------

Codigo y resultado de los test realizados a al Controlado de polizas 

    namespace PruebaRedsoft.Tests
    {
        [TestClass]
        public class PolicyControllerTests
        {
            private Mock<IPolicyService> mockPolicyService;
            private Mock<IJwtService> mockJwtService;
            private PolicyController policyController;
    
            [TestInitialize]
            public void TestInitialize()
            {
                mockPolicyService = new Mock<IPolicyService>();
                mockJwtService = new Mock<IJwtService>();
    
                // Mock ClaimsIdentity
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "Test user"),
                    // Add other claims as needed
                };
                var identity = new ClaimsIdentity(claims, "TestAuthType");
                var claimsPrincipal = new ClaimsPrincipal(identity);
    
                policyController = new PolicyController(mockPolicyService.Object, mockJwtService.Object);
    
                // Setup the controller context
                policyController.ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = claimsPrincipal }
                };
    
                // Set up mockJwtService to return success
                mockJwtService.Setup(service => service.ValidateToken(It.IsAny<ClaimsIdentity>()))
                    .Returns(new { success = true });
            }
    
            [TestMethod]
            public void Get_ReturnsPolicyList_WhenPoliciesExist()
            {
                // Arrange
                var expectedPolicies = new List<Policy> { new Policy { NumberPolicy = 123 } };
                mockPolicyService.Setup(service => service.Get()).Returns(expectedPolicies);
    
                // Act
                var result = policyController.Get();
    
                // Assert
                Assert.AreEqual(expectedPolicies, result.Value);
            }
    
            [TestMethod]
            public void Get_ReturnsPolicy_WhenPolicyExists()
            {
                // Arrange
                var id = "123";
                var expectedPolicy = new Policy { Id = id };
                mockPolicyService.Setup(service => service.Get(id)).Returns(expectedPolicy);
    
                // Act
                var result = policyController.Get(id);
    
                // Assert
                Assert.AreEqual(expectedPolicy, result.Value);
            }
    
            [TestMethod]
            public void Post_ReturnsCreatedAtAction_WhenPolicyIsCreated()
            {
                // Arrange
                var policy = new Policy();
                mockPolicyService.Setup(service => service.Create(policy));
    
                // Act
                var result = policyController.Post(policy);
    
                // Assert
                mockPolicyService.Verify(service => service.Create(policy), Times.Once);
                var createdAtActionResult = result.Result as CreatedAtActionResult;
                Assert.IsNotNull(createdAtActionResult);
                Assert.AreEqual("Get", createdAtActionResult.ActionName);
                Assert.AreEqual(policy, createdAtActionResult.Value);
            }
        } 
    }
 ---------------------
 ![image](https://github.com/juxnmxG/PruebaRedsoft/assets/61563571/823c1db4-8876-42ff-9621-5267a8eef6ff)
Resultado de Tests realizados 
 
