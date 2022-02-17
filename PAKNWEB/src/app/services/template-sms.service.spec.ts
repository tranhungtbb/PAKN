import { TestBed } from '@angular/core/testing';

import { TemplateSmsService } from './template-sms.service';

describe('TemplateSmsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TemplateSmsService = TestBed.get(TemplateSmsService);
    expect(service).toBeTruthy();
  });
});
