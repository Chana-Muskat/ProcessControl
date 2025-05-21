import { TestBed } from '@angular/core/testing';

import { RamInvoicesService } from './ram-invoices.service';

describe('RamInvoicesService', () => {
  let service: RamInvoicesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RamInvoicesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
