import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportTradelogComponent } from './import-tradelog.component';

describe('ImportTradelogComponent', () => {
  let component: ImportTradelogComponent;
  let fixture: ComponentFixture<ImportTradelogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImportTradelogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportTradelogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
