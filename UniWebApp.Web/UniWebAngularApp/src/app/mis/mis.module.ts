import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../shared/shared.module';
import { MisRoutingModule } from './mis-routing.module';

import { MisComponent } from './mis.component';
import { UpdateSectionNameDialogComponent } from './dialogs/update-section-name-dialog.component';
import { MoveFieldToSectionDialogComponent } from './dialogs/move-field-to-section-dialog.component';
import { AddFieldDialogComponent } from './dialogs/add-field-dialog.component';
import { AddEntityDialogComponent } from './dialogs/add-entity-dialog.component';
import { ConfirmDeleteEntityDialogComponent } from './dialogs/confirm-delete-entity-dialog.component';

@NgModule({
  declarations: [
    MisComponent,
    UpdateSectionNameDialogComponent,
    MoveFieldToSectionDialogComponent,
    AddFieldDialogComponent,
    AddEntityDialogComponent,
    ConfirmDeleteEntityDialogComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    MisRoutingModule
  ],
  entryComponents: [
    UpdateSectionNameDialogComponent,
    MoveFieldToSectionDialogComponent,
    AddFieldDialogComponent,
    AddEntityDialogComponent,
    ConfirmDeleteEntityDialogComponent
  ]
})
export class MisModule { }
