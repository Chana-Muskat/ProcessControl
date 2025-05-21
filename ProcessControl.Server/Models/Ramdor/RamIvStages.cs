using System.ComponentModel.DataAnnotations;

namespace ProcessControl.Server.Models.Ramdor
{
    public class RamIvStages
    {
        [Key]
        public int StageId { get; set; }
        public int StageNum { get; set; }
        public string StageDes { get; set; }
        public string Operator { get; set; }
        public virtual ICollection<RamStagesPerIv> RamStagePerIv { get; set; }

        public RamIvStages() { }
    }
}
