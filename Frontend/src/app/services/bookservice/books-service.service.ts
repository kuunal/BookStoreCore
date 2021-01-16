import { Injectable } from '@angular/core';
import { error } from 'protractor';
import { Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { HttpService } from '../httpservice/http-service.service';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class BooksService {
  private _getBooksUri: string = `${environment.backendUri}book`;
  private _getTotalNumberOfBooksUri: string = `${environment.backendUri}book/total`;
  private _getCartUri: string = `${environment.backendUri}cart`;
  private _addToCartUri: string = `${environment.backendUri}cart`;
  private _refreshCart = new Subject();

  constructor(private _http: HttpService) {}

  getBooks(params?) {
    return this._http.get(this._getBooksUri, params);
  }

  getTotalNumberOfBooks() {
    return this._http.get(this._getTotalNumberOfBooksUri);
  }

  getCartItems() {
    return this._http.get(this._getCartUri);
  }

  addToCart(data) {
    return this._http
      .post(data, this._addToCartUri)
      .pipe(tap(() => this._refreshCart.next()));
  }

  getRefreshedCart() {
    return this._refreshCart.asObservable();
  }
}
