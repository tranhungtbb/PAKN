import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BusinessUpdateInfoComponent } from './business-update-info.component';

describe('BusinessUpdateInfoComponent', () => {
  let component: BusinessUpdateInfoComponent;
  let fixture: ComponentFixture<BusinessUpdateInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BusinessUpdateInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BusinessUpdateInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
