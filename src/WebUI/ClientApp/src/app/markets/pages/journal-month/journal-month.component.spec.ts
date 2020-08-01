import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JournalMonthComponent } from './journal-month.component';

describe('JournalMonthComponent', () => {
  let component: JournalMonthComponent;
  let fixture: ComponentFixture<JournalMonthComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JournalMonthComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JournalMonthComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
