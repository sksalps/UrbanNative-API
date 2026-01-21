using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.Common
{


    public class ChangePasswordRequestDto
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
    public enum ChangePasswordResult
    {
        Success = 1,
        InvalidOldPassword = -1,
        SamePassword = -2,
        InvalidInput = -3,
        Failed = -99
    }

    public class ChangePasswordResponseDto
    {
        public bool IsSuccess { get; set; }
        public ChangePasswordResult Result { get; set; }
        public string Message { get; set; } = string.Empty;
    }

}
