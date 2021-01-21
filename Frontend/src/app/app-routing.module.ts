import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CartComponent } from './components/cart/cart.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { LoginComponent } from './components/login/login.component';
import { PlacedorderComponent } from './components/placedorder/placedorder.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthGuard } from './services/auth.guard';
import { LoggedinGuard } from './services/loggedin-guard/loggedin.guard';

const routes: Routes = [
  { path: 'login', component: LoginComponent, canActivate: [LoggedinGuard] },
  {
    path: 'register',
    component: RegisterComponent,
    canActivate: [LoggedinGuard],
  },
  { path: '', component: DashboardComponent, canActivate: [AuthGuard] },
  {
    path: 'cart',
    component: CartComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'order/success/:id',
    component: PlacedorderComponent,
    canActivate: [AuthGuard],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
