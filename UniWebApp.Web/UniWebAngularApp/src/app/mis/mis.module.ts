import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../shared/shared.module';
import { MisRoutingModule } from './mis-routing.module';

import { MisComponent } from './mis.component';

@NgModule({
  declarations: [MisComponent],
  imports: [
    CommonModule,
    SharedModule,
    MisRoutingModule
  ]
})
export class MisModule { }
