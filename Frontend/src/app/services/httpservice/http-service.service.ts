import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  constructor(private _http: HttpClient) {}

  post(data, url) {
    return this._http.post(url, data);
  }

  get(url, params?) {
    return this._http.get(url, params);
  }

  delete(url) {
    return this._http.delete(url);
  }
}
