import { NgModule } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatAutocompleteModule } from '@angular/material/autocomplete';

const material = [
  MatToolbarModule,
  MatInputModule,
  MatIconModule,
  MatAutocompleteModule,
];

@NgModule({
  imports: [material],
  exports: [material],
})
export class MaterialModule {}
