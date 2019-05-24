import { TestBed } from '@angular/core/testing';

import { PhoneConfigService } from './phone-config.service';

describe('PhoneConfigService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PhoneConfigService = TestBed.get(PhoneConfigService);
    expect(service).toBeTruthy();
  });
});
