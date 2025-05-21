import { HttpClient } from "@angular/common/http";
import { Component } from "@angular/core";

@Component({
  selector: 'Ramdor',
  templateUrl: './ramdor.component.html',
  standalone: false,
  styleUrl: './ramdor.component.css'
})
export class Ramdor {
  title = 'ממשקי רמדור מול הפריוריטי';
  constructor(private http: HttpClient) { }
  selectedButton: string = 'חשבוניות'; // אפשר להתחיל עם כפתור נבחר

  selectButton(button: string) {
    this.selectedButton = button;
  }
}
