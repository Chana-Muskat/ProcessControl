using System.ComponentModel.DataAnnotations;

namespace ProcessControl.Server.Models.Ramdor
{
    public class RamStagesPerOrd
    {
        [Key]
        public int StageOrd { get; set; }
        public int StageIdf { get; set; }
        public int OrderIdf { get; set; }
      
        public string? ResStatus { get; set; }
        public DateTime? UDate { get; set; }
        public string? ErrDes { get; set; }
        public virtual RamOrdStages StageId { get; set; }
        public virtual RamOrders OrderId { get; set; }
        public RamStagesPerOrd() { }
    }

}
