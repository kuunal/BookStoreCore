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
import { forkJoin, Observable } from 'rxjs';
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
  limit: string = '53';
  books: any;
  totalBooks: number = 0;
  cartItems: [];
  pageSize: number = 12;
  currentPage: number = 1;
  p: number;
  paginatedBook: [];

  constructor(private _service: BooksService, private _snackbar: MatSnackBar) {}

  ngOnInit() {
    this.getCartItems();

    this.getBooks1();
    this.getTotalNumberOfBooks();
    this._service.getRefreshedCart().subscribe(
      (response) => {
        this.getCartItems();
      },
      (error) => this._snackbar.open(error, '', { duration: 2000 })
    );
    console.log(this.cartItems);
  }

  getTotalNumberOfBooks() {
    this._service.getTotalNumberOfBooks().subscribe(
      (response) => (this.total = response),
      (error) => console.log(error)
    );
  }

  async getBooks() {
    const httpParams = new HttpParams({
      fromObject: {
        field: this.field,
        sortby: this.sortby,
        lastItemValue: this.lastItemValue,
        limit: this.limit,
      },
    });
    await this._service
      .getBooks({
        params: httpParams,
      })
      .toPromise()
      .then((response) => {
        this.books = response['data'];
      })
      .catch((error) => localStorage.clear());
  }

  getBooks1() {
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
        (resp) => {
          this.books = resp['data'];
          this.paginatedBook = this.books.slice(0, this.pageSize);
        },
        (error) => console.log(error)
      );
  }

  getCartItems() {
    this._service.getCartItems().subscribe(
      (res) => {
        this.cartItems = res['Data']['cartItems'];
        console.log(this.cartItems);
      },
      (error) => console.log(error)
    );
  }

  isAddedInCart(book) {
    return this.cartItems &&
      this.cartItems.some((item) => item['Book']['Id'] === book.id)
      ? book
      : null;
  }

  addToCart(bookId) {
    this._service.addToCart({ bookId: bookId, quantity: 1 }).subscribe(
      (response) => {},
      (error) =>
        this._snackbar.open(error, '', {
          duration: 2000,
        })
    );
  }
  changePage(pageNo) {
    this.paginatedBook = this.books.slice(
      pageNo * this.pageSize - this.pageSize,
      pageNo * this.pageSize
    );
  }
}
