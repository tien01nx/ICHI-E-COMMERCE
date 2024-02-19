import { TestBed } from '@angular/core/testing';

import { SlugifyService } from './slugify.service';

describe('SlugifyService', () => {
  let service: SlugifyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SlugifyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
