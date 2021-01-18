import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatStepper } from '@angular/material/stepper';
import { ActivatedRoute, Router } from '@angular/router';
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
  orderedBooks: Array<number> = [];

  constructor(
    private _service: BooksService,
    private _snackbar: MatSnackBar,
    private _router: Router,
    private _route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.getCartItems();
    this._service.getRefreshedCart().subscribe(
      (response) => {
        this.getCartItems();
      },
      (error) => this._snackbar.open(error, '', { duration: 2000 })
    );
    console.log('SAdsadsad', this.cartItems);
  }

  getCartItems() {
    this._service.getCartItems().subscribe(
      (response) => (this.cartItems = response['Data']['cartItems']),
      (error) =>
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

  initiateOrder(bookId: number, stepper: MatStepper) {
    this.orderedBooks.push(bookId);
    console.log(this.orderedBooks);
    stepper.next();
  }

  placeOrder() {
    let data = {
      bookId: this.orderedBooks[0]['Id'],
      addressId: 1,
      quantity: 1,
    };
    this._service.placeOrder(data).subscribe(
      (response) => {
        console.log(response);
        this._router.navigate(['order/success', response['data']['orderId']]);
      },
      (error) =>
        this._snackbar.open('Unable to place order', '', {
          duration: 2000,
        })
    );
  }
}
