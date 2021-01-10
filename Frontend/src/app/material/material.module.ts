import { NgModule } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatTooltipModule } from '@angular/material/tooltip';

const material = [
  MatToolbarModule,
  MatInputModule,
  MatIconModule,
  MatAutocompleteModule,
  MatSnackBarModule,
  MatButtonModule,
  MatCardModule,
  MatTooltipModule,
];

@NgModule({
  imports: [material],
  exports: [material],
})
export class MaterialModule {}
