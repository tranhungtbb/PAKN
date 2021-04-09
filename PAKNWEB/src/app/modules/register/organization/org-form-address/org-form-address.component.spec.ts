import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrgFormAddressComponent } from './org-form-address.component';

describe('OrgFormAddressComponent', () => {
  let component: OrgFormAddressComponent;
  let fixture: ComponentFixture<OrgFormAddressComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrgFormAddressComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrgFormAddressComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
