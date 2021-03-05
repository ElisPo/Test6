using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Posani_Test
{
    public class ADOMethods
    {
        //Definisco la stringa di connessione al database Polizia 
        const string connectionString = @"Persist Security Info=False; Integrated Security=True; Initial Catalog= Polizia; Server=.\SQLEXPRESS";

        //Metodo che ritorna una lista con tutti gli agenti nella tabella del database
        public static List<Agente> GetAllAgents()
        {
            List<Agente> agenti = new List<Agente>(); //Creo una lista di agenti vuota

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Creo il comando per selezionare tutti gli agenti dalla tabella, tramite la connessione appena definita
                SqlCommand getAgents = new SqlCommand("SELECT * FROM Agenti", connection);
                getAgents.CommandType = System.Data.CommandType.Text;

                try
                {
                    //Definisco il DataReader per eseguire il comando e apro la connessione
                    connection.Open();
                    SqlDataReader reader = getAgents.ExecuteReader();


                    while (reader.Read())
                    {
                        agenti.Add(reader.ToAgent()); //Aggiungo alla lista tutti gli agenti nella tabella attraverso l'estensione ad hoc ToAgent() 
                    }

                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally //In ogni caso chiudo la connessione
                {
                    connection.Close();
                }

                return agenti; //ritorno la lista appena riempita
            }
        }

        //Metodo che ritorna una lista con gli agenti della tabella del database assegnati alle aree con il codice area passato in input
        public static List<Agente> GetAgentsByAreaCode(string area)
        {
            List<Agente> agenti = new List<Agente>(); //creao una lista di oggetti agente da riempire

            //creo la connessione
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //creo il comando associato alla connessione precedente, di tipo testo con la stringa di testo in SQL: fa un inner join tra tre tabelle
                //perché all'agente viene assegnata un'area tramite la tabella ponte AssegnazioneAree, mentre il Codice Area è solo nella tabella Aree

                SqlCommand getByArea = new SqlCommand();
                getByArea.Connection = connection;
                getByArea.CommandType = System.Data.CommandType.Text;
                getByArea.CommandText = "SELECT * FROM Agenti WHERE ID IN " +
                    "(SELECT Agenti.ID FROM (Agenti INNER JOIN AssegnazioneAree ON Agenti.ID = AssegnazioneAree.AgenteID )" +
                    " INNER JOIN Aree ON AreaID = Aree.ID WHERE[Codice Area] = @area)";
                getByArea.Parameters.AddWithValue("@area", area);

                try
                {
                    //definisco il DataReader come esecuzione del comando getByArea e apro la connessione
                    connection.Open();
                    SqlDataReader reader = getByArea.ExecuteReader();

                    while (reader.Read())
                    {
                        agenti.Add(reader.ToAgent());
                    }

                } catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally //in ogni caso devo chiudere la connessione
                {
                    connection.Close();
                }
            }

            return agenti; //ritorno la lista appena creata
        }

        //Metodo che ritorna una lista con gli agenti della tabella del database con un numero di anni di servizio maggiore o uguale al numero passato in input
        public static List<Agente> GetAgentsByServiceYears(int anni)
        {
            List<Agente> agenti = new List<Agente>(); //Lista di oggetti Agente da riempire con i dati richiesti

            //Creo la connessione al database 
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                //Creo il comando per selezionare gli agenti con un'anzianità > anni in input
                SqlCommand getByYears = new SqlCommand("SELECT * FROM Agenti WHERE [Anni di Servizio] > @anni", connection);
                getByYears.CommandType = System.Data.CommandType.Text;
                getByYears.Parameters.AddWithValue("@anni", anni);

                try
                {
                    //Apro la connessione e definisco il DataReader come esecuzione del comando getByYears
                    connection.Open();
                    SqlDataReader reader = getByYears.ExecuteReader();

                    //Inserisco nella lista gli Agenti letti dal reader

                    while (reader.Read())
                    {
                        agenti.Add(reader.ToAgent());
                    }

                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return agenti;
        }

        public static bool AddNewAgent(Agente agente)
        {
            //Creo la connessione
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Creo un nuovo data adapter da istruire
                SqlDataAdapter adapter = new SqlDataAdapter();

                //Definisco il comando per inserire l'agente in input
                SqlCommand insert = new SqlCommand("INSERT INTO Agenti VALUES (@nome, @cognome, @codiceFiscale, @dataDiNascita, @anniDiServizio)", connection);
                insert.CommandType = System.Data.CommandType.Text;

                insert.Parameters.Add("@nome", SqlDbType.NVarChar, 255, "Nome");
                insert.Parameters.Add("@cognome", SqlDbType.NVarChar, 255, "Cognome");
                insert.Parameters.Add("@codiceFiscale", SqlDbType.NVarChar, 255, "Codice Fiscale");
                insert.Parameters.Add("@dataDiNascita", SqlDbType.Date, 31, "Data di Nascita");
                insert.Parameters.Add("@anniDiServizio", SqlDbType.Int, 100, "Anni di Servizio");

                //insert.Parameters.AddWithValue("@nome", agente.Nome);
                //insert.Parameters.AddWithValue("@cognome", agente.Cognome);
                //insert.Parameters.AddWithValue("@codiceFiscale", agente.CodiceFiscale);
                //insert.Parameters.AddWithValue("@dataDiNascita", agente.DataDiNascita);
                //insert.Parameters.AddWithValue("@anniDiServizio", agente.AnniDiServizio);

                //Definisco il comando select per riempire la tabella
                SqlCommand select = new SqlCommand("SELECT * FROM Agenti", connection);
                select.CommandType = System.Data.CommandType.Text;

                //Creo il dataset da riempire con gli Agenti e istruisco l'adapter con i nuovi comandi
                DataSet agentiData = new DataSet();
                adapter.SelectCommand = select;
                adapter.InsertCommand = insert;

                try
                {
                    //Apro la connessione e ottengo i dati per riempire la tabella. Da qui lavoro in local fino all'update finale
                    connection.Open();
                    adapter.Fill(agentiData, "Agenti");

                    //Creo una nuova riga da aggiungere alla tabella con i dati del nuovo agente da inserire
                    DataRow nuovoAgente = agentiData.Tables["Agenti"].NewRow();
                    nuovoAgente["Nome"] = agente.Nome;
                    nuovoAgente["Cognome"] = agente.Cognome;
                    nuovoAgente["Codice Fiscale"] = agente.CodiceFiscale;
                    nuovoAgente["Data di Nascita"] = agente.DataDiNascita;
                    nuovoAgente["Anni di Servizio"] = agente.AnniDiServizio;

                    //Aggiungo la riga alla tabella
                    agentiData.Tables["Agenti"].Rows.Add(nuovoAgente);

                    //Aggiorno il database attraverso l'adapter
                    adapter.Update(agentiData, "Agenti");

                    Console.WriteLine("Agente aggiunto con successo!");

                    connection.Close(); //se l'update va a buon fine chiudo la connessione prima del return 

                    return true; //ritorna un valore positivo 

                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    connection.Close(); //se ci fosse qualche errore chiudo comunque la connessione prima del return
                    return false;
                }
            }
        }

    }
}
