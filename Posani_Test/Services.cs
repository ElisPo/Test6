using System;
using System.Collections.Generic;
using System.Text;

namespace Posani_Test
{
    public class Services
    {
        public static void Show()
        {
            foreach (Object obj in ADOMethods.GetAllAgents())
            {
                Console.WriteLine(obj.ToString());
            }
        }

        public static void Show(string code)
        {
            foreach (Object obj in ADOMethods.GetAgentsByAreaCode(code))
            {
                Console.WriteLine(obj.ToString());
            }
        }

        public static void Show(int anni)
        {
            foreach (Object obj in ADOMethods.GetAgentsByServiceYears(anni))
            {
                Console.WriteLine(obj.ToString());
            }
        }

        public static void Aggiungi()
        {
            //Prendo i dati dell'agente dall'utente a console
            Console.WriteLine("Aggiungo un nuovo agente!\nNome?");
            string nome = Console.ReadLine();
            Console.WriteLine("Cognome?");
            string cognome = Console.ReadLine();
            Console.WriteLine("Codice Fiscale? ");
            string codiceFiscale = "";
            bool ok = false;

            while (ok == false) //Controllo sulla lunghezza del codice fiscale
            {
                codiceFiscale = Console.ReadLine();
                if (codiceFiscale.Length != 16)
                {
                    Console.WriteLine("Codice Fiscale non valido.");
                }
                else { ok = true; }
            }
            
            Console.WriteLine("Data di Nascita? ");
            DateTime dataNascita = new DateTime(); //Check sulla validità della data
            do
            {
                ok = DateTime.TryParse(Console.ReadLine(), out dataNascita);
                if (ok == false)
                {
                    Console.WriteLine("Data di Nascita non valida.");
                }

            } while (ok == false);

            Console.WriteLine("Anni di Servizio?"); //Check sulla validità dell'input come int
            int anniServizio = 0;
            do
            {
                ok = Int32.TryParse(Console.ReadLine(), out anniServizio);
                if (ok == false)
                {
                    Console.WriteLine("Inserire un numero valido.");
                }
            } while (ok == false);

            //creazione del nuovo agente con i dati in input
            Agente agente = new Agente
            {
                Nome = nome,
                Cognome = cognome,
                CodiceFiscale = codiceFiscale,
                DataDiNascita = dataNascita,
                AnniDiServizio = anniServizio
            };

            //aggiungo l'agente al database tramite la disconnected mode
            ADOMethods.AddNewAgent(agente);
            //Mostro la lista aggiornata degli agenti
            Console.WriteLine("Ecco la lista agenti aggiornata:");
            Show();
        }
    }
}
