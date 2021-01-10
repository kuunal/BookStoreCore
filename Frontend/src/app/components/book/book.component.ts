import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.css'],
})
export class BookComponent implements OnInit {
  @Input() book: any;
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
}
