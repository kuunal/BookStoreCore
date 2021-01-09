import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { BooksService } from 'src/app/services/bookservice/books-service.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  total: number = 54;
  isDropDownOpen: boolean = false;

  constructor(private _service: BooksService) {}

  ngOnInit(): void {
    this._service.getBooks({});
  }
}
