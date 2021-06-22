import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewRecommendationsSyncComponent } from './view-recommendations-sync.component';

describe('ViewRecommendationsSyncComponent', () => {
  let component: ViewRecommendationsSyncComponent;
  let fixture: ComponentFixture<ViewRecommendationsSyncComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewRecommendationsSyncComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewRecommendationsSyncComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
