import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RecommnendationGetListComponent } from './recommnendation-get-list.component';

describe('RecommnendationGetListComponent', () => {
  let component: RecommnendationGetListComponent;
  let fixture: ComponentFixture<RecommnendationGetListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RecommnendationGetListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RecommnendationGetListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
