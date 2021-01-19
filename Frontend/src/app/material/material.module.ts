import { NgModule } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSelectModule } from '@angular/material/select';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatStepperModule } from '@angular/material/stepper';
import { MatDialogModule } from '@angular/material/dialog';

const material = [
  MatToolbarModule,
  MatInputModule,
  MatIconModule,
  MatAutocompleteModule,
  MatSnackBarModule,
  MatButtonModule,
  MatCardModule,
  MatTooltipModule,
  MatFormFieldModule,
  MatCheckboxModule,
  MatSelectModule,
  MatPaginatorModule,
  MatStepperModule,
  MatDialogModule,
];

@NgModule({
  imports: [material],
  exports: [material],
})
export class MaterialModule {}
