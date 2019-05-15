import { IAppEntityField } from './IAppEntityField';

export interface IAppEntity {
  id: number;
  typeId: number;
  typeName: string;
  fields: IAppEntityField[];
}
