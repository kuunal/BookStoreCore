import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlacedorderComponent } from './placedorder.component';

describe('PlacedorderComponent', () => {
  let component: PlacedorderComponent;
  let fixture: ComponentFixture<PlacedorderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlacedorderComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlacedorderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
