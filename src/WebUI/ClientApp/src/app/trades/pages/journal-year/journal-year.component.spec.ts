import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JournalYearComponent } from './journal-year.component';

describe('JournalYearComponent', () => {
  let component: JournalYearComponent;
  let fixture: ComponentFixture<JournalYearComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JournalYearComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JournalYearComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
