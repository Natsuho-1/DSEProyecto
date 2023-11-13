-- Creación de la base de datos
CREATE DATABASE observatoriodb;

-- Selección de la base de datos
USE observatoriodb;

-- Tabla para la bodega
CREATE TABLE Bodegas (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(255) NOT NULL,
    Descripcion TEXT,
    Cantidad INT
);
CREATE TABLE Roles (
    ID INT AUTO_INCREMENT PRIMARY KEY,
   Rol VARCHAR(20) NOT NULL
);

-- Tabla para colaboradores
CREATE TABLE Colaboradores (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Usuario VARCHAR(50) NOT NULL UNIQUE,
	Rol INT,
    Correo VARCHAR(255) NOT NULL,
    Contrasena VARCHAR(255) NOT NULL,
    Nombre VARCHAR(255) NOT NULL,
    Apellido VARCHAR(255) NOT NULL,
    Estado INT NOT NULL default 1,
    FOREIGN KEY (Rol) REFERENCES Roles(ID)
);

-- Tabla para documentos
CREATE TABLE Documentos (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Titulo VARCHAR(255) NOT NULL,
    UsuarioID INT NOT NULL,
    Rol INT NOT NULL,
    Fecha timestamp default current_timestamp,
    URL VARCHAR(255) NOT NULL,
    FOREIGN KEY (UsuarioID) REFERENCES Colaboradores(ID)
);
