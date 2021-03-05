using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Posani_Test
{
    public static class SqlDataReaderExtensions
    {
        //Estensione di SqlDataReader per convertire un record della tabella Agenti in un oggetto di classe Agente
        public static Agente ToAgent(this SqlDataReader reader)
        {
            return new Agente()
            {
                Nome = reader["Nome"].ToString(),
                Cognome = reader["Cognome"].ToString(),
                CodiceFiscale = reader["Codice Fiscale"].ToString(),
                DataDiNascita = Convert.ToDateTime(reader["Data di Nascita"]),
                AnniDiServizio = (int)reader["Anni di Servizio"]
            };
        }
    }
}
