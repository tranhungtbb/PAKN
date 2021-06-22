import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CongThongTinDvHccComponent } from './cong-thong-tin-dv-hcc.component';

describe('CongThongTinDvHccComponent', () => {
  let component: CongThongTinDvHccComponent;
  let fixture: ComponentFixture<CongThongTinDvHccComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CongThongTinDvHccComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CongThongTinDvHccComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
