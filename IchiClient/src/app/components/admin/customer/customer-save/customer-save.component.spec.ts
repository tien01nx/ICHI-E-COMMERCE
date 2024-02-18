import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerSaveComponent } from './customer-save.component';

describe('CustomerSaveComponent', () => {
  let component: CustomerSaveComponent;
  let fixture: ComponentFixture<CustomerSaveComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerSaveComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CustomerSaveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
