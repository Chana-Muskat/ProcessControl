namespace ProcessControl.Server.Models.Ramdor
{
    public class RamUpdateStage
    {
        public string WorkNum { get; set; }
        public string Version { get; set; }
        public int StageNum { get; set; }
        public string IvRequest { get; set; }
        public string Type { get; set; } // Type of the stage, e.g., "Invoice", "Order", etc.


    }
}
