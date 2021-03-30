import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewsRelateModalComponent } from './news-relate-modal.component';

describe('NewsRelateModalComponent', () => {
  let component: NewsRelateModalComponent;
  let fixture: ComponentFixture<NewsRelateModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewsRelateModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewsRelateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
