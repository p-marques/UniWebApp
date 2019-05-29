import { FieldTypeEnum } from './FieldTypeEnum';

export interface IAppEntityField {
  fieldId: number;
  name: string;
  fieldType: FieldTypeEnum;
  section: string;
  booleanValue: boolean;
  comboboxOptions: string[];
  comboboxSelected: number;
  dateValue: string;
  numberValue: number;
  textValue: string;
}

// export class AppEntityField implements IAppEntityField {
//   fieldId: number;
//   name: string;
//   fieldType: FieldTypeEnum;
//   section: string;
//   booleanValue: boolean;
//   comboboxOptions: string[];
//   comboboxSelected: number;
//   dateValue: Date;
//   numberValue: number;
//   textValue: string;

//   constructor(field?: IAppEntityField) {
//     if (field != null) {
//       this.fieldId = field.fieldId;
//       this.name = field.name;
//       this.fieldType = field.fieldType;
//       this.section = field.section;
//       this.booleanValue = field.booleanValue;
//       this.comboboxOptions = field.comboboxOptions;
//       this.comboboxSelected = field.comboboxSelected;
//       this.dateValue = field.dateValue;
//       this.numberValue = field.numberValue;
//       this.textValue = field.textValue;
//     }
//   }
// }
