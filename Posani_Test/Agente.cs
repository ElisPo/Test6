using System;
using System.Collections.Generic;
using System.Text;

namespace Posani_Test
{
    //Classe concreta Agente, implementa le proprietà astratte di Persona da cui eredita
    public class Agente : Persona
    {
        public override string Nome { get; set; }
        public override string Cognome { get; set; }
        public override string CodiceFiscale { get; set; }

        //Aggiungo proprietà specifiche di un Agente, cioè i campi della tabella Agenti del DataBase Polizia da cui andrò a prendere gli agenti
        public DateTime DataDiNascita { get; set; }
        public int AnniDiServizio { get; set; }

        //Sovrascrivo la funzione ToString per questa classe, che ritorna una stringa con Codice Fiscale, Nome Cognome ed Anni di Servizio 
        //dell'Agente istanziato che la richiama.
        public override string ToString()
        {
            return CodiceFiscale + " - " + Nome + " " + Cognome + " - " + AnniDiServizio.ToString() + " anni di servizio";
        }

        
    }
}
