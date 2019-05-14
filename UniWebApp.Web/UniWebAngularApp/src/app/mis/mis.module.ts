import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MisRoutingModule } from './mis-routing.module';
import { MisComponent } from './mis.component';

@NgModule({
  declarations: [MisComponent],
  imports: [
    CommonModule,
    MisRoutingModule
  ]
})
export class MisModule { }
