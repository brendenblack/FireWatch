import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RunningReturnComponent } from './running-return.component';

describe('RunningReturnComponent', () => {
  let component: RunningReturnComponent;
  let fixture: ComponentFixture<RunningReturnComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RunningReturnComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RunningReturnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
