import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { LoginService } from 'src/app/services/loginservice/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  hide: boolean = true;
  myForm: FormGroup;

  constructor(
    private builder: FormBuilder,
    private service: LoginService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.myForm = this.builder.group({
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
    });
  }

  get email() {
    return this.myForm.get('email');
  }

  get password() {
    return this.myForm.get('password');
  }

  redirectToSignUp() {
    this.router.navigate(['/register']);
  }

  submit(): void {
    console.log(this.myForm.value);

    let data = { ...this.myForm.value, cartId: '' };
    console.log(data);

    this.service.login({ ...data }).subscribe(
      (response) => {
        localStorage.setItem('token', response['data'].token);
        localStorage.setItem('userData', JSON.stringify(response['data']));
        this.router.navigate(['/']);
      },
      (error) => {
        if (error.status === 401) {
          this.snackBar.open('Invalid Id or password', '', {
            duration: 2000,
          });
        } else {
          this.snackBar.open('Something went wrong', '', {
            duration: 2000,
          });
        }
      }
    );
  }
}
