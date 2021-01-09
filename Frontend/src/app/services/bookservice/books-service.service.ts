import { Injectable } from '@angular/core';
import { HttpService } from '../httpservice/http-service.service';

@Injectable({
  providedIn: 'root',
})
export class BooksService {
  getBooksApi = 'http://localhost:43421/books';

  constructor(private _http: HttpService) {}

  getBooks(params?) {
    return this._http.get(this.getBooks, params);
  }
}
