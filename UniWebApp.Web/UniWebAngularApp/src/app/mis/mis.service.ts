import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import { IAppEntity } from '../models/IAppEntity';
import { IApiResponse, IApiResponseWithReturn } from '../models/IApiResponse';
import { IAppEntityType } from '../models/IAppEntityType';
import { SnackBarService } from '../shared/snack-bar.service';

@Injectable({
  providedIn: 'root'
})
export class MisService {
  static snack;
  private entityUrl = 'http://localhost:53115/api/entities';
  private entityTypesUrl = 'http://localhost:53115/api/entities/types';

  constructor(snackService: SnackBarService, private http: HttpClient) { MisService.snack = snackService; }

  public getEntities(withFieldName?: string, withFieldValue?: string): Observable<IApiResponseWithReturn<IAppEntity[]>> {
    let url = this.entityUrl;
    if (withFieldName != null && withFieldValue != null) {
      url += '?withFieldName=' + withFieldName + '&withFieldValue=' + withFieldValue;
    }

    return this.http.get<IApiResponseWithReturn<IAppEntity[]>>(url)
      .pipe(
        retry(1),
        catchError(this.handleError)
      );
  }

  public addEntity(entity: IAppEntity): Observable<IApiResponse> {
    return this.http.post<IApiResponse>(this.entityUrl, entity)
      .pipe(
        retry(1),
        catchError(this.handleError)
      );
  }

  public updateEntity(entity: IAppEntity): Observable<IApiResponse> {
    return this.http.put<IApiResponse>(this.entityUrl + '/' + entity.id, entity)
      .pipe(
        retry(1),
        catchError(this.handleError)
      );
  }

  public removeEntity(entity: IAppEntity): Observable<IApiResponse> {
    return this.http.delete<IApiResponse>(this.entityUrl + '/' + entity.id)
      .pipe(
        retry(1),
        catchError(this.handleError)
      );
  }

  public getEntityTypes(): Observable<IApiResponseWithReturn<IAppEntityType[]>> {
    return this.http.get<IApiResponseWithReturn<IAppEntityType[]>>(this.entityTypesUrl)
      .pipe(
        retry(1),
        catchError(this.handleError)
      );
  }

  private handleError(error) {
    MisService.snack.showSnackBar(error.error.message, null, 5000);
    return throwError(error.error);
  }

}
