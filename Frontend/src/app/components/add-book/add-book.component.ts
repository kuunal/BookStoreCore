import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
    private _snackbar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.bookForm = this._builder.group({
      Author: ['', [Validators.required]],
      Title: ['', [Validators.required]],
      Description: ['', [Validators.required]],
      Price: [9, [Validators.required]],
      Quantity: [9, [Validators.required]],
    });
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
      let data = this.bookForm.value;
      let dataWithImage = { ...data, Image: this.image };
      let stringifiedData = JSON.stringify(dataWithImage);
      this.image = null;
      this._service.addBook(body).subscribe(
        (response) => alert('successful'),
        (error) =>
          this._snackbar.open('Error adding book', '', {
            duration: 2000,
          })
      );
    }
  }
}
