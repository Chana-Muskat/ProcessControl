import { HttpClient } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { NetzerService } from "./netzer.service"

@Component({
  selector: 'NetzerIv',
  templateUrl: './netzerIv.component.html',
  standalone: false,
  styleUrl: './netzer.component.css'
})
export class NetzerIv implements OnInit {
  // title = 'ממשקי נצר מול הפריוריטי';
  public res:string[] = [];
  constructor(private netzerService: NetzerService) { }
  ngOnInit(): void {
    this.getAllIv();
       // throw new Error("Method not implemented.");
    }
  getAllIv() {
    this.netzerService.getInvoice().subscribe(
      (result) => {
       this.res = result;
      },
      (error) => {
        console.error('There was an error', error);
      }
    );
  } 
}
