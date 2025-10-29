import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Netzer } from './components/netzer/netzer.component';
import { NetzerIv } from './components/netzer/netzerIv.component';
import { Ramdor } from './pages/ramdor/ramdor.component';
import { RamdorIv } from './pages/ramdorIv/ramdorIv.component';
import { AppComponent } from './app.component';
import { RamdorOrd } from './pages/ramdorOrd/ramdorOrd.component';

const routes: Routes = [
  { path: 'netzer', component: Netzer },
  {
    path: 'ramdor', component: Ramdor,
    children: [
      { path: 'ramdorIv', component: RamdorIv },
      { path: 'ramdorOrd', component: RamdorOrd }
    ]
  },
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
