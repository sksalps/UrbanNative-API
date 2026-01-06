using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.GlobalCall.VariantValueSignature
{
    public interface IValueSignatureDecoder
    {
        string Decode(string? valueSignature);
    }
}

