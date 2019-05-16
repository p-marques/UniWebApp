import { Component, OnInit } from '@angular/core';
import { MisService } from './mis.service';
import { IAppEntity } from '../models/IAppEntity';
import { IAppEntityField } from '../models/IAppEntityField';

@Component({
  selector: 'app-mis',
  templateUrl: './mis.component.html',
  styleUrls: ['./mis.component.css']
})
export class MisComponent implements OnInit {
  isBusy = true;
  appEntities: IAppEntity[] = [];
  selectedEntity: IAppEntity;
  selectedEntitySections: string[];
  selectedEntitySelectedSection: string;
  selectedEntityFields: IAppEntityField[];

  constructor(private misService: MisService) { }

  ngOnInit() {
    this.misService.getEntities().subscribe(
      data => {
        this.appEntities = data.return;
        this.isBusy = false;
      }
    );
  }

  public selectEntity(entity: IAppEntity) {
    this.selectedEntity = entity;
    this.parseSections(this.selectedEntity.fields);
    this.selectedEntityFields = this.selectedEntity.fields;
  }

  private parseSections(fields: IAppEntityField[]) {
    this.selectedEntitySections = ['All'];
    fields.forEach(e => {
      if (!this.selectedEntitySections.includes(e.section)) { this.selectedEntitySections.push(e.section); }
    });
    this.selectedEntitySelectedSection = 'All';
  }

  public onSelectedSectionChange(value: string) {
    this.selectedEntitySelectedSection = value;
    if (this.selectedEntitySelectedSection === 'All') {
      this.selectedEntityFields = this.selectedEntity.fields;
    } else {
      this.selectedEntityFields = this.selectedEntity.fields.filter(z => z.section === this.selectedEntitySelectedSection);
    }
  }


}
