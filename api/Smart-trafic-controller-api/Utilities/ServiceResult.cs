using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_trafic_controller_api.Utilities
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = null!;
        public string ErrorCode { get; set; } = null!;
        public T Data { get; set; } = default!;
        public static ServiceResult<T> Ok(T data) => new ServiceResult<T> { Success = true, Data = data };
        public static ServiceResult<T> Fail(string error, string code = null!) => new ServiceResult<T> { Success = false, ErrorMessage = error, ErrorCode = code };
    }
}