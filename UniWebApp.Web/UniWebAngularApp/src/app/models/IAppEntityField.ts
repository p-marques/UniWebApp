import { FieldTypeEnum } from './FieldTypeEnum';

export interface IAppEntityField {
  fieldId: number;
  name: string;
  fieldType: FieldTypeEnum;
  section: string;
  booleanValue: boolean;
  comboboxOptions: string[];
  comboboxSelected: number;
  dateValue: Date;
  numberValue: number;
  textValue: string;
}
