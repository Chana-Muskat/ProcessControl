import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environment';  // ייבוא קובץ הסביבה

@Injectable({
  providedIn: 'root'
})
export class NetzerService {
  private apiUrl = `${environment.apiBaseUrl}/netzer`;  // השתמש ב-API מתוך קובץ הסביבה

  constructor(private http: HttpClient) { }

  // בקשה לקבלת חשבונית לפי מזהה
  getInvoiceById(invoiceId: number): Observable<any> {
    const params = new HttpParams().set('invoiceId', invoiceId.toString());

    return this.http.get<any>(`${this.apiUrl}/getInvoice`, { params });
  }

  getInvoice(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }
}
