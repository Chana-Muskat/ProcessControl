import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environment';  // ייבוא קובץ הסביבה
import { RamInvoices } from '../models/RamInvoices';
import { RamIvStages } from '../models/RamIvStages';
import { RamStagesPerIv } from '../models/RamStagesPerIv';

@Injectable({
  providedIn: 'root'
})
export class RamdorInvService {
  private apiUrl = `${environment.apiBaseUrl}/RamIv`;  // השתמש ב-API מתוך קובץ הסביבה

  constructor(private http: HttpClient) { }

  // בקשה לקבלת חשבונית לפי מזהה
  getInvoiceById(invoiceId: number): Observable<any> {
    const params = new HttpParams().set('invoiceId', invoiceId.toString());

    return this.http.get<any>(`${this.apiUrl}/getInvoice`, { params });
  }

 /* getInvoices(page: number, pageSize: number = 30): Observable<RamInvoices[]> {
    return this.http.get<RamInvoices[]>(`${this.apiUrl}/getInvoices?page=${page}&pageSize=${pageSize}`);*/

  getInvoices(filters: any = { }, page: number, pageSize: number): Observable<RamInvoices[] > {
      let params = new HttpParams()
        .set('skip', ((page - 1) * pageSize).toString())
        .set('take', pageSize.toString());

      if(filters.searchTerm) {
      params = params.set('searchTerm', filters.searchTerm);
    }
    if (filters.selectedDate) {
      params = params.set('selectedDate', filters.selectedDate);
    }
    if (filters.selectedStatus) {
      params = params.set('selectedStatus', filters.selectedStatus);
    }
    console.log("Sending request with params:", params.toString());
    return this.http.get<RamInvoices[]>(`${this.apiUrl}/getInvoices`, { params });
  }
  

  getInvoice(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }

  getInvoiceStages(invoiceId: number): Observable<RamStagesPerIv[]> {
    return this.http.get<RamStagesPerIv[]>(`${this.apiUrl}/getIvStages/${invoiceId}/stages`);
  }
  markInvoiceAsDone(body: { invoiceId: number }) {
    return this.http.post(`${this.apiUrl}/markAsDone`, body);
  }
}
