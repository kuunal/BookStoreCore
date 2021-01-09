import { Injectable } from '@angular/core';
import { HttpService } from '../httpservice/http-service.service';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LoginService {
  loginBackendUri: string = `${environment.backendUri}user/login`;
  registerBackendUri: string = `${environment.backendUri}user`;

  constructor(private _http: HttpService) {}

  login(data) {
    return this._http.post(data, this.loginBackendUri);
  }

  register(data) {
    return this._http.post(data, this.registerBackendUri);
  }
}
