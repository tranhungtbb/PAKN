import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HeThongPaknChinhPhuComponent } from './he-thong-pakn-chinh-phu.component';

describe('HeThongPaknChinhPhuComponent', () => {
  let component: HeThongPaknChinhPhuComponent;
  let fixture: ComponentFixture<HeThongPaknChinhPhuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HeThongPaknChinhPhuComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HeThongPaknChinhPhuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
