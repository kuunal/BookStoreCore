import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BooksService } from 'src/app/services/bookservice/books-service.service';

@Component({
  selector: 'app-add-book',
  templateUrl: './add-book.component.html',
  styleUrls: ['./add-book.component.css'],
})
export class AddBookComponent implements OnInit {
  bookForm: FormGroup;
  image: any;
  constructor(
    private _builder: FormBuilder,
    private _service: BooksService,
    private _snackbar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public book
  ) {}

  ngOnInit(): void {
    this.bookForm = this._builder.group({
      Author: [this.book.author ? this.book.author : '', [Validators.required]],
      Title: [this.book.title ? this.book.title : '', [Validators.required]],
      Description: [
        this.book.description ? this.book.description : '',
        [Validators.required],
      ],
      Price: [this.book.price ? this.book.price : '', [Validators.required]],
      Quantity: [
        this.book.quantity ? this.book.quantity : '',
        [Validators.required],
      ],
    });
    console.log(this.book);
  }

  get Author() {
    return this.bookForm.get('Author').value;
  }

  get Title() {
    return this.bookForm.get('Title').value;
  }

  get Description() {
    return this.bookForm.get('Description').value;
  }

  get Price() {
    return this.bookForm.get('Price').value;
  }

  get Quantity() {
    return this.bookForm.get('Quantity').value;
  }

  getImage(event) {
    this.image = event.target.files[0];
  }

  addBook() {
    let body = new FormData();
    body.append('Author', this.Author);
    body.append('Title', this.Title);
    body.append('Image', this.image);
    body.append('Description', this.Description);
    body.append('Price', this.Price);
    body.append('Quantity', this.Quantity);
    if (this.bookForm.valid === true) {
      if (this.book) {
        this._service.updateBook(body, this.book.Id).subscribe(
          (response) => {},
          (error) =>
            this._snackbar.open('Error updating book', '', {
              duration: 2000,
            })
        );
      } else {
        this._service.addBook(body).subscribe(
          (response) => {},
          (error) =>
            this._snackbar.open('Error adding book', '', {
              duration: 2000,
            })
        );
      }
    }
  }
}
