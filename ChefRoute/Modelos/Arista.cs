namespace ChefRoute.Modelos
{
    public class Arista
    {
        public Tarea Destino { get; set; }
        public int TiempoMinutos { get; set; }

        public Arista(Tarea destino, int tiempoMinutos)
        {
            Destino = destino;
            TiempoMinutos = tiempoMinutos;
        }
    }
}