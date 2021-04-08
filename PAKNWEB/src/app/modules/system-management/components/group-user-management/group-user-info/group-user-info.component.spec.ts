import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupUserInfoComponent } from './group-user-info.component';

describe('GroupUserInfoComponent', () => {
  let component: GroupUserInfoComponent;
  let fixture: ComponentFixture<GroupUserInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupUserInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupUserInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
