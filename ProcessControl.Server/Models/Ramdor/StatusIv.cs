using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace ProcessControl.Server.Models.Ramdor
{
    public class StatusIv
    {
        [Key]
        public int StatusIvId { get; set; }
        public int? SCode { get; set; }
        public string? SDes { get; set; }
        public virtual ICollection<RamInvoices>? RamInvoice { get; set; }
        //public StatusIv(int statusIvId, int sCode, string sDes)
        //{
        //    StatusIvId = statusIvId;
        //    SCode = sCode;
        //    SDes = sDes;
        //}
        public StatusIv() { }
    }
}
