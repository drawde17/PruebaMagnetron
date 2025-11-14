# Documentacion
Descripción: Api Rest en la cual se podrá consumir los servicios para Consultar, Crear, Editar y eliminar
			productos, personas y facturas, tendrá la posibilidad de consultar vistas de Bd en el servicio Reportes.

Tecnología utilizada: La aplicación se encuentra desarrollada en NET CORE  6 y se utiliza como motor 
			de base de datos SQL SERVER.

Arquitectura:Se implementó una estructura por capas confirmada por las siguientes:
	Capa API: Se encuentran los controladores y la configuración de la aplicación.
	Capa Dominio: Se encuentran los modelos de base datos y los DTOs.
	Capa Servicio: Se encuentra la lógica de negocio.
	Capa Infraestructura: Se encarga de gestionar la comunicación con la base de datos.
	Ventajas:
		* Separación de responsabilidades: cada capa tiene una única responsabilidad.
		* Ágil desarrollo y mantenimiento: es un patrón común.
		* Reutilización: se puede usar diferentes clases de apoyo entre capas.
	Desventajas:
		* Rendimiento: de no estar optimizada puede haber sobrecargas.
		* Dependencia: si el diseño no se encuentra óptimo puede haber alta dependencia entre las capas.
Motivos: Se selecciona esta tecnologia ya que es ampliamente soportada, la comunidad esta activa y es robusta.

#Instalacion
Requerimiento: Se debe instalar el SKD de net core 6.0
Base de datos: Se implementó SQL SERVER para lo cual se debe crear la base de datos PruebaMagnetron, 
			se puede iniciar sesión con la autenticación de windows o si es necesario se debe crear un usuario de SQL 
			configuración opcional para la creación del usuario.
			En el proyecto en la en la carpeta scripts se encuentra un backup con la configuración inicial de datos y tablas
			SQL creacion usuario:
				USE master;
				GO
				CREATE LOGIN [UsuarioPruebaMagnetron] 
					WITH PASSWORD = N'TestMagnetron', 
					CHECK_POLICY = ON, -- Aplica la política de contraseñas de Windows
					CHECK_EXPIRATION = ON; -- Fuerza la expiración de la contraseña
				GO
				USE [PruebaMagnetron]; -- Cambia esto por el nombre de tu base de datos
				GO
				-- Crea el usuario y lo mapea al login que acabas de crear
				CREATE USER [UsuarioPruebaMagnetron] FOR LOGIN [UsuarioPruebaMagnetron];
				GO
				-- Otorga permisos (ejemplo: db_owner)
				EXEC sp_addrolemember 'db_owner', 'UsuarioPruebaMagnetron';
				-- Si prefieres permisos más limitados, usa:
				-- EXEC sp_addrolemember 'db_datareader', 'UsuarioPrueba';
				-- EXEC sp_addrolemember 'db_datawriter', 'UsuarioPrueba';
				GO
			Configuracion cadena de conexion base de datos: en el archivo appsettings.json se encuentran 
				diferentes cadenas de conexión dependiendo el uso
				* Cadena conexion para usuario de windows: "Server=DRAWDEPC\\SQLEXPRESS; Database=PruebaMagnetron; trusted_Connection=True;"
				* Cadena de conexion para usuario de SQL:"Server=DRAWDEPC\\SQLEXPRESS; Database=PruebaMagnetron; User Id=UsuarioPruebaMagnetron; Password=TestMagnetron; Encrypt=False;"
				* Cadena de conexion para Docker: "Server=host.docker.internal,1433; Database=PruebaMagnetron; User Id=UsuarioPruebaMagnetron; Password=TestMagnetron; Encrypt=False; TrustServerCertificate=True;" 

