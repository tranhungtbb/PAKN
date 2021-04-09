import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrgRepreFormComponent } from './org-repre-form.component';

describe('OrgRepreFormComponent', () => {
  let component: OrgRepreFormComponent;
  let fixture: ComponentFixture<OrgRepreFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrgRepreFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrgRepreFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
