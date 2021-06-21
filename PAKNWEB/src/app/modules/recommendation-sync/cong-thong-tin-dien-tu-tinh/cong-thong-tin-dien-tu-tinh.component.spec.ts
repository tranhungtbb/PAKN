import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CongThongTinDienTuTinhComponent } from './cong-thong-tin-dien-tu-tinh.component';

describe('CongThongTinDienTuTinhComponent', () => {
  let component: CongThongTinDienTuTinhComponent;
  let fixture: ComponentFixture<CongThongTinDienTuTinhComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CongThongTinDienTuTinhComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CongThongTinDienTuTinhComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
