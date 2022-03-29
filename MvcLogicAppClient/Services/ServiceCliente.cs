using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeguridadApiAlumnosPractica.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MvcLogicAppClient.Services
{
    public class ServiceCliente
    {
        private string UrlApi;
        private MediaTypeWithQualityHeaderValue Header;

        public ServiceCliente(string urlapi)
        {
            this.UrlApi = urlapi;
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }

        public async Task<string> GetTokenAsync(string nombre, string apellidos)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                LoginModel model = new LoginModel
                {
                    Nombre = nombre,
                    Apellidos = apellidos
                };
                string json = JsonConvert.SerializeObject(model);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                string request = "/api/auth/ValidarAlumno";
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    JObject jObject = JObject.Parse(data);
                    string token = jObject.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }

        internal async Task CreateAlumno(int idAlumno, string curso, string nombre, string apellidos, int nota, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                string request = "/api/alumnos";

                Alumno al = new Alumno { IdAlumno = idAlumno, Curso = curso, Nombre = nombre, Apellidos = apellidos, Nota = nota };

                string json = JsonConvert.SerializeObject(al);

                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(request, content);
            }
        }

        public async Task DeleteAlumno(int id, string token)
        {
            string request = "api/alumnos/" + id;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                HttpResponseMessage response = await client.DeleteAsync(request);
            }
        }

        public async Task UpdateAlumno(int idalumno, string curso, string nombre, string apellidos, int nota, string token)
        {
            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                string request = "/api/alumnos";

                Alumno al = await this.GetAlumnoAsync(idalumno, token);

                if (al != null)
                {

                    al.Curso = curso;
                    al.Nombre = nombre;
                    al.Apellidos = apellidos;
                    al.Nota = nota;

                    string json = JsonConvert.SerializeObject(al);

                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync(request, content);

                }
            }
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        //METODO CON SEGURIDAD QUE RECIBE EL TOKEN
        private async Task<T> CallApiAsync<T>(string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }

            }
        }

        public async Task<List<Alumno>>
            GetAlumnosAsync(string token)
        {
            string request = "/api/alumnos";
            List<Alumno> alumnos =
                await this.CallApiAsync<List<Alumno>>(request, token);
            return alumnos;
        }

        public async Task<Alumno> GetAlumnoAsync(int id, string token)
        {
            string request = "/api/alumnos/" + id;
            Alumno alumno = await this.CallApiAsync<Alumno>(request,token);
            return alumno;
        }
        public async Task<Alumno> GetPerfilAsync(string token)
        {
            string request = "/api/alumnos/perfilusuario";
            Alumno alumno = await this.CallApiAsync<Alumno>(request, token);
            return alumno;
        }
        public async Task<List<Alumno>> GetAlumnosFlowAsync()
        {
            string urlFlowAlumnos = "https://prod-208.westeurope.logic.azure.com:443/workflows/0c43badc187f4776963a6ea443b4a394/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=8VyfiXPm9hiUTY9hkcAfdq4aX2bY-ZMVGQL6IWcout8";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.PostAsync(urlFlowAlumnos, null);
                if (response.IsSuccessStatusCode)
                {
                    List<Alumno> alumnos = await response.Content.ReadAsAsync<List<Alumno>>();
                    return alumnos;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
