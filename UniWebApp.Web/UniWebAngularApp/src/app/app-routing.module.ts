import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BlenderComponent } from './blender/blender.component';
import { ModdingComponent } from './modding/modding.component';
import { DesktopAppsComponent } from './desktop-apps/desktop-apps.component';

const routes: Routes = [
  { path: 'blender', component: BlenderComponent },
  { path: 'mods', component: ModdingComponent },
  { path: 'desktop', component: DesktopAppsComponent },
  { path: '', pathMatch: 'full', redirectTo: 'home' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
