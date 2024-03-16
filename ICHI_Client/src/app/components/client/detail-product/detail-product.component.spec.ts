import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailProductComponent } from './detail-product.component';
import { Environment } from '../../../environment/environment';

describe('DetailProductComponent', () => {
  let component: DetailProductComponent;
  Environment: Environment;
  let fixture: ComponentFixture<DetailProductComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DetailProductComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(DetailProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
