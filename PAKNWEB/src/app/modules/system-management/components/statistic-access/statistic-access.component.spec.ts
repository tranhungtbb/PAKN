import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StatisticAccessComponent } from './statistic-access.component';

describe('StatisticAccessComponent', () => {
  let component: StatisticAccessComponent;
  let fixture: ComponentFixture<StatisticAccessComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StatisticAccessComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StatisticAccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
