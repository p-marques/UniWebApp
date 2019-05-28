import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IAppEntity } from '../models/IAppEntity';
import { IApiResponse, IApiResponseWithReturn } from '../models/IApiResponse';

@Injectable({
  providedIn: 'root'
})
export class MisService {
  private entityUrl = 'http://localhost:53115/api/entities';

  constructor(private http: HttpClient) { }

  public getEntities(): Observable<IApiResponseWithReturn<IAppEntity[]>> {
    return this.http.get<IApiResponseWithReturn<IAppEntity[]>>(this.entityUrl + '?includeFields=true');
  }

  public updateEntity(entity: IAppEntity): Observable<IApiResponse> {
    return this.http.put<IApiResponse>(this.entityUrl + '/' + entity.id, entity);
  }
}
