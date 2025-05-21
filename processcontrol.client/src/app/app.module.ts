import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { Netzer } from './components/netzer/netzer.component';
import { NetzerIv } from './components/netzer/netzerIv.component';
import { Ramdor } from './pages/ramdor/ramdor.component';
import { providePrimeNG } from 'primeng/config';
import { ButtonModule } from 'primeng/button';
import { RamdorIv } from './pages/ramdorIv/ramdorIv.component';
import { RamIvList } from './components/ramdor/ram-iv-list/ram-iv-list.component';
import { StagesDialogComponent } from './components/ramdor/ram-stagesIv-dialog/ram-stagesIv-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { RamIvFilter } from './components/ramdor/ram-iv-filter/ram-iv-filter.component';
import { FormsModule } from '@angular/forms';
import { NgxSpinnerModule } from "ngx-spinner";


@NgModule({
  declarations: [
    AppComponent, Netzer, NetzerIv, Ramdor, RamdorIv, RamIvList, RamIvFilter
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule, ButtonModule, MatDialogModule, CommonModule, FormsModule,
    NgxSpinnerModule
  ],
  providers: [
    providePrimeNG()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
