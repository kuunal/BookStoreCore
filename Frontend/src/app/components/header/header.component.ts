import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { BooksService } from 'src/app/services/bookservice/books-service.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  searchInput = new FormControl();
  searchedResult: [];
  isLoggedIn = !!localStorage.getItem('token');

  @Input() isLoginRegister: boolean = false;

  constructor(private _router: Router, private _service: BooksService) {}

  ngOnInit(): void {
    this.searchInput.valueChanges
      .pipe(
        debounceTime(1000),
        switchMap((value) => this._service.searchByTitle(value)),
        distinctUntilChanged()
      )
      .subscribe((response) => (this.searchedResult = response['data']));
  }

  redirectToHome() {
    this._router.navigate(['']);
  }

  redirectToCart() {
    this._router.navigate(['cart']);
  }
}
