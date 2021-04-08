import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateOrUpdateUnitComponent } from './create-or-update-unit.component';

describe('CreateOrUpdateUnitComponent', () => {
  let component: CreateOrUpdateUnitComponent;
  let fixture: ComponentFixture<CreateOrUpdateUnitComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateOrUpdateUnitComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateOrUpdateUnitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
