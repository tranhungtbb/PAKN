import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RecommendationsSyncComponent } from './recommendations-sync.component';

describe('RecommendationsSyncComponent', () => {
  let component: RecommendationsSyncComponent;
  let fixture: ComponentFixture<RecommendationsSyncComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RecommendationsSyncComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RecommendationsSyncComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
