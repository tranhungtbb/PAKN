import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HeThongQuanLyKienNghiCuTriComponent } from './he-thong-quan-ly-kien-nghi-cu-tri.component';

describe('HeThongQuanLyKienNghiCuTriComponent', () => {
  let component: HeThongQuanLyKienNghiCuTriComponent;
  let fixture: ComponentFixture<HeThongQuanLyKienNghiCuTriComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HeThongQuanLyKienNghiCuTriComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HeThongQuanLyKienNghiCuTriComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
