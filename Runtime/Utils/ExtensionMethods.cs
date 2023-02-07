using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using UnityEngine.Networking;

namespace OpenAI
{
    public static class ExtensionMethods
    {
        /// <summary>
        ///     Read a PNG file and add it to this form.
        /// </summary>
        /// <param name="form">This form.</param>
        /// <param name="path">Path of the file to read.</param>
        /// <param name="name">Name of the form field.</param>
        public static void AddImage(this MultipartFormDataContent form, string path, string name)
        {
            if (path != null)
            {
                var imageContent = new ByteArrayContent(File.ReadAllBytes(path));
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png");
                form.Add(imageContent, name, $"{name}.png");
            }
        }
        
        /// <summary>
        ///     Read a JSONL file and add it to this form.
        /// </summary>
        /// <param name="form">This form.</param>
        /// <param name="path">Path of the file to read.</param>
        /// <param name="name">Name of the form field.</param>
        public static void AddJsonl(this MultipartFormDataContent form, string path, string name)
        {
            if (path != null)
            {
                var fileContent = new ByteArrayContent(File.ReadAllBytes(path));
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                form.Add(fileContent, name, Path.GetFileName(path));
            }
        }
        
        /// <summary>
        ///     Add a primitive value to the form.
        /// </summary>
        /// <param name="form">This form.</param>
        /// <param name="value">Value of the form field.</param>
        /// <param name="name">Name of the form field.</param>
        public static void AddValue(this MultipartFormDataContent form, object value, string name)
        {
            if (value != null)
            {
                form.Add(new StringContent(value.ToString()), name);
            }
        }
        
        // TODO: summary
        public static void AddValue(this List<IMultipartFormSection> form, object value, string name)
        {
            if (value != null)
            {
                form.Add(new MultipartFormDataSection(value.ToString(), name));
            }
        }
        
        // TODO: summary
        public static void AddImage(this List<IMultipartFormSection> form, string path, string name)
        {
            if (path != null)
            {
                var bytes = File.ReadAllBytes(path);
                form.Add(new MultipartFormFileSection(name, bytes, $"{name}.png", "image/png"));
            }
        }
        
        // TODO: summary
        public static void AddJsonl(this List<IMultipartFormSection> form, string path, string name)
        {
            if (path != null)
            {
                var bytes = File.ReadAllBytes(path);
                form.Add(new MultipartFormFileSection(name, bytes, $"{name}.jsonl", "application/jsonl"));
            }
        }

        /// <summary>
        ///     Set headers of the HTTP request with user credentials.
        /// </summary>
        /// <param name="client">this HttpClient</param>
        /// <param name="configuration">Configuration file that contains user credentials.</param>
        /// <param name="type">The value of the Accept header for an HTTP request.</param>
        public static void SetHeaders(this HttpClient client, Configuration configuration, string type)
        {
            if (configuration.Auth.Organization != null)
            {
                client.DefaultRequestHeaders.Add("OpenAI-Organization", configuration.Auth.Organization);
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration.Auth.ApiKey);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(type));
        }

        // TODO: summary
        public static void SetHeaders(this UnityWebRequest request, Configuration configuration, string type)
        {
            request.SetRequestHeader("Content-Type", type);
            request.SetRequestHeader("OpenAI-Organization", configuration.Auth.Organization);
            request.SetRequestHeader("Authorization", "Bearer " + configuration.Auth.ApiKey);
        }
    }
}
