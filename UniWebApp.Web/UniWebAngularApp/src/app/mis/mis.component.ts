import { Component, OnInit } from '@angular/core';
import { MisService } from './mis.service';
import { IAppEntity } from '../models/IAppEntity';

@Component({
  selector: 'app-mis',
  templateUrl: './mis.component.html',
  styleUrls: ['./mis.component.css']
})
export class MisComponent implements OnInit {
  appEntities: IAppEntity[] = [];
  selectedEntity: IAppEntity;

  constructor(private misService: MisService) { }

  ngOnInit() {
    this.misService.getEntities().subscribe(
      data => {
        this.appEntities = data.return;
      }
    );
  }

  public selectEntity(entity: IAppEntity) {
    this.selectedEntity = entity;
  }



}
