# Preparing
In VS, click `Tools\NuGet Package Manager\Package Manager Console` to open the console, then type in: `Install-Package Microsoft.AspNet.WebApi.Client` to install neccessary dependencies for the client development;  

# Models
Replicate the models at the server side to the client:
```C#
namespace ConsoleRestClient
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

# Complete Code
```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ConsoleRestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:58036/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));;
                client.DefaultRequestHeaders.Add("APIKey", "whencanifinishthisgame");

                HttpResponseMessage response;

                /////////////POST////////////////////
                Console.WriteLine("POST");

                Person newPerson = new Person();
                newPerson.FirstName = "Frodo";
                newPerson.LastName = "Frodo";
                newPerson.PayRate = 40;
                newPerson.StartDate = DateTime.Parse("12/12/1990");
                newPerson.EndDate = DateTime.Parse("11/11/2012");

                try
                {
                    response = await client.PostAsJsonAsync("api/person", newPerson);

                    if (response.IsSuccessStatusCode)
                    {
                        Uri personUrl = response.Headers.Location;
                        Console.WriteLine("Person Location is: " + personUrl);
                    }
                    else
                    {
                        Console.WriteLine(response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                ///////////PUT AND DELETE///////////////////////////
                Console.WriteLine("PUT and DELETE");

                Person personToUpdate = new Person();
                personToUpdate.FirstName = "ToUpdate";
                personToUpdate.LastName = "ToUpdate";
                personToUpdate.PayRate = 13.33;
                personToUpdate.StartDate = DateTime.Parse("10/10/1980");
                personToUpdate.EndDate = DateTime.Parse("1/1/2000");
                try
                {
                    response = await client.PostAsJsonAsync("api/person", personToUpdate);
                    Uri personUrl = response.Headers.Location;
                    if (response.IsSuccessStatusCode)
                    {
                        personToUpdate.PayRate = 14.44;
                        response = await client.PutAsJsonAsync(client.BaseAddress.MakeRelativeUri(personUrl), personToUpdate);
                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Put Successful");

                            response = await client.DeleteAsync(client.BaseAddress.MakeRelativeUri(personUrl));
                            if (response.IsSuccessStatusCode)
                            {
                                Console.WriteLine("Deleted");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Put Not Successful");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Post Not Successful before PUT");
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("ERROR: " + ex.Message);
                }

                ////////////GET//////////////////////////
                Console.WriteLine("GET");
                try
                {
                    response = await client.GetAsync("api/Person/4");
                    if (response.IsSuccessStatusCode)
                    {
                        Person person = await response.Content.ReadAsAsync<Person>();
                        Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", person.FirstName, person.LastName, person.PayRate, person.StartDate, person.EndDate);
                    }
                    else 
                    {
                        Console.WriteLine(response.StatusCode);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("ERROR: " + ex.Message);
                }

                ////////////GET EVERYONE//////////////////
                Console.WriteLine("GET EVERYONE");
                try
                {
                    response = await client.GetAsync("api/Person");
                    if (response.IsSuccessStatusCode)
                    {
                        List<Person> persons = await response.Content.ReadAsAsync<List<Person>>();
                        for(int i = 0; i < persons.Count; i++)
                        {
                            Person person = persons[i];
                            Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", person.FirstName, person.LastName, person.PayRate, person.StartDate, person.EndDate);
                        }
                    }
                    else
                    {
                        Console.WriteLine(response.StatusCode);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("ERROR: " + ex.Message);
                }
            }
        }
    }
}
```
This client example is coorperating with the service that is secured with APIKey, so I added a header of APIKey;