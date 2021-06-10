import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CallHistoryListComponent } from './call-history-list.component';

describe('CallHistoryListComponent', () => {
  let component: CallHistoryListComponent;
  let fixture: ComponentFixture<CallHistoryListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CallHistoryListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CallHistoryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
