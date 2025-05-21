import { ChangeDetectorRef, Component, Input, ViewContainerRef } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { RamInvoices } from '../../../models/RamInvoices';
import { StagesDialogComponent } from '../ram-stagesIv-dialog/ram-stagesIv-dialog.component';
import { RamdorInvService } from '../../../services/ramdorInv.service';

@Component({
  selector: 'ram-iv-list',
  templateUrl: './ram-iv-list.component.html',
  styleUrls: ['./ram-iv-list.component.css'],
  standalone: false
})
export class RamIvList {

  @Input() invoices: RamInvoices[] = [];

  constructor(
    
    private dialog: MatDialog, private ramInvService: RamdorInvService, private viewContainerRef: ViewContainerRef
  ) { }

  showStages(invoiceId: number) {
    console.log("Opening modal for invoice ID:", invoiceId);
    this.ramInvService.getInvoiceStages(invoiceId).subscribe(stages => {
      console.log("Fetched stages:", stages);
      this.dialog.open(StagesDialogComponent, {
        width: '800px',
        height: '380px',
        viewContainerRef: this.viewContainerRef,
        data: { stages }
      });
    });
    }
    getRowClass(statusId: number): string {
      if (statusId === 3) return 'error-row';
      //if (statusId === 1) return 'in-process-row';
      //if (statusId === 2) return 'done-row';
      return '';
  }
  markAsCompleted(invoice: RamInvoices, event: MouseEvent) {
    event.stopPropagation(); // זה עוצר את האירוע של השורה מיד
    this.ramInvService.markInvoiceAsDone({ invoiceId: invoice.invoiceId }).subscribe({
      next: (res) => {
        invoice.statusIv = 2;
        invoice.statusDescription = 'הסתיים';
        alert('הסטטוס עודכן בהצלחה');
      },
      error: (err) => {
        console.error(err);
        alert('שגיאה בעדכון הסטטוס');
      }
    });
  }
}
