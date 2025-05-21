import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

/*interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}*/

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'מסך בקרה';
  tableData: any[] = [];
  constructor(private http: HttpClient) { }

  selectedTopic: string | null = null;

  selectTopic(topic: string) {
    this.selectedTopic = topic;
  }

}
