using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;

namespace UsersMicroservice.Logs
{
    public class Error
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public string Url { get; set; }
    }

    public class JsonErrorResponses
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Result { get; set; }
        public bool Success { get; set; }
        public Error ErrorContainer { get; set; }
    }

    public sealed class ResponsesContainer
    {
        private static ResponsesContainer SingletonInstance = null;
        private static readonly object Lock = new object();

        public readonly string githubUrl = "https://github.com/marskr/UserMicroservice/tree/master";
        public string GetResponseContent(HttpStatusCode response, string result_s, bool success_b, 
                                         int errorCode_i, string errorMessage_s, 
                                         string errorMessageDetails_s)
        {
            JsonErrorResponses _error = new JsonErrorResponses
            {
                StatusCode = response,
                Result = result_s,
                Success = success_b,
                ErrorContainer = new Error
                {
                    Code = errorCode_i,
                    Message = errorMessage_s,
                    Details = errorMessageDetails_s,
                    Url = githubUrl
                }
            };
            JObject o = (JObject)JToken.FromObject(_error);

            return o.ToString(); 
        }


        public static ResponsesContainer Instance
        {
            get
            {
                lock (Lock)
                {
                    if (SingletonInstance == null)
                    {
                        SingletonInstance = new ResponsesContainer();
                    }
                    return SingletonInstance;
                }
            }
        }
    }
}
