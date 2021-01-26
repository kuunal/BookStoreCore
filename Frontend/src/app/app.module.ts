import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './components/header/header.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material/material.module';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { LoginComponent } from './components/login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  HttpClientModule,
  HttpInterceptor,
  HTTP_INTERCEPTORS,
} from '@angular/common/http';
import { RegisterComponent } from './components/register/register.component';
import { TokenInterceptorService } from './services/token-interceptor/token-interceptor.service';
import { BookComponent } from './components/book/book.component';
import { AuthGuard } from './services/auth.guard';
import { FooterComponent } from './components/footer/footer.component';
import { CartComponent } from './components/cart/cart.component';
import { CartItemComponent } from './components/cart-item/cart-item.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { AddressComponent } from './components/address/address.component';
import { SummaryComponent } from './components/summary/summary.component';
import { PlacedorderComponent } from './components/placedorder/placedorder.component';
import { AddBookComponent } from './components/add-book/add-book.component';
import { LoggedinGuard } from './services/loggedin-guard/loggedin.guard';
import { MoreComponent } from './components/button/more/more.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    DashboardComponent,
    LoginComponent,
    RegisterComponent,
    BookComponent,
    FooterComponent,
    CartComponent,
    CartItemComponent,
    AddressComponent,
    SummaryComponent,
    PlacedorderComponent,
    AddBookComponent,
    MoreComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    NgxPaginationModule,
  ],
  providers: [
    AuthGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptorService,
      multi: true,
    },
    LoggedinGuard,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
