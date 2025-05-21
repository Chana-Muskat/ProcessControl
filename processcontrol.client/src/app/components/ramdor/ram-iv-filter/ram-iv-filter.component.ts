import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'ram-iv-filter',
  templateUrl: './ram-iv-filter.component.html',
  styleUrls: ['./ram-iv-filter.component.css'],
  standalone: false
})
export class RamIvFilter {
  searchTerm: string = '';
  selectedDate: string = '';
  selectedStatus: string = '';

  statusOptions: string[] = ['בתהליך', 'שגיאה', 'הסתיים'];

  @Output() filterChanged = new EventEmitter<any>(); // אירוע שמעביר את החיפוש להורה

  applyFilter() {
    console.log("🔍 פילטר נשלח:", this.searchTerm, this.selectedDate, this.selectedStatus);
    this.filterChanged.emit({
      searchTerm: this.searchTerm,
      selectedDate: this.selectedDate,
      selectedStatus: this.selectedStatus
    });
  }

  resetFilters() {
    this.searchTerm = '';
    this.selectedDate = '';
    this.selectedStatus = '';
    this.applyFilter(); // שולח חזרה את כל הנתונים כדי להציג את כל החשבוניות
  }
}
