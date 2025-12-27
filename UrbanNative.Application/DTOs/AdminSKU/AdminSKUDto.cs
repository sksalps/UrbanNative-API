using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanNative.Application.DTOs.AdminSKU
{
    public class VariantSelectionDto
    {
        public int VariantId { get; set; }
        
        public List<int> VariantValueIds { get; set; } = new();
}

public class SkuSignatureDto
    {
        public int SeqNo { get; set; }
        public string ValueSignature { get; set; } = string.Empty;
    }

}
