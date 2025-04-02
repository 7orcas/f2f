using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class _ResponseDto
    {
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public object Result { get; set; }

    }
}
