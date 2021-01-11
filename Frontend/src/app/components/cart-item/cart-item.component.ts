import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-cart-item',
  templateUrl: './cart-item.component.html',
  styleUrls: ['./cart-item.component.css'],
})
export class CartItemComponent implements OnInit {
  @Input() cartItem: any;
  @Input() book: any;
  quantity: number;

  constructor() {}

  ngOnInit(): void {
    console.log(this.cartItem);
    this.quantity = this.cartItem.itemQuantity;
  }
}
