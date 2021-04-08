import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateGroupUserComponent } from './create-group-user.component';

describe('CreateGroupUserComponent', () => {
  let component: CreateGroupUserComponent;
  let fixture: ComponentFixture<CreateGroupUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [CreateGroupUserComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateGroupUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
