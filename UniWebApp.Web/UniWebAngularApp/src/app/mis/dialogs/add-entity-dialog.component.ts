import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { IAppEntity } from 'src/app/models/IAppEntity';
import { IDialogData } from 'src/app/models/IDialogData';
import { FieldTypeEnum } from 'src/app/models/FieldTypeEnum';
import { MisService } from '../mis.service';
import { IAppEntityType } from 'src/app/models/IAppEntityType';
import { SnackBarService } from 'src/app/shared/snack-bar.service';

@Component({
  selector: 'app-add-entity-dialog',
  templateUrl: './add-entity-dialog.component.html',
  styleUrls: ['./add-entity-dialog.component.css']
})
export class AddEntityDialogComponent implements OnInit {
  types: IAppEntityType[] = [];

  constructor(public dialogRef: MatDialogRef<AddEntityDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: IDialogData<IAppEntity, string>, private misService: MisService,
    private snackService: SnackBarService) { }

  ngOnInit() {
    this.data.responseObject = { id: 0, typeId: null, typeName: null, name: null, fields: [], relations: [] };
    this.data.responseObject.fields.push({
      fieldId: 0, name: 'Nome', fieldType: FieldTypeEnum.Text, section: 'Dados Pessoais',
      booleanValue: false, comboboxOptions: null, comboboxSelected: 0, dateValue: new Date().toDateString(),
      numberValue: 0, textValue: ''
    });

    this.misService.getEntityTypes().subscribe(data => {
      if (data.status === 200) {
        this.types = data.return;
      } else {
        this.snackService.showSnackBar(data.message, null, 5000);
        this.dialogRef.close(this.data);
      }
    });
  }

  public onCancelExit() {
    this.dialogRef.close(this.data);
  }

  public onSaveExit() {
    this.misService.addEntity(this.data.responseObject).subscribe(data => {
      this.snackService.showSnackBar(data.message, null, 3000);
      if (data.status === 201) {
        this.data.success = true;
        this.dialogRef.close(this.data);
      }
    });
  }

}
