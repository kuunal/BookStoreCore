import { NgModule } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatButtonModule } from '@angular/material/button';

const material = [
  MatToolbarModule,
  MatInputModule,
  MatIconModule,
  MatAutocompleteModule,
  MatSnackBarModule,
  MatButtonModule,
];

@NgModule({
  imports: [material],
  exports: [material],
})
export class MaterialModule {}
