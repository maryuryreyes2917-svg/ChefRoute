namespace ChefRoute.Estructuras
{
    public class ListaEnlazadaCircular<T>
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
                nuevoNodo.Siguiente = Cabeza;
            }
            else
            {
                Cola!.Siguiente = nuevoNodo;
                Cola = nuevoNodo;
                Cola.Siguiente = Cabeza;
            }
            Cantidad++;
        }
    }
}