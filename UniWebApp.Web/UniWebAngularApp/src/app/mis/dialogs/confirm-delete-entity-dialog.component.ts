import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { IDialogData } from 'src/app/models/IDialogData';

@Component({
  selector: 'app-confirm-delete-entity-dialog',
  templateUrl: './confirm-delete-entity-dialog.component.html'
})
export class ConfirmDeleteEntityDialogComponent {

  constructor(public dialogRef: MatDialogRef<ConfirmDeleteEntityDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: IDialogData<boolean, string>) { }

  public onNoExit() {
    this.dialogRef.close(this.data);
  }

  public onYesExit() {
    this.data.success = true;
    this.data.responseObject = true;
    this.dialogRef.close(this.data);
  }

}
