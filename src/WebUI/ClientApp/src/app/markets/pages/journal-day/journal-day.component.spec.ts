import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JournalDayComponent } from './journal-day.component';

describe('JournalDayComponent', () => {
  let component: JournalDayComponent;
  let fixture: ComponentFixture<JournalDayComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JournalDayComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JournalDayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
