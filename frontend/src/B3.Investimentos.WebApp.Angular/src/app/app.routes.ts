import { Routes } from '@angular/router';
import { BoasVindasComponent } from './features/boas-vindas/boas-vindas/boas-vindas.component';

export const routes: Routes = [
  { path: 'boas-vindas', component: BoasVindasComponent },
  { path: '', redirectTo: '/boas-vindas', pathMatch: 'full' }
];
