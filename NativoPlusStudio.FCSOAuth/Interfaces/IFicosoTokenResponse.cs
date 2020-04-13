using System;
using System.Collections.Generic;
using System.Text;

namespace NativoPlusStudio.FCSOAuth

{
    public interface IFicosoTokenResponse : IBaseResponse
    {
        string AccessToken { get; set; }
        string ExpiresIn { get; set; }
        string TokenType { get; set; }
       
    }
}
