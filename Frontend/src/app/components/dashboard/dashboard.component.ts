import { HttpParams } from '@angular/common/http';
import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
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

  constructor(private _service: BooksService) {}

  ngOnInit(): void {
    this.getBooks();
    this.getTotalNumberOfBooks();
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
        (error) => console.log(error)
      );
  }
}
