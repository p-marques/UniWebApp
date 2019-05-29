import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { IDialogData } from 'src/app/models/IDialogData';
import { IAppEntityField } from 'src/app/models/IAppEntityField';
import { FieldTypeEnum } from 'src/app/models/FieldTypeEnum';
import { SnackBarService } from 'src/app/shared/snack-bar.service';

@Component({
  selector: 'app-add-field-dialog',
  templateUrl: './add-field-dialog.component.html',
  styleUrls: ['./add-field-dialog.component.css']
})
export class AddFieldDialogComponent {
  fieldTypes = ['Texto', 'Número', 'Data', 'Escolha Múltipla', 'Binário'];
  newComboboxOption = '';

  constructor(public dialogRef: MatDialogRef<AddFieldDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: IDialogData<IAppEntityField, string>, private snackService: SnackBarService) { }

  public addComboboxOption() {
    if (this.newComboboxOption.length >= 1 || this.newComboboxOption.length <= 120) {
      this.data.responseObject.comboboxOptions.push(this.newComboboxOption);
      this.newComboboxOption = '';
    }
  }

  public removeComboboxOption(optionIndex: number) {
    this.data.responseObject.comboboxOptions.splice(optionIndex, 1);
  }

  public onCancelExit() {
    this.dialogRef.close(this.data);
  }

  public onSaveExit() {
    if (this.data.responseObject.fieldType === FieldTypeEnum.Combobox && this.data.responseObject.comboboxOptions.length === 0) {
      this.snackService.showSnackBar('Erro! Um campo de escolha múltipla precisa de opções.', null, 10000);
    } else {
      this.data.success = true;
      this.dialogRef.close(this.data);
    }
  }

}
