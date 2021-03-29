import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewsCreateOrUpdateComponent } from './news-create-or-update.component';

describe('NewsCreateOrUpdateComponent', () => {
  let component: NewsCreateOrUpdateComponent;
  let fixture: ComponentFixture<NewsCreateOrUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewsCreateOrUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewsCreateOrUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
