using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Response<T>
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

        public Response<T> Failure(string message) => new(0, (int)ResponseTypes.Failure, message);

        public int Code { get; set; } = 0;
        public int Type { get; set; } = 0;

        public string Message { get; set; } = "";
        public T Payload { get; set; } = default;

    }
}
