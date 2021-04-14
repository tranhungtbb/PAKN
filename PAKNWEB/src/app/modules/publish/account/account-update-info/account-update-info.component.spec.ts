import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountUpdateInfoComponent } from './account-update-info.component';

describe('AccountUpdateInfoComponent', () => {
  let component: AccountUpdateInfoComponent;
  let fixture: ComponentFixture<AccountUpdateInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccountUpdateInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountUpdateInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
