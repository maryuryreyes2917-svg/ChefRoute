namespace ChefRoute.Estructuras
{
    public class ListaEnlazadaSimple<T>
    {
        public NodoLista<T>? Cabeza { get; private set; }
        public NodoLista<T>? Cola { get; private set; }
        public int Cantidad { get; private set; }

        public void Agregar(T valor)
        {
            var nuevoNodo = new NodoLista<T>(valor);
            if (Cabeza == null)
            {
                Cabeza = nuevoNodo;
                Cola = nuevoNodo;
            }
            else
            {
                Cola!.Siguiente = nuevoNodo;
                Cola = nuevoNodo;
            }
            Cantidad++;
        }

        public void AgregarAlInicio(T valor)
        {
            var nuevoNodo = new NodoLista<T>(valor);
            if (Cabeza == null)
            {
                Cabeza = nuevoNodo;
                Cola = nuevoNodo;
            }
            else
            {
                nuevoNodo.Siguiente = Cabeza;
                Cabeza = nuevoNodo;
            }
            Cantidad++;
        }
    }
}