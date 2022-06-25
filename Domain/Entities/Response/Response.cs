using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Web;

namespace Domain.Entities
{
    public class Response : ActionResult, IActionResult
    {
        public int Code { get; set; } = 0;
        public int Type { get; set; } = 0;
        public string Message { get; set; } = "";
        protected int? StatusCode { get; set; } = 200;

        public Response()
        {
            this.Code = 0;
            this.Type = -1;
            this.Message = "Unknown error";
        }

        public Response(int code, int type, string message)
        {
            this.Code = code;
            this.Type = type;
            this.Message = message;
        }

        public Response(int code, int type)
        {
            this.Code = code;
            this.Type = type;
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            if (this.StatusCode.HasValue)
            {
                context.HttpContext.Response.StatusCode = this.StatusCode.Value;
            }
            return base.ExecuteResultAsync(context);
        }

        public void SetStatusCode(int code)
        {
            this.StatusCode = code;
        }

        public Response Success() => new(0, (int)ResponseTypes.Success);
        public Response Failure(string message) => new(0, (int)ResponseTypes.Failure, message);

        public Response NotFound(string message)
        {
            var response = new Response(0, (int)ResponseTypes.Failure, message);
            response.SetStatusCode(404);
            return response;
        }

        public Response AccessDenied(string message)
        {
            var response = new Response(0, (int)ResponseTypes.Failure, message);
            response.SetStatusCode(403);
            return response;
        }
    }

    public class Response<T> : Response
    {
        public Response()
        {
            this.Code = 0;
            this.Type = -1;
            this.Message = "Unknown error";
        }
        public Response(int code, int type, T payload)
        {
            this.Code = code;
            this.Type = type;
            this.Payload = payload;
        }

        public Response(int code, int type, string message)
        {
            this.Code = code;
            this.Type = type;
            this.Message = message;
        }

        public Response(int code, int type)
        {
            this.Code = code;
            this.Type = type;
        }

        public Response<T> Success(T payload) => new(0, (int)ResponseTypes.Success, payload);
        public new Response<T> Failure(string message) => new(0, (int)ResponseTypes.Failure, message);
        public T Payload { get; set; } = default;

        public new Response<T> AccessDenied(string message)
        {
            var response = new Response<T>(0, (int)ResponseTypes.Failure, message);
            response.SetStatusCode(403);
            return response;
        }

        public new Response<T> NotFound(string message)
        {
            var response = new Response<T>(0, (int)ResponseTypes.Failure, message);
            response.SetStatusCode(404);
            return response;
        }

    }
}
