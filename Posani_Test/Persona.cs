using System;
using System.Collections.Generic;
using System.Text;

namespace Posani_Test
{
    //Classe astratta che contiene informazioni generali di una persona 
    public abstract class Persona
    {
        public abstract string Nome { get; set; }
        public abstract string Cognome { get; set; }
        public abstract string CodiceFiscale { get; set; }

        //Sovrascrivo il metodo Equals per rendere due persone uguali se hanno lo stesso codice fiscale
        public override bool Equals(Object obj)
        {
            return obj is Persona persona && CodiceFiscale == persona.CodiceFiscale; //se l'oggetto non è una persona non sono uguali a prescindere
        }

        //Aggiungo gli operatori == e != attraverso il risultato di Equals
        public static bool operator ==(Persona left, Persona right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Persona left, Persona right)
        {
            return !left.Equals(right);
        }

    }
}
