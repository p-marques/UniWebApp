import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MisComponent } from './mis.component';

const routes: Routes = [
  { path: 'mis', component: MisComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MisRoutingModule { }
