import { Component, OnInit } from '@angular/core';
import { MisService } from './mis.service';
import { IAppEntity } from '../models/IAppEntity';
import { IAppEntityField } from '../models/IAppEntityField';
import { SnackBarService } from '../shared/snack-bar.service';
import { MatDialog } from '@angular/material';
import { UpdateSectionNameDialogComponent } from './dialogs/update-section-name-dialog.component';
import { IDialogData } from '../models/IDialogData';
import { MoveFieldToSectionDialogComponent } from './dialogs/move-field-to-section-dialog.component';

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
  selectedEntityFieldsFiltered: IAppEntityField[];
  selectedEntitySections: string[];
  selectedEntitySelectedSection: string;
  fieldsDisabled = true;

  constructor(private misService: MisService, private snackService: SnackBarService, public dialog: MatDialog) { }

  ngOnInit() {
    this.updateEntitiesList();
  }

  private updateEntitiesList() {
    this.isBusy = true;
    this.misService.getEntities().subscribe(
      data => {
        this.appEntities = data.return;
        this.isBusy = false;
      }
    );
  }

  public selectEntity(entity: IAppEntity) {
    this.fieldsDisabled = true;
    this.selectedEntity = entity;
    this.parseSections(this.selectedEntity.fields);
    this.selectedEntityFields = this.getSelectedEntityFields();
    this.filterSelectedEntityFields('Todos');
  }

  private parseSections(fields: IAppEntityField[]) {
    this.selectedEntitySections = ['Todos'];
    fields.forEach(e => {
      if (!this.selectedEntitySections.includes(e.section)) { this.selectedEntitySections.push(e.section); }
    });
    this.selectedEntitySelectedSection = 'Todos';
  }

  private getSelectedEntityFields(section?: string): IAppEntityField[] {
    const value: IAppEntityField[] = [];
    this.selectedEntity.fields.forEach(field => {
      if (section == null || section === 'Todos') {
        value.push(Object.assign({}, field));
      } else if (field.section === section) {
        value.push(Object.assign({}, field));
      }
    });

    return value;
  }

  private filterSelectedEntityFields(section: string) {
    this.selectedEntityFieldsFiltered = [];
    this.selectedEntityFields.forEach(field => {
      if (section === 'Todos') {
        this.selectedEntityFieldsFiltered.push(field);
       } else if (field.section === section) {
        this.selectedEntityFieldsFiltered.push(field);
      }
    });
  }

  public setSelectedSection(value: string) {
    this.selectedEntitySelectedSection = value;
    this.filterSelectedEntityFields(value);
  }

  public clickEditSectionName(value: string) {
    if (value === 'Todos') { return; }
    const dialogObj: IDialogData<string> = { success: false, responseObject: value, options: null };
    const dialogRef = this.dialog.open(UpdateSectionNameDialogComponent, {
      data: dialogObj
    });

    dialogRef.afterClosed().subscribe((result: IDialogData<string>) => {
      if (!result.success) { return; }
      this.updateSectionName(value, result.responseObject);
      this.setSelectedSection('Todos');
      this.parseSections(this.selectedEntityFields);
    });
  }

  private updateSectionName(section: string, newSectionName: string) {
    this.selectedEntityFields.forEach(field => {
      if (field.section === section) { field.section = newSectionName; }
    });
  }

  public clickEditEntity() {
    this.fieldsDisabled = false;
  }

  public clickCancelEditEntity() {
    this.selectedEntityFields = this.getSelectedEntityFields();
    this.parseSections(this.selectedEntityFields);
    this.filterSelectedEntityFields(this.selectedEntitySelectedSection);
    this.fieldsDisabled = true;
  }

  public clickSaveEditEntity() {
    this.selectedEntity.fields = this.selectedEntityFields;
    this.misService.updateEntity(this.selectedEntity).subscribe(
      data => {
        if (data.status === 200) {
          this.snackService.showSnackBar(data.message, null, 5000);
          this.fieldsDisabled = true;
          this.selectedEntity = null;
          this.updateEntitiesList();
        }
      }
    );
  }

  public clickMoveFieldToSection(field: IAppEntityField) {
    const dialogObj: IDialogData<string> = {
      success: false, responseObject: field.section, options: this.selectedEntitySections.filter(x => x !== 'Todos')
    };
    const dialogRef = this.dialog.open(MoveFieldToSectionDialogComponent, {
      data: dialogObj
    });

    dialogRef.afterClosed().subscribe((result: IDialogData<string>) => {
      if (!result.success) { return; }
      field.section = result.responseObject;
      this.setSelectedSection('Todos');
      this.parseSections(this.selectedEntityFields);
    });
  }

  public clickRemoveField(field: IAppEntityField) {
    const i = this.selectedEntityFields.indexOf(field);
    this.selectedEntityFields.splice(i, 1);
    this.setSelectedSection('Todos');
    this.parseSections(this.selectedEntityFields);
  }
}
