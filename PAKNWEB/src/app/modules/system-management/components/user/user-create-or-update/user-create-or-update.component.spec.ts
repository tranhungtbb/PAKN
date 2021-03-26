import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserCreateOrUpdateComponent } from './user-create-or-update.component';

describe('UserCreateOrUpdateComponent', () => {
  let component: UserCreateOrUpdateComponent;
  let fixture: ComponentFixture<UserCreateOrUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserCreateOrUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserCreateOrUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
