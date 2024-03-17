### Español
# Expanded Lists
_Expanded Lists_, antes _ListaBloques Remake_ es una biblioteca de estructuras de datos, por ahora solo listas, en C# que ofrecen más métodos que las implementaciones del lenguaje. Intento que mantengan un buen rendimineto, pero aún no he hecho pruebas de redimiento.
Ha sido traducida a inglés para que sea más accesible porque dudo mucho que nadie vaya a usarla en español.

## Estado de desarrollo
### Lanzamiento 0.1
La parte más importante de esta versión, y uno de los motivos para crear este repositorio, es _ListBloques_.
Una lista que usa arrays y _Bloques<sup>*</sup>_ para ofrecer los beneficios de un array y una lista enlazada.<br/>
Usa Lists para guardar los bloques y las posiciones que representan en la lista para agilizar el acceso a los elementos respecto a las listas enlazadas y usa los bloques para que la lista esté contenida en trozos de memoria separados como las listas enlazadas.
Se le puede proporcionar una función para obtener longitudes para bloques nuevos y gestionar así la memoria usada, longitudes distintas darán rendimientos distintos.
 - <sup>*</sup>Los Bloques son listas de capacidad fija usadas en la implementación de las listas de bloques
   - Pueden usarse sin listas, pero no están pensadas para ello
   - Por ahora la única implementación de Bloque es ArrayBloque
     - Puede ser usado como una expansión de un array, que incluye una conversión implicita de esta

Tiene una jerarquía de interfaces, permitiendo la creación de listas con métodos y propiedades distintos.

Además incluye ListSerie, usa un List para implementar la interfaz ISerie, que permite usar funciones para generar nuevos elementos e insertarlos.
 - ListSerie, no ha sido completamente depurada y puede contener errores, pero será usada en el repositorio [DivClac](https://github.com/JosePrietoPaez/DivCalc)

### Lanzamiento 0.2
En esta versión el proyecto ha sido traducido al inglés, lo he traducio yo, por lo que puede haber errores.
He aprovechado para cambiar clases y métodos mal nombrados, como _ISerie_, que representa una sucesión en lugar de una serie y, por lo tanto, ahora se llama _ISequence_.

## Objetivos para versiones futuras
 - Asegurar la eficiencia de ListBloques
 - Refinar la jerarquía de interfaces
 - Implementar otra lista, parecida a ListBloques, pero permitiendo que haya huecos en los bloques y que permita que haya bloques vacíos
   - Estas diferencias permitirán usar esta estructura como una matriz, quizás usar un array de Bloques como una matriz sea más obvio que como una lista
    - Esto puede requerir cambios en la jerarquía de interfaces
 - Hacer algo con las listas ordenadas

### English
# Expanded Lists
_Expanded Lists_, previously _ListaBloques Remake_ is a C# data structures library, currently only containing lists. These lists offer more methods than the implementations of _System.Collections_. While I try to ensure efficiency, I still have not performed performance tests.
This library was previosuly in Spanish.

## Development notes
### Release 0.2
Mostly the same as 0.1, ignoring the translation almost of the entire library.

The most important feature from 0.1, and the reason I considered publishing this library, is _BlockList_.
A list that uses _Blocks<sup>*</sup>_ to offer the benefits from both array lists and linked lists.<br/>
Uses Lists to store blocks and the positions of their first elements for faster random access than linked lists and uses block for better memory management than regular array lists.
Can be given a function to get lengths for new blocks, this can be used to manage memory usage, different block lengths will have different performances.
 - <sup>*</sup>Blocks are fixed-capacity lists used by block lists
   - Blocks may be used by themselves but are not designed with this use in mind
   - Currently the only block is _ArrayBlock_
     - Can be used as an extension of an array, featuring an implicit conversion to ArrayBlock

Features an interface hierarchy, allowing for the creation of lists with different methods and properties.

Also includes _ListSequence_, which delegates its methods to a List. Can be given a name , this makes its ToString() return a string with the index for each element.
 - This class has not been properly tested and was created for use in [DivClac](https://github.com/JosePrietoPaez/DivCalc).

## Plans for future releases
 - Ensure BlockList has good performance
 - Refine the interfaces
 - Create a new block list, which can allow empty slots and empty blocks
   - This list would be designed to be used as a matrix, which, perhaps, is more intuitive than a list like _BlockList_
    - May require changing the interface hierarchy
 - Do something with sorted lists
