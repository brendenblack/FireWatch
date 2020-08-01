import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JournalMonth2Component } from './journal-month2.component';

describe('JournalMonth2Component', () => {
  let component: JournalMonth2Component;
  let fixture: ComponentFixture<JournalMonth2Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JournalMonth2Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JournalMonth2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
