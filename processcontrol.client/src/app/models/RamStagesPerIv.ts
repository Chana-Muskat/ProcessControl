import { RamInvoices } from "./RamInvoices";
import { RamIvStages } from "./RamIvStages";

export class RamStagesPerIv {
  stageIv!: number;
  invoiceIdf!: number;
  stageIdf!: number;
  resStatus!: string;
  uDate!: Date;
  errDes!: string;
  ivRequest!: string;
  stageDes!: string;

  stageId!: RamIvStages[];
  invoiceId!: RamInvoices[];
}
