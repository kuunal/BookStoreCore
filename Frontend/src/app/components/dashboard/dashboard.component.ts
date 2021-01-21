import { HttpParams } from '@angular/common/http';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { forkJoin, Observable } from 'rxjs';
import { BooksService } from 'src/app/services/bookservice/books-service.service';
import { AddBookComponent } from '../add-book/add-book.component';

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
  isCartLoaded: boolean = false;
  isBooksLoaded: boolean = false;
  isAdmin: string = JSON.parse(localStorage.getItem('userData')).role;

  constructor(
    private _service: BooksService,
    private _snackbar: MatSnackBar,
    private dialog: MatDialog,
    private _router: Router
  ) {}

  ngOnInit() {
    this.getCartItems();

    this.getBooks();
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
        (resp) => {
          this.books = resp['data'];
          this.paginatedBook = this.books.slice(0, this.pageSize);
        },
        (error) => {
          if (error.status === 400) {
            localStorage.clear();
            this._router.navigate(['']);
          }
        },
        () => (this.isBooksLoaded = true)
      );
  }

  getCartItems() {
    this._service.getCartItems().subscribe(
      (res) => {
        this.cartItems = res['Data']['cartItems'];
        console.log(this.cartItems);
      },
      (error) => console.log(error),
      () => (this.isCartLoaded = true)
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

  filterBooks(value) {
    this.p = 1;
    switch (value) {
      case 'PRICE_ASC':
        this.books = [
          ...this.books.sort((prev, next) => prev.price - next.price),
          [],
        ];
        this.changePage(1);
        break;
      case 'PRICE_DESC':
        this.books = [
          ...this.books.sort((prev, next) => next.price - prev.price),
        ];
        this.changePage(1);
        break;
      case 'DESC':
        this.books = [...this.books.sort((prev, next) => next.id - prev.id)];
        this.changePage(1);
        break;
      default:
        this.books = [...this.books.sort((prev, next) => prev.id - next.id)];
        this.changePage(1);
    }
  }

  openDialog() {
    let dialogref = this.dialog.open(AddBookComponent, {
      width: '50%',
    });
  }
}
