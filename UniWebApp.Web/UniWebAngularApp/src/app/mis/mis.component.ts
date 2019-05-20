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
  selectedEntityFields: IAppEntityField[];
  selectedEntitySections: string[];
  selectedEntitySelectedSection: string;
  fieldsDisabled = true;

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
    this.selectedEntityFields = this.getSelectedEntityFields();
  }

  private parseSections(fields: IAppEntityField[]) {
    this.selectedEntitySections = ['All'];
    fields.forEach(e => {
      if (!this.selectedEntitySections.includes(e.section)) { this.selectedEntitySections.push(e.section); }
    });
    this.selectedEntitySelectedSection = 'All';
  }

  private getSelectedEntityFields(section?: string): IAppEntityField[] {
    const value: IAppEntityField[] = [];
    this.selectedEntity.fields.forEach(field => {
      if (section == null || section === 'All') {
        value.push(Object.assign({}, field));
      } else if (field.section === section) {
        value.push(Object.assign({}, field));
      }
    });

    return value;
  }

  public onSelectedSectionChange(value: string) {
    this.selectedEntitySelectedSection = value;
    this.selectedEntityFields = this.getSelectedEntityFields(value);
  }

  public clickEditEntity() {
    this.fieldsDisabled = false;
  }

  public clickCancelEditEntity() {
    this.selectedEntityFields = this.getSelectedEntityFields(this.selectedEntitySelectedSection);
    this.fieldsDisabled = true;
  }
}
