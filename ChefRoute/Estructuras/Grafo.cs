using ChefRoute.Modelos;

namespace ChefRoute.Estructuras
{
    public class Grafo
    {
        public ListaEnlazadaSimple<Vertice> Vertices { get; private set; }

        public Grafo()
        {
            Vertices = new ListaEnlazadaSimple<Vertice>();
        }

        public void AgregarVertice(Tarea tarea)
        {
            if (ObtenerVertice(tarea.Nombre) == null)
            {
                Vertices.Agregar(new Vertice(tarea));
            }
        }

        public void AgregarArista(string nombreOrigen, string nombreDestino, int tiempo)
        {
            Vertice? origen = ObtenerVertice(nombreOrigen);
            Vertice? destino = ObtenerVertice(nombreDestino);

            if (origen != null && destino != null)
            {
                origen.Aristas.Agregar(new Arista(destino.InfoTarea, tiempo));
            }
        }

        public Vertice? ObtenerVertice(string nombreTarea)
        {
            var actual = Vertices.Cabeza;
            while (actual != null)
            {
                if (actual.Valor.InfoTarea.Nombre == nombreTarea)
                {
                    return actual.Valor;
                }
                actual = actual.Siguiente;
            }
            return null;
        }
        public bool ContieneCiclos()
        {
            var actual = Vertices.Cabeza;
            while (actual != null)
            {
                actual.Valor.Estado = EstadoVisita.NoVisitado;
                actual = actual.Siguiente;
            }

            actual = Vertices.Cabeza;
            while (actual != null)
            {
                if (actual.Valor.Estado == EstadoVisita.NoVisitado)
                {
                    if (DFSCiclo(actual.Valor))
                    {
                        return true; 
                    }
                }
                actual = actual.Siguiente;
            }
            return false;
        }

        private bool DFSCiclo(Vertice vertice)
        {
            vertice.Estado = EstadoVisita.EnProceso;

            var actualArista = vertice.Aristas.Cabeza;
            while (actualArista != null)
            {
                Vertice? destino = ObtenerVertice(actualArista.Valor.Destino.Nombre);

                if (destino != null)
                {
                    if (destino.Estado == EstadoVisita.EnProceso)
                    {
                        return true; 
                    }
                    if (destino.Estado == EstadoVisita.NoVisitado)
                    {
                        if (DFSCiclo(destino))
                        {
                            return true;
                        }
                    }
                }
                actualArista = actualArista.Siguiente;
            }

            vertice.Estado = EstadoVisita.Visitado;
            return false;
        }
        public ListaEnlazadaSimple<Tarea>? ObtenerOrdenTopologico()
        {
            if (ContieneCiclos())
            {
                return null;
            }

            var actual = Vertices.Cabeza;
            while (actual != null)
            {
                actual.Valor.Estado = EstadoVisita.NoVisitado;
                actual = actual.Siguiente;
            }

            var ordenResultante = new ListaEnlazadaSimple<Tarea>();

            actual = Vertices.Cabeza;
            while (actual != null)
            {
                if (actual.Valor.Estado == EstadoVisita.NoVisitado)
                {
                    DFSTopologico(actual.Valor, ordenResultante);
                }
                actual = actual.Siguiente;
            }

            return ordenResultante;
        }

        private void DFSTopologico(Vertice vertice, ListaEnlazadaSimple<Tarea> ordenResultante)
        {
            vertice.Estado = EstadoVisita.Visitado;

            var actualArista = vertice.Aristas.Cabeza;
            while (actualArista != null)
            {
                Vertice? destino = ObtenerVertice(actualArista.Valor.Destino.Nombre);

                if (destino != null && destino.Estado == EstadoVisita.NoVisitado)
                {
                    DFSTopologico(destino, ordenResultante);
                }
                actualArista = actualArista.Siguiente;
            }
            ordenResultante.AgregarAlInicio(vertice.InfoTarea);
        }
    }
}