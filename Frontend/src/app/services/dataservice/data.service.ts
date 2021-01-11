import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  constructor() {}

  private _booksEvent$ = new BehaviorSubject('');
  private _cartEvent$ = new BehaviorSubject('');
}
