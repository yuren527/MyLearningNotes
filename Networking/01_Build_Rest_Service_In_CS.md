# Content #
- [Content](#content)
- [Preparing Development](#preparing-development)
- [Add a Controller](#add-a-controller)
- [Create a Model](#create-a-model)
- [Create a Persistence Class](#create-a-persistence-class)
- [Finish the Code](#finish-the-code)
  - [PersonPersistence.cs](#personpersistencecs)
  - [PersonController.cs](#personcontrollercs)
- [Use Configuration File](#use-configuration-file)
- [Securing API with API Key](#securing-api-with-api-key)

# Preparing Development #
- Install C# to Visual Studio;
  
- Install Rest extension to Chrome, so we can test web api easily within Chrome, for me it is Restlet Client;  
  
- Create our project using `Visual C#\ASP.NET Web Application\Empty` template, check `WebAPI` box; To enable authentication, we should select `WebAPI` template then click `change Authentication` button to change the authentication mehtod;  
  
-  For this tutorial, we created a database containing a table named `tblpersonnel` to connect with this web API; Table description:  
    ```
    +-----------+-------------+------+-----+---------+----------------+
    | Field     | Type        | Null | Key | Default | Extra          |
    +-----------+-------------+------+-----+---------+----------------+
    | ID        | int(11)     | NO   | PRI | NULL    | auto_increment |
    | FirstName | varchar(50) | YES  |     | NULL    |                |
    | LastName  | varchar(50) | YES  |     | NULL    |                |
    | PayRate   | double      | YES  |     | NULL    |                |
    | StartDate | datetime    | YES  |     | NULL    |                |
    | EndDate   | datetime    | YES  |     | NULL    |                |
    +-----------+-------------+------+-----+---------+----------------+
    ```

- Add `MySql.Data` to References to connect to MySQL Database, but before we can do this, we should have `ConnectorNet` installed to MySQL;

# Add a Controller #
To add a new controller, right click the `Controller` folder, select Add\controller, then choose a template, for this case, we gonna use `Web API 2 Controller with Read/Write actions` template;  

Now we have a new controller class, VS add some code automatically as below:  
```C#
public class PersonController : ApiController
{
    // GET: api/Person
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET: api/Person/5
    public string Get(int id)
    {
        return "value";
    }

    // POST: api/Person
    public void Post([FromBody]string value)
    {
    }

    // PUT: api/Person/5
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE: api/Person/5
    public void Delete(int id)
    {
    }
}
```

The Comments shows the way we can request: `http://localhost:<port\>/api/{controller}/{id}`, use chrome extension to test;

# Create a Model #
Create a folder named `Models` in the project, then create a `person` class in it:  
```C#
namespace SimpleRestServer.Models
{
    public class Person
    {
        public long ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public double PayRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
```
Then include the Person Model into the controller;

# Create a Persistence Class #
We don't want to write the database query codes in the controllers or models, so, create a `PersonPersistence.cs` class to store the database manipulating code:
```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;

namespace SimpleRestServer
{
    public class PersonPersistence
    {
        private MySql.Data.MySqlClient.MySqlConnection conn;

        public PersonPersistence()
        {
            string myConnectionString;
            myConnectionString = "server=127.0.0.1;uid=root;pwd=tmwfiawc;database=employeedb";
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open(); //Open the connection
            }
            catch(MySql.Data.MySqlClient.MySqlException ex)
            {

            }
            //Once the object is constructed, the database connection is open
        }
    }
}
```

# Finish the Code
Next we should finish all the functions that are used to query the database, and all the APIs;  

## PersonPersistence.cs ##
```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using SimpleRestServer.Models;

namespace SimpleRestServer
{
    public class PersonPersistence
    {
        private MySql.Data.MySqlClient.MySqlConnection conn;

        public PersonPersistence()
        {
            string myConnectionString;
            myConnectionString = "server=127.0.0.1;uid=root;pwd=tmwfiawc;database=employeedb";
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
                 //reader is opened, needs to be closed before reassigning cmd
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
                //reader is opened, needs to be closed before reassigning cmd
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
```

## PersonController.cs ##
```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SimpleRestServer.Models;
using System.Collections;

namespace SimpleRestServer.Controllers
{
    public class PersonController : ApiController
    {
        // GET: api/Person
        public ArrayList Get()
        {
            PersonPersistence pp = new PersonPersistence();
            return pp.GetPersons();
        }

        // GET: api/Person/5
        public Person Get(int id)
        {
            PersonPersistence pp = new PersonPersistence();           
            return pp.GetPerson(id);
        }

        // POST: api/Person
        public HttpResponseMessage Post([FromBody]Person person)
        {
            PersonPersistence pp = new PersonPersistence();
            long id = pp.SavePerson(person);
            person.ID = id;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(Request.RequestUri, "person/" + id.ToString());
            return response;
        }

        // PUT: api/Person/5
        public HttpResponseMessage Put(int id, [FromBody]Person person)
        {
            PersonPersistence pp = new PersonPersistence();
            if(pp.UpdatePerson(id, person))//If the record of id exists and executed cmd
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NoContent);
                response.Headers.Location = new Uri(Request.RequestUri, "person/" + id.ToString());
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        // DELETE: api/Person/5
        public HttpResponseMessage Delete(int id)
        {
            PersonPersistence pp = new PersonPersistence();
            if (pp.DeletePerson(id)) //If the record of id exists and executed cmd
            {
                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
```

# Use Configuration File #
For security reason, we don't want our connection strings or something sensitive writen in code, write it in the configuration file;  

In `Web.config` file, add a new section in `configuration/appSettings`:  
```
<connectionStrings>
    <add name="localDB" connectionString="server=127.0.0.1;uid=root;pwd=tmwfiawc;database=employeedb">
</connectionStrings>
```
Then replace the connectionStrings in code like this:  
`myConnectionString = ConfigurationManager.ConnectionStrings["localDB"].ConnectionString;`

*Note: `connectionStrings` is with a lower case character in configuations, but in code, it comes up with a upper case in the `ConfiguationManager`, don't get it stuck you there;*

**Furthermore**, if we don't want our sensitive information upload to source control like github, we can seperate it into a new xml file, and make source control ignoring it:  
- Add a new config file named `DBConnections.config`, and copy the section added above to the new config file;
- Replace the copied section in the `Web.config` with:  
    `<connectionStrings configSource="DBConnections.config" />`  

Now, the code access to the `Web.config` file to get the connection strings, and `Web.config` get the actual strings from `DBConnections.config` file;

# Securing API with API Key #
- Create a new class derived from `DelegatingHander` and name it `APIKeyMessageHandler.cs` in a newly created folder `MessageHandlers`;  Put following code in it:  
    ```C#
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http; //Required
    using System.Threading; //Required
    using System.Threading.Tasks; //Required
    using System.Net; //Required
    using System.Web;

    namespace SimpleRestServer.MessageHandlers
    {
        public class APIKeyMessageHandler : DelegatingHandler
        {
            private const string CorrectAPIKey = "whencanifinishthisgame";

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
            {
                bool isValidKey = false;
                IEnumerable<string> requestHeaders;
                var checkApiKeyExists = httpRequestMessage.Headers.TryGetValues("APIKey", out requestHeaders);
                if(checkApiKeyExists && requestHeaders.FirstOrDefault().Equals(CorrectAPIKey))
                {
                    isValidKey = true;
                }

                if (!isValidKey)
                {
                    return httpRequestMessage.CreateResponse(HttpStatusCode.Forbidden, "Invalid API Key");
                }

                //Calling base class Method
                var response = await base.SendAsync(httpRequestMessage,    cancellationToken);
                return response;
            }
        }
    }
    ```
    If `isValidKey` is false after checking API key, then this `SendAsync` function will directly end up with a forbidden response that blocks the request, otherwise, the function will call the base function and return the result which let the request coming through; *I think this is the key of the security functionality* 

- Add a new configuration in the `Global.asax.sc` file, in the `Application_Start()` function:  
    `GlobalConfiguration.Configuration.MessageHandlers.Add(new APIKeyMessageHandler());`  
    So, everytime a request coming in, an `APIKeyMessageHandler` will be created and do the check;