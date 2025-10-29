
import { Component, OnDestroy, OnInit } from "@angular/core";
import { RamdorInvService } from "../../services/ramdorInv.service";
import { RamInvoices } from "../../models/RamInvoices";
import { publish, Subscription } from "rxjs";
import { debounceTime, Subject, switchMap } from 'rxjs';
import { NgxSpinnerService } from "ngx-spinner";

@Component({
  selector: 'RamdorOrd',
  templateUrl: './ramdorOrd.component.html',
  standalone: false,
  styleUrl: './ramdorOrd.component.css'
})
export class RamdorOrd implements OnInit, OnDestroy {
  private subscription!: Subscription;
  invoices: RamInvoices[] = [];
  public res: string[] = [];
  page: number = 1;
  pageb: number = 1;
  pagec: number = 1;
  pageSize: number = 5;
  filters = { searchTerm: '', selectedDate: '', selectedStatus: '' };
  filteredInvoices: RamInvoices[] = [];
  private searchSubject = new Subject<any>();
  isLoading: boolean = false; // משתנה לניהול מצב טעינה
  constructor(private ramInvService: RamdorInvService, private spinner: NgxSpinnerService) { }
  ngOnInit(): void {
    this.searchSubject.pipe(
      debounceTime(500), // עיכוב קטן לפני שליחת בקשה למניעת שליחת בקשות מרובות
      switchMap(filters => this.ramInvService.getOrders(filters, this.page, this.pageSize))
    ).subscribe(data => {
      this.invoices = data; // טוען נתונים מהשרת
      this.filteredInvoices = this.invoices;
    });
    // this.getIv();
    this.getAllIv();

  }
  getIv() {
    this.subscription = this.ramInvService.getInvoice().subscribe(
      (result) => {
        this.res = result;
      },
      (error) => {
        console.error('There was an error', error);
      }
    );
  }


  getAllIv() {
    this.isLoading = true;
    this.spinner.show(); // הצגת האנימציה
    console.log("Fetching orders with page:", this.page, "pageSize:", this.pageSize);
    this.subscription = this.ramInvService.getOrders(this.filters, this.page, this.pageSize).subscribe(data => {
      if (this.page === 1) {
        this.invoices = data; // טעינה ראשונית
      } else {
        this.invoices = [...this.invoices, ...data]; // טעינת עוד נתונים
      }
      this.filteredInvoices = this.invoices;
      this.isLoading = false;
      this.spinner.hide(); // הסתרת האנימציה
      // this.page++;
    },
      error => {
        console.error("Error loading orders", error);
        this.isLoading = false;
        this.spinner.hide(); // הסתרת האנימציה// סיום טעינה גם במקרה של שגיאה
      });
  }
  loadMore() {
    this.page++; // מגדילים את מספר העמודים
    console.log("Loading more data, page:", this.page); // בדיקת דיבאג
    this.getAllIv();
  }
  onFilterChange(filters: any) {
    this.filters = filters; // שומר את המסנן לכל טעינה מחדש
    console.log("Filter applied and sent to server:", filters);
    this.page = 1; // איפוס הדף כדי שהחיפוש יתחיל מההתחלה
    this.invoices = []; // איפוס הרשימה כדי לא להציג תוצאות ישנות
    this.searchSubject.next(filters); // שליחת חיפוש עם עיכוב קטן
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
