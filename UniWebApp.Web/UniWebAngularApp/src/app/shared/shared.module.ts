import { NgModule } from '@angular/core';
import {
  MatToolbarModule, MatButtonModule, MatSidenavModule, MatIconModule,
  MatListModule, MatCardModule, MatGridListModule, MatSnackBarModule, MatTabsModule,
  MatInputModule, MatProgressSpinnerModule, MatButtonToggleModule, MatSelectModule,
  MatDatepickerModule, MatNativeDateModule, MatTooltipModule, MatDialogModule,
  MatAutocompleteModule, MatCheckboxModule
} from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { LayoutModule } from '@angular/cdk/layout';

@NgModule({
  imports: [
    FormsModule,
    ReactiveFormsModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    LayoutModule,
    FlexLayoutModule,
    MatCardModule,
    MatGridListModule,
    MatSnackBarModule,
    MatTabsModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatButtonToggleModule,
    MatSelectModule,
    MatNativeDateModule,
    MatDatepickerModule,
    MatTooltipModule,
    MatDialogModule,
    MatAutocompleteModule,
    MatCheckboxModule
  ],
  exports: [
    FormsModule,
    ReactiveFormsModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    LayoutModule,
    FlexLayoutModule,
    MatCardModule,
    MatGridListModule,
    MatSnackBarModule,
    MatTabsModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatButtonToggleModule,
    MatSelectModule,
    MatNativeDateModule,
    MatDatepickerModule,
    MatTooltipModule,
    MatDialogModule,
    MatAutocompleteModule,
    MatCheckboxModule
  ]
})
export class SharedModule { }
