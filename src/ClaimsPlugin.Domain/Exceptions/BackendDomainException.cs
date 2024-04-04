// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Runtime.Serialization;
// using System.Threading.Tasks;

// namespace ClaimsPlugin.Domain.Exceptions
// {
//     [Serializable]
//     public class BackendDomainException
//     {
//         public string Code { get; private set; } = string.Empty;

//         public BackendDomainException() { }

//         public BackendDomainException(string message)
//             : base(message) { }

//         public BackendDomainException(string code, string message)
//             : base(message)
//         {
//             Code = code;
//         }

//         public BackendDomainException(string message, Exception innerException)
//             : base(message, innerException) { }

//         public BackendDomainException(string code, string message, Exception innerException)
//             : base(message, innerException)
//         {
//             Code = code;
//         }

//         protected BackendDomainException(SerializationInfo info, StreamingContext context)
//             : base(info, context) { }
//     }
// }
