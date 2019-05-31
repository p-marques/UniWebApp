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
import { BlenderComponent } from './blender/blender.component';
import { ModdingComponent } from './modding/modding.component';
import { DesktopAppsComponent } from './desktop-apps/desktop-apps.component';


@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    BlenderComponent,
    ModdingComponent,
    DesktopAppsComponent
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
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
