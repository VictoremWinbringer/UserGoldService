using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserGoldService.Entities
{
   public enum ErrorKind
    {
       None,
       NotValidToken,
       Message,
    }

   public struct Result<T>
    {
        public Result(T value, ErrorKind errorKind, string errorMessage)
        {
            this.value = value;
            this.errorKind = errorKind;
            this.errorMessage = errorMessage;
        }
        public T value;
        public ErrorKind errorKind;
        public string errorMessage;
    }
}
