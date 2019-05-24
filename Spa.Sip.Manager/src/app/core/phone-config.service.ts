import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { ContentModel } from '../model/content-model';
import { AuthService } from './auth.service';
import { ConfigModel } from '../model/config-model';
import { tap, catchError } from 'rxjs/operators';
import { ConfigFileModel } from '../model/config-file-model';

@Injectable({
  providedIn: 'root'
})
export class PhoneConfigService {
  private _apiRoot: string = "https://api-sip-mgr.azurewebsites.net";

  constructor(
    private _httpClient: HttpClient,
    private _authService: AuthService
    ) { }

  private getHeadersJson(): HttpHeaders {
    var accessToken = this._authService.getAccessToken();
    var authHeaderText = `Bearer ${accessToken}`;
    return new HttpHeaders()
      .set('Authorization', authHeaderText)
      .set('Content-Type', 'application/json')
      .set('Cache-Control', 'no-cache, no-store, must-revalidate, post-check=0, pre-check=0')
      .set('Pragma', 'no-cache')
      .set('Expires', '0');
  }

  private getHeadersText(): HttpHeaders {
    var accessToken = this._authService.getAccessToken();
    var authHeaderText = `Bearer ${accessToken}`;
    return new HttpHeaders()
      .set('Authorization', authHeaderText)
      .set('Content-Type', 'text/plain')
      .set('Cache-Control', 'no-cache, no-store, must-revalidate, post-check=0, pre-check=0')
      .set('Pragma', 'no-cache')
      .set('Expires', '0');
  }

  getConfigurations(): Observable<ContentModel[]> {
    return this._httpClient.get<ContentModel[]>(this._apiRoot + "/api/config", {headers: this.getHeadersJson()});
  }

  getConfiguration(fileName: string): Observable<string> {
    return this._httpClient.get(this._apiRoot + `/api/config/${fileName}/content`, {headers: this.getHeadersText(), responseType: 'text'});
  }

  getConfigurationContent(fileName: string): Observable<ConfigFileModel> {
    return this._httpClient.get<ConfigFileModel>(this._apiRoot + `/api/config/${fileName}`, {headers: this.getHeadersJson()})
      .pipe(
        catchError(this.handleError)
      );
  }

  getConfigurationModel(fileName: string): Observable<ConfigModel> {
    return this._httpClient.get<ConfigModel>(this._apiRoot + `/api/config/${fileName}/config`, {headers: this.getHeadersJson()})
      .pipe(
        catchError(this.handleError)
      );
  }

  updateConfiguration(macAddress: string, name: string, extension: string, password: string): Observable<string> {
    var body: ConfigModel = {
      featureLabel: `${extension} - ${name}`,
      name: name,
      displayName: extension,
      contact: extension,
      authName: extension,
      authPassword: password
    };

    return this._httpClient.put(
      this._apiRoot + `/api/config/SEP${macAddress}.cnf.xml`,
      body,
      {headers: this.getHeadersJson(), responseType: 'text'}
    )
      .pipe(tap(data => console.log(data), error => console.log(error)));
  }

  createConfiguration(macAddress: string, name: string, extension: string, password: string): Observable<string> {
    var body: ConfigModel = {
      featureLabel: `${extension} - ${name}`,
      name: name,
      displayName: extension,
      contact: extension,
      authName: extension,
      authPassword: password
    };

    return this._httpClient.post(
      this._apiRoot + `/api/config/SEP${macAddress}.cnf.xml`,
      body,
      {headers: this.getHeadersJson(), responseType: 'text'}
    )
      .pipe(tap(data => console.log(data), error => console.log(error)));
  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.log('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.log(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`);
    }
    // return an observable with a user-facing error message
    return throwError(
      'Something bad happened; please try again later.');
  };
}
