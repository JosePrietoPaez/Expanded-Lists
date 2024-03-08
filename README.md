# ListaBloques-Remake
_ListaBloques-Remake_ es una biblioteca de estructuras de datos, por ahora solo listas, en C# que ofrecen más métodos que las implementaciones del lenguaje. Intento que mantengan un buen rendimineto, pero aún no he hecho pruebas de redimiento 

## Estado de desarrollo
### Lanzamiento 0.1
La parte más importante de esta versión, y uno de los motivos para crear este repositorio, es _ListBloques_.
Una lista que usa arrays y _Bloques<sup>*</sup>_ para ofrecer los beneficios de un array y una lista enlazada.<br/>
Usa arrays para guardar los bloques y las posiciones que representan en la lista para agilizar el acceso a los elementos respecto a las listas enlazadas y usa los bloques para que la lista esté contenida en trozos de memoria separados como las listas enlazadas
 - <sup>*</sup>Los Bloques son listas de capacidad fija usadas en la implementación de las listas de bloques
   - Pueden usarse sin listas, pero no están pensadas para ello
   - Por ahora la única implementación de Bloque es ArrayBloque
     - Puede ser usado como una expansión de un array, que incluye una conversión implicita de esta

Tiene una jerarquía de interfaces, permitiendo la creación de listas con más métodos que la lista base.

Además incluye ListSerie, usa un List para implementar la interfaz ISerie, que permite usar funciones para generar nuevos elementos e insertarlos.
 - ListSerie, no ha sido completamente depurada y puede contener errores, pero será usada en el repositorio [DivClac](https://github.com/JosePrietoPaez/DivCalc)

## Objetivos para versiones futuras
 - Asegurar la eficiencia de ListBloques
 - Refinar la jerarquía de interfaces
 - En el futuro seguramente cambie el idioma de los métodos a inglés, no solo para hacer este repositorio más accesible, también para ofrecer más características que necesitan propiedades en inglés
   - Téngase esto en cuenta si espera usar versiones futuras de este repositorio
 - Implementar otra lista, parecida a ListBloques, pero permitiendo que haya huecos en los bloques y que permita que haya bloques vacíos
   - Estas diferencias permitirán usar esta estructura como una matriz, quizás usar un array de Bloques como una matriz sea más obvio que como una lista
   - Esto puede requerir cambios en la jerarquía de interfaces
   - Cuando haga este cambio podría hacer que ILista herede de IList
 - Hacer algo con las listas ordenadas
