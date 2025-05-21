import { HttpClient } from "@angular/common/http";
import { Component } from "@angular/core";

@Component({
  selector: 'Netzer',
  templateUrl: './netzer.component.html',
  standalone: false,
  styleUrl: './netzer.component.css'
})
export class Netzer {
  title = 'ממשקי נצר מול הפריוריטי';
  constructor(private http: HttpClient) { }
}
