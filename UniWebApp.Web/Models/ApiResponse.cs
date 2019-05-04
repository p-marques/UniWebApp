using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniWebApp.Web.Models
{
    public class ApiResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }

        public ApiResponse() { }

        public ApiResponse(int inStatus, string inMessage)
        {
            this.Status = inStatus;
            this.Message = inMessage;
        }
    }
    public class ApiResponse<T> : ApiResponse
    {
        public T Return { get; set; }

        public ApiResponse() { }

        public ApiResponse(int inStatus, string inMessage, T inReturn)
        {
            this.Status = inStatus;
            this.Message = inMessage;
            this.Return = inReturn;
        }
    }
}
