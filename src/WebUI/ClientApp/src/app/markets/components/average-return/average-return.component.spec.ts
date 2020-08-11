import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AverageReturnComponent } from './average-return.component';

describe('AverageReturnComponent', () => {
  let component: AverageReturnComponent;
  let fixture: ComponentFixture<AverageReturnComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AverageReturnComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AverageReturnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
