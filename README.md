# README.md

## El siguiente documento detalla como se realizan los diferentes desarrollos al Sistema de Expedientes TDPI.

### Repositorio GIT

### El actual repositorio GIT cuenta con 3 ramas, siendo estas:

1. Main, la cual es la rama que esta encargada del versionamiento Productivo
2. develop, rama encarga de los desarrollos y versionamiento en Testing.
3. release, rama encargada de publicar los cambios en producción.

Todos los desarrollos y mejoras deben ser desarrolladas en la rama develop, creando una nueva rama con la nomenclatura `feature-` seguido del cambio a aplicar
Cuando se termina de desarrollar un modulo en la rama `feature-` se debe solicitar un Pull Request hacia `develop` con la finalidad de validar y probar los cambios y unirlos con los desarrollos internos

Todos los desarrollos realizados, deben quedar registrados en el documento CHANGELOG.md siguiendo la nomenclatura indicada en los links adjuntos en el mismo documento.

## Antes de comenzar a desarrollar

Antes de comenzar a realizar cambios en el proyecto, verificar que se este conectado a la rama develop, ya que esta contiene los ultimos cambios del ambiente de Testing y crear la respectiva rama de desarrollo `feature-`.
