import { IAppEntity } from './IAppEntity';

export interface IAppEntityRelation {
  id: number;
  relatedEntity: IAppEntity;
  description: string;
}
