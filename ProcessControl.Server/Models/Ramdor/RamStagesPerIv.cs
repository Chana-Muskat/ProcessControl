using System.ComponentModel.DataAnnotations;

namespace ProcessControl.Server.Models.Ramdor
{
    public class RamStagesPerIv
    {
        [Key]
        public int StageIv { get; set; }
        public int InvoiceIdf {  get; set; }
        public int StageIdf { get; set; }
        public string? ResStatus { get; set; }
        public DateTime? UDate { get; set; }
        public string? ErrDes { get; set; }
        public string? IvRequest { get; set; }
        public virtual RamIvStages StageId { get; set; }
        public virtual RamInvoices InvoiceId { get; set; }
        public RamStagesPerIv() { }
    }
}
