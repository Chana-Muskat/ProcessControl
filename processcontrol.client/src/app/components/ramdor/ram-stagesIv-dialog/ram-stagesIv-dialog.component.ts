import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { RamIvStages } from '../../../models/RamIvStages';
import { RamdorInvService } from '../../../services/ramdorInv.service';
import { RamStagesPerIv } from '../../../models/RamStagesPerIv';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-stages-dialog',
  templateUrl: './ram-stagesIv-dialog.component.html',
  styleUrls: ['./ram-stagesIv-dialog.component.css'],
  imports: [CommonModule]
  
})
export class StagesDialogComponent{
  ivStages: RamStagesPerIv[] = [];

  constructor(
    private ramInvService: RamdorInvService,
    public dialogRef: MatDialogRef<StagesDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { stages: RamStagesPerIv[] },
      
  ) {
    console.log("Received data in modal:", this.data);
    this.ivStages = this.data.stages || [];
    console.log("stages c:", this.ivStages);
}

 
  close(): void {
    this.dialogRef.close();
  }
}
