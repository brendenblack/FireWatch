import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TradesTestComponent } from './trades-test.component';

describe('TradesTestComponent', () => {
  let component: TradesTestComponent;
  let fixture: ComponentFixture<TradesTestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TradesTestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TradesTestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
