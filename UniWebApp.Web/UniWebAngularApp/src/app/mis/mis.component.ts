import { Component, OnInit } from '@angular/core';
import { MisService } from './mis.service';
import { IAppEntity } from '../models/IAppEntity';
import { IAppEntityField } from '../models/IAppEntityField';
import { SnackBarService } from '../shared/snack-bar.service';
import { MatDialog } from '@angular/material';
import { UpdateSectionNameDialogComponent } from './dialogs/update-section-name-dialog.component';
import { IDialogData } from '../models/IDialogData';
import { MoveFieldToSectionDialogComponent } from './dialogs/move-field-to-section-dialog.component';
import { FieldTypeEnum } from '../models/FieldTypeEnum';
import { AddFieldDialogComponent } from './dialogs/add-field-dialog.component';
import { IAppEntityType } from '../models/IAppEntityType';
import { AddEntityDialogComponent } from './dialogs/add-entity-dialog.component';
import { ConfirmDeleteEntityDialogComponent } from './dialogs/confirm-delete-entity-dialog.component';

@Component({
  selector: 'app-mis',
  templateUrl: './mis.component.html',
  styleUrls: ['./mis.component.css']
})
export class MisComponent implements OnInit {
  isBusy = true;
  isFiltered = false;
  searchText = '';
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

  public searchEntities() {
    const values = this.searchText.split(':');
    if (values.length !== 2) {
      this.snackService.showSnackBar('Erro! Formato incorreto. Dever ser [FieldName]:[FieldValue].', null, 5000);
    } else {
      this.selectedEntity = null;
      this.updateEntitiesList(values[0], values[1]);
      this.isFiltered = true;
    }
  }

  public clearFilter() {
    if (this.isFiltered) {
      this.updateEntitiesList();
      this.isFiltered = false;
      this.searchText = '';
    }
  }

  private updateEntitiesList(withFieldName?: string, withFieldValue?: string, entityToSelect?: string) {
    this.isBusy = true;
    this.misService.getEntities(withFieldName, withFieldValue).subscribe(
      data => {
        this.appEntities = data.return;
        this.isBusy = false;
        if (entityToSelect != null) {
          const a = this.appEntities.filter(t => t.name === entityToSelect);
          if (a.length > 0) {
            this.selectEntity(a[0]);
          }
        }
      }
    );
  }

  public clickAddEntity() {
    const dialogObj: IDialogData<IAppEntity, IAppEntityType> = {
      success: false, responseObject: null, options: null
    };

    const dialogRef = this.dialog.open(AddEntityDialogComponent, {
      minWidth: '400px',
      data: dialogObj
    });

    dialogRef.afterClosed().subscribe((result: IDialogData<IAppEntity, string>) => {
      if (!result.success) { return; }
      this.updateEntitiesList(null, null, result.responseObject.fields[0].textValue);
    });
  }

  public clickRemoveEntity() {
    const dialogObj: IDialogData<boolean, string> = { success: false, responseObject: false, options: null };
    const dialogRef = this.dialog.open(ConfirmDeleteEntityDialogComponent, {
      minWidth: '400px',
      data: dialogObj
    });

    dialogRef.afterClosed().subscribe((result: IDialogData<boolean, string>) => {
      if (!result.success) { return; }
      this.misService.removeEntity(this.selectedEntity).subscribe(data => {
        this.snackService.showSnackBar(data.message, null, 5000);
        if (data.status === 200) {
          this.updateEntitiesList();
          this.selectedEntity = null;
        }
      });
    });
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
    const dialogObj: IDialogData<string, string> = { success: false, responseObject: value, options: null };
    const dialogRef = this.dialog.open(UpdateSectionNameDialogComponent, {
      data: dialogObj
    });

    dialogRef.afterClosed().subscribe((result: IDialogData<string, string>) => {
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

    if (this.selectedEntity.fields.length === 0) {
      this.snackService.showSnackBar('Erro! É obrigatório ter pelo menos um campo.', null, 5000);
      return;
    }

    if (this.selectedEntity.fields.filter(t => t.name === 'Nome').length === 0) {
      this.snackService.showSnackBar('Erro! É obrigatório ter um campo com o nome de campo: \'Nome\'.', null, 5000);
      return;
    }

    this.misService.updateEntity(this.selectedEntity).subscribe(
      data => {
        this.snackService.showSnackBar(data.message, null, 5000);
        if (data.status === 200) {
          this.fieldsDisabled = true;
          this.updateEntitiesList(null, null, this.selectedEntity.fields.filter(t => t.name === 'Nome')[0].textValue);
        }
      }
    );
  }

  public clickMoveFieldToSection(field: IAppEntityField) {
    const dialogObj: IDialogData<string, string> = {
      success: false, responseObject: field.section, options: this.selectedEntitySections.filter(x => x !== 'Todos')
    };
    const dialogRef = this.dialog.open(MoveFieldToSectionDialogComponent, {
      data: dialogObj
    });

    dialogRef.afterClosed().subscribe((result: IDialogData<string, string>) => {
      if (!result.success) { return; }
      field.section = result.responseObject;
      this.setSelectedSection('Todos');
      this.parseSections(this.selectedEntityFields);
    });
  }

  public clickAddField() {
    const newField: IAppEntityField = {
      fieldId: 0, name: '', fieldType: FieldTypeEnum.Text, booleanValue: false, section: '', textValue: '',
      numberValue: 0, dateValue: new Date().toDateString(), comboboxOptions: [], comboboxSelected: 0
    };
    const dialogObj: IDialogData<IAppEntityField, string> = {
      success: false, responseObject: newField, options: this.selectedEntitySections.filter(x => x !== 'Todos')
    };

    const dialogRef = this.dialog.open(AddFieldDialogComponent, {
      minWidth: '400px',
      data: dialogObj
    });

    dialogRef.afterClosed().subscribe((result: IDialogData<IAppEntityField, string>) => {
      if (!result.success) { return; }
      this.selectedEntityFields.push(result.responseObject);
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
