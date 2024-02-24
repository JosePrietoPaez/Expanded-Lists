# ListaBloques-Remake
En _ListaBloques-Remake_ están la recreación y expansión en C# de la biblioteca de listas y sus clases relacionadas para la creación de una aplicación de reglas de divisibilidad que creé el verano de 2023 en Java, una idea en la que he estado trabajando desde 2021.

## Objetivos
 - Refinar la jerarquía de las interfaces, en la versión de Java, todas las características de las listas se encontraban en dos interfaces
 - Crear una versión más eficiente de _LinkedListaBloques_ de la versión de Java, ha sido renombrada a _ListBloques_
 - Rediseñar y crear una nueva versión de la calculadora de reglas divisibilidad, que por ahora llamaré _DivCalc_
   - Digo por ahora, pero no creo que lo cambie
   - Cuando comienze el desarrollo de GUI crearé un nuevo repositorio, moveré el código de _DivCalc_ cuando lo haga
 
## Estado de desarrollo
 - He creado una versión de consola de _DivCalc_, que tiene la capacidad de calcular reglas de divisibilidad dado una base y un divisor
 - Respecto a las estructuras de datos, el diseño de las interfaces está casi finalizado, sólo queda la interfaz para listas ordenadas
   - Esta no es una prioridad, pero pienso hacerlo una vez acabe con _ListBloques_
 - Para mostrar el uso de estas inerfaces y ofrecer una implementación, las clases C# _ListSerie_ y _ListBloques_
   - Ambas implementan una interfaz mediante el uso de una _List_ de _System.Collections_
   - Por ahora, _ListSerie_ está terminada y _ListBloques_ está siendo depurada
     - A pesar de que _ListBloques_ funcionara en Java, los grandes cambios a las interfaces, cambios en detalles de implementación y errores a pasar el código a C# me obligan a volver a crear pruebas para esta clase

### Sobre la biblioteca original en Java
Entiendo que puede ser confuso que haga referencia a una versión en Java sin compartirla.<br/>
No publicaré la biblioteca orignal, ya que contiene bastantes errores y a estas alturas es muy diferente a la versión actual.
