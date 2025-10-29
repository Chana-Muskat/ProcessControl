using System.ComponentModel.DataAnnotations;

namespace ProcessControl.Server.Models.Ramdor
{
    public class RamInvoices
    {
        [Key]
        public int InvoiceId { get; set; }
        public string? WorkNum { get; set; }
        public DateTime? StartDate { get; set; }
        public string? Version { get; set; }
        public string? Type { get; set; }
        public int? StatusIv { get; set; }
        public virtual StatusIv? StatusIvId { get; set; }
        public virtual ICollection<RamStagesPerIv>? RamStagePerIv { get; set; }

        //public RamInvoices(int invoiceId, string workNum, DateTime startDate, int statusIv, StatusIv statusIvId)
        //{
        //    InvoiceId = invoiceId;
        //    WorkNum = workNum;
        //    StartDate = startDate;
        //    this.StatusIv = statusIv;
        //}
        public RamInvoices() { }
    }
}
