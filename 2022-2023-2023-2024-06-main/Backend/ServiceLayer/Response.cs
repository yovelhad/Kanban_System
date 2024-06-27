using System.Text.Json;
using System.Text.Json.Serialization;
namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Response
    {
        ///<summary>Class <c>Response</c> represents the result of a call to a void function. 
        ///If an exception was thrown, <c>ErrorOccured = true, ReturnValue = null</c> and <c>ErrorMessage != null</c>. 
        ///Otherwise, <c>ErrorOccured = false</c> and <c>ErrorMessage = null</c>.</summary>
        [JsonInclude]
        public string ErrorMessage { get; set; }
        [JsonInclude]
        public object ReturnValue { get; set; }
        public bool ErrorOccured
        {
            get => ErrorMessage != null;
        }
        public Response() { }
        public Response(string msg, object returnValue)
        {
            ErrorMessage = msg;
            ReturnValue = returnValue;
        }
    }
}
