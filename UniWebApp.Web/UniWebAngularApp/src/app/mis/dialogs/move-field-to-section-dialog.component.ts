import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { IDialogData } from 'src/app/models/IDialogData';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'move-field-to-section-dialog',
  templateUrl: './move-field-to-section-dialog.html',
})
export class MoveFieldToSectionDialogComponent {

  constructor(public dialogRef: MatDialogRef<MoveFieldToSectionDialogComponent>,
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
