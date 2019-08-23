using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using SimpleRestServer.Models;
using System.Configuration;

namespace SimpleRestServer
{
    public class PersonPersistence
    {
        private MySql.Data.MySqlClient.MySqlConnection conn;

        public PersonPersistence()
        {
            string myConnectionString;
            myConnectionString = ConfigurationManager.ConnectionStrings["localDB"].ConnectionString;
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open(); //Open the connection
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {

            }
            //Once the object is constructed, the connection is open
        }

        public long SavePerson(Person personToSave)
        {
            string sqlString = "INSERT INTO tblpersonnel (FirstName, LastName, PayRate, StartDate, EndDate) VALUES('" +
                personToSave.FirstName + "', '" +
                personToSave.LastName + "', " +
                personToSave.PayRate + ", '" +
                personToSave.StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "', '" +
                personToSave.EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "')";

            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);
            cmd.ExecuteNonQuery();
            long id = cmd.LastInsertedId;
            return id;
        }

        public Person GetPerson(long id)
        {
            string sqlString = "SELECT * FROM tblpersonnel WHERE ID=" + id.ToString();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

            MySql.Data.MySqlClient.MySqlDataReader reader = null;            
            reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                Person p = new Person();
                p.ID = reader.GetInt32(0);
                p.FirstName = reader.GetString(1);
                p.LastName = reader.GetString(2);
                p.PayRate = reader.GetFloat(3);
                p.StartDate = reader.GetDateTime(4);
                p.EndDate = reader.GetDateTime(5);
                return p;
            }
            else
            {
                return null;
            }
        }

        public ArrayList GetPersons()
        {
            ArrayList persons = new ArrayList();

            string sqlString = "SELECT * FROM tblpersonnel";
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

            MySql.Data.MySqlClient.MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Person p = new Person();
                p.ID = reader.GetInt32(0);
                p.FirstName = reader.GetString(1);
                p.LastName = reader.GetString(2);
                p.PayRate = reader.GetFloat(3);
                p.StartDate = reader.GetDateTime(4);
                p.EndDate = reader.GetDateTime(5);
                persons.Add(p);
            }

            return persons;
        }

        public bool DeletePerson(long id)
        {
            string sqlString = "SELECT * FROM tblpersonnel WHERE ID=" + id.ToString();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

            MySql.Data.MySqlClient.MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                reader.Close();

                sqlString = "DELETE FROM tblpersonnel WHERE ID=" + id.ToString();
                cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);
                cmd.ExecuteNonQuery();

                return true;
            }
            else
            {
                return false;
            }               
        }

        public bool UpdatePerson(long id, Person p)
        {
            string sqlString = "SELECT * FROM tblpersonnel WHERE ID=" + id.ToString();
            MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);

            MySql.Data.MySqlClient.MySqlDataReader reader = null;
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                reader.Close();

                sqlString = "UPDATE tblpersonnel SET FirstName='" + p.FirstName +
                    "', LastName='" + p.LastName +
                    "', PayRate=" + p.PayRate +
                    ", StartDate='" + p.StartDate.ToString("yyyy-MM-dd HH:mm:ss") +
                    "', EndDate='" + p.EndDate.ToString("yyyy-MM-dd HH:mm:ss") +
                    "' WHERE ID=" + id.ToString();
                cmd = new MySql.Data.MySqlClient.MySqlCommand(sqlString, conn);
                cmd.ExecuteNonQuery();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}