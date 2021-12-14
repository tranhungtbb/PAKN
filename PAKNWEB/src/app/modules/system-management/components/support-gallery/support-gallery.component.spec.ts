import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SupportGalleryComponent } from './support-gallery.component';

describe('SupportGalleryComponent', () => {
  let component: SupportGalleryComponent;
  let fixture: ComponentFixture<SupportGalleryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SupportGalleryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SupportGalleryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
