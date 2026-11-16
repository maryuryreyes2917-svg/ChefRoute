using ChefRoute.Estructuras;

namespace ChefRoute.Modelos
{
    public enum EstadoVisita
    {
        NoVisitado,
        EnProceso,
        Visitado
    }

    public class Vertice
    {
        public Tarea InfoTarea { get; set; }
        public ListaEnlazadaSimple<Arista> Aristas { get; set; }
        public EstadoVisita Estado { get; set; } 

        public Vertice(Tarea infoTarea)
        {
            InfoTarea = infoTarea;
            Aristas = new ListaEnlazadaSimple<Arista>();
            Estado = EstadoVisita.NoVisitado;
        }
    }
}