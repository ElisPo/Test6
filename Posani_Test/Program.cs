using System;

namespace Posani_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Cosa vuoi fare dal database Polizia?");
            Console.WriteLine("1. Mostra tutti gli agenti");
            Console.WriteLine("2. Mostra gli agenti assegnati ad un'area");
            Console.WriteLine("3. Mostra gli agenti con più di * anni di servizio");
            Console.WriteLine("4. Aggiungi un agente");

            bool ok = false;
            int scelta = -1;

            while(ok == false)
            {
                ok = Int32.TryParse(Console.ReadLine(), out scelta);
                if (scelta != 1 && scelta != 2 && scelta != 3 && scelta != 4)
                {
                    Console.WriteLine("Scelta non valida. Riprova.");
                    ok = false;
                }
            }

            switch (scelta)
            {
                case 1:
                    Services.Show();
                    break;
                case 2:
                    Console.WriteLine("Nome area?");
                    Services.Show(Console.ReadLine());
                    break;
                case 3:
                    Console.WriteLine("Quanti anni?");
                    int anni = 0;
                    do
                    {
                        ok = Int32.TryParse(Console.ReadLine(), out anni);
                        if (ok == false)
                        {
                            Console.WriteLine("Mi serve un numero.");
                        }
                    } while (ok == false);
                    Services.Show(anni);
                    break;
                case 4:
                    Services.Aggiungi();
                    break;
                default:
                    Console.WriteLine("Qualcosa è andato storto...");
                    break;
            }

        }
    }
}
