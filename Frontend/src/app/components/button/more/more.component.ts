import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-more',
  templateUrl: './more.component.html',
  styleUrls: ['./more.component.css'],
})
export class MoreComponent implements OnInit {
  isAdmin: boolean =
    JSON.parse(localStorage.getItem('userData')).user.role === 'admin'
      ? true
      : false;
  @Input() isFocused: boolean;
  @Output() deleteEvent = new EventEmitter();
  @Output() editEvent = new EventEmitter();

  constructor() {}

  ngOnInit(): void {
    console.log(this.isAdmin);
  }

  deleteBook() {
    this.deleteEvent.emit();
  }

  editBook() {
    this.editEvent.emit();
  }
}
