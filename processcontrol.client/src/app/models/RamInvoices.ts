import { RamStagesPerIv } from "./RamStagesPerIv";
import { StatusIv } from "./StatusIv";

export class RamInvoices {
  invoiceId!: number;
  workNum!: string;
  startDate!: Date;
  version!: string;
  statusIv!: number;
  statusDescription!: string;

  statusIvId!: StatusIv[];
  

}
