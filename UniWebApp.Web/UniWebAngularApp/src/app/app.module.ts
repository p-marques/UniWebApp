import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { SharedModule } from './shared/shared.module';
import { HomeModule } from './home/home.module';

import { AppComponent } from './app.component';
import { NavComponent } from './nav.component';
import { MisModule } from './mis/mis.module';
import { UpdateSectionNameDialogComponent } from './mis/dialogs/update-section-name-dialog.component';
import { MoveFieldToSectionDialogComponent } from './mis/dialogs/move-field-to-section-dialog.component';


@NgModule({
  declarations: [
    AppComponent,
    NavComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    SharedModule,
    HomeModule,
    MisModule,
    AppRoutingModule
  ],
  entryComponents: [
    UpdateSectionNameDialogComponent,
    MoveFieldToSectionDialogComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
