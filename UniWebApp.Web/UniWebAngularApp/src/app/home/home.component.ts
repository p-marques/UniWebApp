import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
  }

  onGoToPage(value: string) {
    if (value === 'MIS') {
      this.router.navigate(['/mis']);
    } else if (value === 'BLENDER') {
      this.router.navigate(['/blender']);
    } else if (value === 'MODDING') {
      this.router.navigate(['/mods']);
    } else if (value === 'DESKTOP APPS') {
      this.router.navigate(['/desktop']);
    }
  }

}
