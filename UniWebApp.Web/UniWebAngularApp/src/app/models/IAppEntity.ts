import { IAppEntityField } from './IAppEntityField';

export interface IAppEntity {
  id: number;
  typeId: number;
  typeName: string;
  name: string;
  fields: IAppEntityField[];
}

// export class AppEntity implements IAppEntity {
//   id: number;
//   typeId: number;
//   typeName: string;
//   fields: AppEntityField[];

//   constructor(entity: IAppEntity) {
//     this.id = entity.id;
//     this.typeId = entity.typeId;
//     this.typeName = entity.typeName;
//     this.fields = [];
//     entity.fields.forEach(field => this.fields.push(new AppEntityField(field)));
//   }
// }
