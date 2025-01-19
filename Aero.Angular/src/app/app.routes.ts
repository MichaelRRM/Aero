import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { WorkersComponent } from './workers/workers.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'workers',
    component: WorkersComponent
  },
  {
    path: '**',
    redirectTo: ''
  }
];
