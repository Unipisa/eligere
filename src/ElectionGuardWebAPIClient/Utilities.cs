using System;
using System.Net.Http;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ElectionGuard
{
    public class BigIntegerConverter : JsonConverter<BigInteger>
    {
        public override BigInteger Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var data = reader.ValueSpan;
            var s = new String(Array.ConvertAll(data.ToArray(), (b) => (char)b));
            return BigInteger.Parse(s);
        }

        // FIXME: Probably is the wrong call
        public override void Write(
            Utf8JsonWriter writer,
            BigInteger bigIntegerValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(bigIntegerValue.ToString());
    }

    internal static class RequestHelper
    {
        internal static async Task<RespT> PostRequest<RespT, ReqT>(string baseUrl, HttpClient client, string endpoint, ReqT ctxt)
        {
            var urlBuilder = new System.Text.StringBuilder();
            urlBuilder.Append(baseUrl != null ? baseUrl.TrimEnd('/') : "").Append(endpoint);
            var val = JsonSerializer.Serialize(ctxt);
            var content = new StringContent(val);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var resp = await client.PostAsync(urlBuilder.ToString(), content);
            if (resp.IsSuccessStatusCode)
            {
                var data = await resp.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions();
                options.Converters.Add(new BigIntegerConverter());
                var ret = JsonSerializer.Deserialize<RespT>(data, options);
                return ret;
            }

            throw new Exception("Error when invoking " + urlBuilder + ": " + resp.StatusCode + "\n\n" + resp.ReasonPhrase);
        }

        internal static async Task<RespT> PostRequest<RespT>(string baseUrl, HttpClient client, string endpoint)
        {
            var urlBuilder = new System.Text.StringBuilder();
            urlBuilder.Append(baseUrl != null ? baseUrl.TrimEnd('/') : "").Append(endpoint);

            var resp = await client.PostAsync(urlBuilder.ToString(), new StringContent(""));
            if (resp.IsSuccessStatusCode)
            {
                var data = await resp.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions();
                options.Converters.Add(new BigIntegerConverter());
                var ret = JsonSerializer.Deserialize<RespT>(data, options);
                return ret;
            }

            throw new Exception("Error when invoking " + urlBuilder + ": " + resp.StatusCode + "\n\n" + resp.ReasonPhrase);
        }

        internal static async Task<RespT> GetRequest<RespT>(string baseUrl, HttpClient client, string endpoint)
        {
            var urlBuilder = new System.Text.StringBuilder();
            urlBuilder.Append(baseUrl != null ? baseUrl.TrimEnd('/') : "").Append(endpoint);

            var resp = await client.GetAsync(urlBuilder.ToString());
            if (resp.IsSuccessStatusCode)
            {
                var data = await resp.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions();
                options.Converters.Add(new BigIntegerConverter());
                var ret = JsonSerializer.Deserialize<RespT>(data, options);
                return ret;
            }

            throw new Exception("Error when invoking " + urlBuilder + ": " + resp.StatusCode + "\n\n" + resp.ReasonPhrase);
        }

    }
}