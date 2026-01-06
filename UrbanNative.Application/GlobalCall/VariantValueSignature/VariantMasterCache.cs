using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.GlobalCall.VariantValueSignature
{
    public class VariantMasterCache
    {
        public Dictionary<int, string> VariantNames { get; }
        public Dictionary<int, string> VariantValueNames { get; }

        public VariantMasterCache(
            Dictionary<int, string> variantNames,
            Dictionary<int, string> variantValueNames)
        {
            VariantNames = variantNames;
            VariantValueNames = variantValueNames;
        }
    }

}


