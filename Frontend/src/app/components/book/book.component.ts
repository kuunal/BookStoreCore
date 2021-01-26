import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BooksService } from 'src/app/services/bookservice/books-service.service';
import { AddBookComponent } from '../add-book/add-book.component';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.css'],
})
export class BookComponent implements OnInit {
  @Input() book: any;
  @Input() cartItem: any;
  @Output() cartId = new EventEmitter();
  isHeaderFocused: boolean = false;

  constructor(
    private _service: BooksService,
    private _snackBar: MatSnackBar,
    private _dialog: MatDialog
  ) {}
  style = {
    textAlign: 'left',
    color: 'gray',
    fontStyle: 'bold',
    fontSize: '0.5em',
    padding: '0',
    margin: '0',
  };

  ngOnInit(): void {}

  addToCart(book) {
    this.cartId.emit(book.id);
  }

  deleteBook() {
    this._service.deleteBook(this.book.id).subscribe(
      (response) => {},
      (error) =>
        this._snackBar.open('Error deleting book', '', {
          duration: 2000,
        })
    );
  }

  editBook() {
    this._dialog.open(AddBookComponent, { data: this.book });
  }
}
