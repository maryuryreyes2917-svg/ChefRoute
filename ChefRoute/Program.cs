using System;
using System.IO;
using System.Text.Json;
using ChefRoute.Estructuras;
using ChefRoute.Modelos;

namespace ChefRoute
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== BIENVENIDO A CHEFROUTE ===");

            Grafo grafoCocina = new Grafo();
            string nombrePlato = string.Empty;

            try
            {
                string jsonText = File.ReadAllText("receta.json");
                using (JsonDocument document = JsonDocument.Parse(jsonText))
                {
                    JsonElement root = document.RootElement;
                    nombrePlato = root.GetProperty("plato").GetString() ?? "Plato Desconocido";

                    JsonElement tareasElement = root.GetProperty("tareas");
                    foreach (JsonElement tarea in tareasElement.EnumerateArray())
                    {
                        string? nombreTarea = tarea.GetString();
                        if (nombreTarea != null)
                        {
                            grafoCocina.AgregarVertice(new Tarea(nombreTarea));
                        }
                    }

                    JsonElement dependenciasElement = root.GetProperty("dependencias");
                    foreach (JsonElement dep in dependenciasElement.EnumerateArray())
                    {
                        string? desde = dep.GetProperty("desde").GetString();
                        string? hacia = dep.GetProperty("hacia").GetString();
                        int tiempo = dep.GetProperty("tiempoMinutos").GetInt32();

                        if (desde != null && hacia != null)
                        {
                            grafoCocina.AgregarArista(desde, hacia, tiempo);
                        }
                    }
                }

                Console.WriteLine($"[EXITO] Receta '{nombrePlato}' cargada correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Falla al leer el archivo JSON: {ex.Message}");
                return;
            }

            Console.WriteLine("\nVerificando viabilidad de la receta (Ciclos)...");
            if (grafoCocina.ContieneCiclos())
            {
                Console.WriteLine("[ALERTA] La receta contiene dependencias circulares y es imposible de cocinar.");
                return;
            }
            Console.WriteLine("La receta es viable. No se detectaron ciclos.");

            Console.WriteLine("\nGenerando Secuencia de Preparación (Orden Topológico)...");
            ListaEnlazadaSimple<Tarea>? ordenPreparacion = grafoCocina.ObtenerOrdenTopologico();

            if (ordenPreparacion == null) return;

            ListaEnlazadaCircular<Tarea> rondaChef = new ListaEnlazadaCircular<Tarea>();

            var nodoActualSimple = ordenPreparacion.Cabeza;
            int paso = 1;
            while (nodoActualSimple != null)
            {
                Console.WriteLine($"Paso {paso}: {nodoActualSimple.Valor.Nombre}");
                rondaChef.Agregar(nodoActualSimple.Valor);
                nodoActualSimple = nodoActualSimple.Siguiente;
                paso++;
            }

            Console.WriteLine("\n=== INICIANDO RONDA DE SUPERVISIÓN DEL CHEF ===");
            Console.WriteLine("Presiona ENTER para avanzar a la siguiente estación (Escribe 'salir' para terminar)");

            var estacionActual = rondaChef.Cabeza;
            int ronda = 1;

            while (estacionActual != null)
            {
                Console.Write($"[Ronda {ronda}] Chef supervisando estación: {estacionActual.Valor.Nombre}. (ENTER para continuar)");
                string? input = Console.ReadLine();

                if (input?.Trim().ToLower() == "salir")
                {
                    break;
                }

                estacionActual = estacionActual.Siguiente;
                if (estacionActual == rondaChef.Cabeza)
                {
                    ronda++;
                    Console.WriteLine("--- El chef completó una vuelta completa por la cocina ---");
                }
            }

            Console.WriteLine("Ronda de supervisión terminada.");
        }
    }
}
