using System.ComponentModel.DataAnnotations;

namespace ProcessControl.Server.Models.Ramdor
{
    public class RamOrders
    {
        [Key]
        public int OrderId { get; set; }
        public string? WorkNum { get; set; }
        public string? Version { get; set; }
        public DateTime? StartDate { get; set; }
        
        public int? StatusIv { get; set; }
        public virtual StatusIv? StatusIvId { get; set; }
        public virtual ICollection<RamStagesPerOrd>? RamStagePerOrd { get; set; }

        public RamOrders() { }
    }

}
