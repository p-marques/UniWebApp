import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IAppEntity } from '../models/IAppEntity';
import { IApiResponse, IApiResponseWithReturn } from '../models/IApiResponse';
import { IAppEntityType } from '../models/IAppEntityType';

@Injectable({
  providedIn: 'root'
})
export class MisService {
  private entityUrl = 'http://localhost:53115/api/entities';
  private entityTypesUrl = 'http://localhost:53115/api/entities/types';

  constructor(private http: HttpClient) { }

  public getEntities(withFieldName?: string, withFieldValue?: string): Observable<IApiResponseWithReturn<IAppEntity[]>> {
    let url = this.entityUrl;
    if (withFieldName != null && withFieldValue != null) {
      url += '?withFieldName=' + withFieldName + '&withFieldValue=' + withFieldValue;
    }

    return this.http.get<IApiResponseWithReturn<IAppEntity[]>>(url);
  }

  public addEntity(entity: IAppEntity): Observable<IApiResponse> {
    return this.http.post<IApiResponse>(this.entityUrl, entity);
  }

  public updateEntity(entity: IAppEntity): Observable<IApiResponse> {
    return this.http.put<IApiResponse>(this.entityUrl + '/' + entity.id, entity);
  }

  public getEntityTypes(): Observable<IApiResponseWithReturn<IAppEntityType[]>> {
    return this.http.get<IApiResponseWithReturn<IAppEntityType[]>>(this.entityTypesUrl);
  }
}
