import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { IAppEntityRelation } from 'src/app/models/IAppEntityRelation';
import { IAppEntity } from 'src/app/models/IAppEntity';
import { SnackBarService } from 'src/app/shared/snack-bar.service';
import { IDialogData } from 'src/app/models/IDialogData';

@Component({
  selector: 'app-add-relation-dialog',
  templateUrl: './add-relation-dialog.component.html',
  styleUrls: ['./add-relation-dialog.component.css']
})
export class AddRelationDialogComponent implements OnInit {
  entities: IAppEntity[] = [];
  selectedEntity: IAppEntity;

  constructor(public dialogRef: MatDialogRef<AddRelationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: IDialogData<IAppEntityRelation, IAppEntity>,
    private snackService: SnackBarService) { }

  ngOnInit() {
    this.entities = Object.assign([], this.data.options);
    const i = this.data.options.findIndex(x => x.id === this.data.responseObject.relatedEntity.id);
    if (i >= 0) {
      this.entities.splice(i, 1);
    }
  }

  public selectEntity(entity: IAppEntity) {
    this.selectedEntity = entity;
  }

  public onCancelExit() {
    this.dialogRef.close(this.data);
  }

  public onSaveExit() {
    if (this.selectedEntity == null) {
      this.snackService.showSnackBar('Erro! Seleccione uma entidade para criar a relação.', null, 5000);
    } else {
      this.data.responseObject.relatedEntity = this.selectedEntity;
      this.data.success = true;
      this.dialogRef.close(this.data);
    }
  }

}
