import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { LoginService } from 'src/app/services/loginservice/login.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  myForm: FormGroup;
  userAlreadyExists: boolean = false;
  selectedValue: string = '';
  roles = [
    { value: 'admin', viewValue: 'admin' },
    { value: 'user', viewValue: 'user' },
  ];
  isLoggedIn = !!localStorage.getItem('token');

  constructor(
    private builder: FormBuilder,
    private service: LoginService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.myForm = this.builder.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      email: [
        '',
        [
          Validators.required,
          Validators.pattern(
            '^[a-zA-Z0-9]+[\\.\\-\\+\\_]?[a-zA-Z0-9]+@[a-zA-Z0-9]+[.]?[a-zA-Z]{2,4}[\\.]?([a-z]{2,4})?$'
          ),
        ],
      ],
      password: [
        '',
        [
          Validators.required,
          Validators.pattern(
            '^(?=.*[0-9])(?=.*[A-Z])(?=[a-zA-Z0-9]*[^a-zA-Z0-9][a-zA-Z0-9]*$).{8,}'
          ),
        ],
      ],
      confirmPassword: ['', [Validators.required]],
      showPassword: 'false',
      phonenumber: [
        '',
        [Validators.required, Validators.pattern('^[0-9]{10}$')],
      ],
      role: '',
    });
  }

  get email() {
    return this.myForm.get('email');
  }

  get firstName() {
    return this.myForm.get('firstName');
  }

  get lastName() {
    return this.myForm.get('lastName');
  }

  get password() {
    return this.myForm.get('password');
  }

  get role() {
    return this.myForm.get('role');
  }

  get confirmPassword() {
    return (
      this.myForm.get('confirmPassword').value ===
      this.myForm.get('password').value
    );
  }
  get showPassword() {
    return this.myForm.get('showPassword').value;
  }

  signIn(): void {
    this.router.navigate(['/login']);
  }

  submit(): void {
    let data = this.myForm.value;
    data.phonenumber = '+' + 91 + '' + data.phonenumber;
    this.service.register(data).subscribe(
      (response) => this.router.navigate(['/login']),
      (error) =>
        this.snackBar.open('User already exists', ``, {
          duration: 2000,
        })
    );
  }

  selectRole(roleValue) {
    this.myForm.patchValue({
      role: roleValue,
    });
  }

  setUserAlreadyExist() {
    this.userAlreadyExists = true;
  }
}
