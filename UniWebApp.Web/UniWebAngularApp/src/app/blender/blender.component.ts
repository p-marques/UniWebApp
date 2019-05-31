import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-blender',
  templateUrl: './blender.component.html',
  styleUrls: ['./blender.component.css']
})
export class BlenderComponent implements OnInit {
  images: string[] = [];

  constructor() { }

  ngOnInit() {
    for (let i = 1; i < 10; i++) {
      this.images.push('assets/images/blender_' + i + '.jpeg');
    }
  }

}
