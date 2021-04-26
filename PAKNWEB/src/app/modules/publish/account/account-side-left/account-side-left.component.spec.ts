import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountSideLeftComponent } from './account-side-left.component';

describe('AccountSideLeftComponent', () => {
  let component: AccountSideLeftComponent;
  let fixture: ComponentFixture<AccountSideLeftComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccountSideLeftComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountSideLeftComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
