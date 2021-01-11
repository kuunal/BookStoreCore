import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BooksService } from 'src/app/services/bookservice/books-service.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
})
export class CartComponent implements OnInit {
  books: any;
  totalBooks: void;
  cartItems = [];

  constructor(private _service: BooksService, private _snackbar: MatSnackBar) {}

  async ngOnInit() {
    await this.getCartItems();
  }

  async getCartItems() {
    await this._service
      .getCartItems()
      .toPromise()
      .then((response) => (this.cartItems = response['data']))
      .catch((error) =>
        this._snackbar.open('Error fetching cart items', '', {
          duration: 2000,
        })
      );
  }

  isAddedInCart(book) {
    return this.cartItems.some((item) => item.book.id === book.id)
      ? book
      : null;
  }
}
