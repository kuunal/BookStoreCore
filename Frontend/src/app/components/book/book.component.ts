import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.css'],
})
export class BookComponent implements OnInit {
  @Input() book: any;
  @Input() cartItem: any;
  @Output() cartId = new EventEmitter();

  constructor() {}
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
}
