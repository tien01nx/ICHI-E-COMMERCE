import { TestBed } from '@angular/core/testing';

import { PromotiondemoService } from './promotiondemo.service';

describe('PromotiondemoService', () => {
  let service: PromotiondemoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PromotiondemoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
