import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class HttpServiceService {
  constructor(private _http: HttpClient) {}

  post(data, url) {
    this._http.post(url, data);
  }

  get(url) {
    this._http.get(url);
  }

  delete(url) {
    this._http.delete(url);
  }
}
