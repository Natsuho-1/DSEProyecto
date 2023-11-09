-- Creación de la base de datos
CREATE DATABASE observatoriodb;

-- Selección de la base de datos
USE observatoriodb;

-- Tabla para administradores
CREATE TABLE Administradores (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Usuario VARCHAR(50) NOT NULL UNIQUE, -- Cambiado de Correo a Usuario
    Contrasena VARCHAR(255) NOT NULL,
    Rol INT DEFAULT 1
);

-- Tabla para la bodega
CREATE TABLE Bodegas (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(255) NOT NULL,
    Descripcion TEXT,
    Cantidad INT
);

-- Tabla para colaboradores
CREATE TABLE Colaboradores (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Usuario VARCHAR(50) NOT NULL UNIQUE,
    Correo VARCHAR(255) NOT NULL,
    Contrasena VARCHAR(255) NOT NULL,
    Nombre VARCHAR(255) NOT NULL,
    Apellido VARCHAR(255) NOT NULL,
    Rol INT DEFAULT 2
);

-- Tabla para documentos
CREATE TABLE Documentos (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Titulo VARCHAR(255) NOT NULL,
    ColaboradorID INT,
    Fecha timestamp default current_timestamp,
    URL VARCHAR(255) NOT NULL,
    FOREIGN KEY (ColaboradorID) REFERENCES Colaboradores(ID)
);
