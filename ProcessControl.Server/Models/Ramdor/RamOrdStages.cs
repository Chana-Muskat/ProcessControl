using System.ComponentModel.DataAnnotations;

namespace ProcessControl.Server.Models.Ramdor
{
    public class RamOrdStages
    {
        [Key]
        public int StageId { get; set; }
        public int StageNum { get; set; }
        public string StageDes { get; set; }
        public string Operator { get; set; }
        public virtual ICollection<RamStagesPerOrd> RamStagePerOrd { get; set; }

        public RamOrdStages() { }
    }
}

