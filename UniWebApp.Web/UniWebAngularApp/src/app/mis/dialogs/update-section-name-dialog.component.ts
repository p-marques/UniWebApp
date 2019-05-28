import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { IDialogData } from 'src/app/models/IDialogData';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'update-section-name-dialog',
  templateUrl: './update-section-name-dialog.html',
})
export class UpdateSectionNameDialogComponent {

  constructor(public dialogRef: MatDialogRef<UpdateSectionNameDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: IDialogData<string>) { }

  onCancelClick(): void {
    this.dialogRef.close(this.data);
  }

  onSaveClick(): void {
    if (this.data.responseObject.length >= 2 && this.data.responseObject.length <= 50) {
      this.data.success = true;
      this.dialogRef.close(this.data);
    }
  }

}
