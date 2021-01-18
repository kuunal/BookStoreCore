import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BooksService } from 'src/app/services/bookservice/books-service.service';

@Component({
  selector: 'app-address',
  templateUrl: './address.component.html',
  styleUrls: ['./address.component.css'],
})
export class AddressComponent implements OnInit {
  address;
  addressForm: FormGroup;
  isEditEnabled: boolean = false;
  constructor(private _service: BooksService, private builder: FormBuilder) {}

  ngOnInit(): void {
    this._service.getAddress().subscribe(
      (response) => {
        this.address = response['data'];
        this.initializeForm();
      },
      (error) => console.log(error)
    );

    console.log(this.address);
  }

  initializeForm() {
    this.addressForm = this.builder.group({
      room: [this.address.house, [Validators.required]],
      street: [this.address.street, [Validators.required]],
      locality: [this.address.locality, [Validators.required]],
      city: [this.address.city, [Validators.required]],
      pincode: [this.address.pincode, [Validators.required]],
    });
  }
}
