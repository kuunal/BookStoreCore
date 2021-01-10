import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpService } from '../httpservice/http-service.service';

@Injectable({
  providedIn: 'root',
})
export class BooksService {
  getBooksApi = `${environment.backendUri}book`;
  getTotalNumberOfBooksApi = `${environment.backendUri}book/total`;

  constructor(private _http: HttpService) {}

  getBooks(params?) {
    return this._http.get(this.getBooksApi, params);
  }

  getTotalNumberOfBooks() {
    return this._http.get(this.getTotalNumberOfBooksApi);
  }
}
