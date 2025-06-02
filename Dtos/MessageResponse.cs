using System.Runtime.Intrinsics;
using api.Controllers;

namespace api.Dtos
{
    public class MessageResponse
    {
        public string title { get; set; }
        public MessageResponse(string message) => title = message;
    }
}