import { HttpParams } from '@angular/common/http';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ConsoleReporter } from 'jasmine';
import { BooksService } from 'src/app/services/bookservice/books-service.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  total: any;
  isDropDownOpen: boolean = false;
  field: string = 'id';
  sortby: string = 'asc';
  lastItemValue: string = '0';
  limit: string = '12';
  books: any;
  totalBooks: void;
  cartItems = [];

  constructor(private _service: BooksService, private _snackbar: MatSnackBar) {}

  ngOnInit(): void {
    this.getBooks();
    this.getTotalNumberOfBooks();
    this.getCartItems();
  }

  getTotalNumberOfBooks() {
    this._service.getTotalNumberOfBooks().subscribe(
      (response) => (this.total = response),
      (error) => console.log(error)
    );
  }

  getBooks() {
    const httpParams = new HttpParams({
      fromObject: {
        field: this.field,
        sortby: this.sortby,
        lastItemValue: this.lastItemValue,
        limit: this.limit,
      },
    });
    this._service
      .getBooks({
        params: httpParams,
      })
      .subscribe(
        (response) => (this.books = response['data']),
        (error) => localStorage.clear()
      );
  }

  getCartItems() {
    this._service.getCartItems().subscribe(
      (response) => (this.cartItems = response['data']),
      (error) =>
        this._snackbar.open('Error fetching cart', '', {
          duration: 2000,
        })
    );
  }

  isAddedInCart(book) {
    return this.cartItems.some((item) => item.bookId === book.id) ? book : null;
  }

  addToCart(bookId) {
    this._service.addToCart({ bookId: bookId, quantity: 1 }).subscribe(
      (response) => this.getCartItems(),
      (error) =>
        this._snackbar.open('Error adding to cart', '', {
          duration: 2000,
        })
    );
  }
}
