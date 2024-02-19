import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InsertAdminProductComponent } from './insert-admin-product.component';

describe('InsertAdminProductComponent', () => {
  let component: InsertAdminProductComponent;
  let fixture: ComponentFixture<InsertAdminProductComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InsertAdminProductComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(InsertAdminProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
