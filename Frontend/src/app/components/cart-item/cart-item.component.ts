import { Component, Input, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BooksService } from 'src/app/services/bookservice/books-service.service';

@Component({
  selector: 'app-cart-item',
  templateUrl: './cart-item.component.html',
  styleUrls: ['./cart-item.component.css'],
})
export class CartItemComponent implements OnInit {
  @Input() cartItem: any;
  @Input() book: any;
  quantity: number;

  constructor(private _service: BooksService, private _snackbar: MatSnackBar) {}

  ngOnInit(): void {
    console.log(this.cartItem);
    this.quantity = this.cartItem.itemQuantity;
  }

  decrementQuantity(quantity) {
    quantity--;
    this.updateQuantity(quantity);
  }

  incrementQuantity(quantity) {
    quantity++;
    console.log(quantity);
    this.updateQuantity(quantity);
  }

  updateQuantity(quantity) {
    this._service
      .updateCart({
        bookId: this.cartItem.Book.Id,
        quantity: quantity,
      })
      .subscribe(
        (response) =>
          (this.cartItem.ItemQuantity = response['data']['itemQuantity']),
        (error) =>
          this._snackbar.open('Error changing quantity', '', {
            duration: 2000,
          })
      );
  }

  removeFromCart(bookId) {
    this._service.deleteFromCart(bookId).subscribe(
      (response) => {},
      (error) =>
        this._snackbar.open('Error deleting from cart', '', {
          duration: 2000,
        })
    );
  }
}
