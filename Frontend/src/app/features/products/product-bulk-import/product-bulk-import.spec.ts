import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductBulkImport } from './product-bulk-import';

describe('ProductBulkImport', () => 
{
  let component: ProductBulkImport;
  let fixture: ComponentFixture<ProductBulkImport>;

  beforeEach(async () => 
  {
    await TestBed.configureTestingModule({
      imports: [ProductBulkImport]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductBulkImport);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => 
  {
    expect(component).toBeTruthy();
  });
});
