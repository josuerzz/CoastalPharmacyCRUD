import { Component, Input, Output, EventEmitter, ViewChild, ElementRef, ChangeDetectorRef, NgZone, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../../core/services/product.service';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { BulkCreateProductDto } from '../../../models/coastalpharmacy.models';
import * as XLSX from 'xlsx';
import Swal from 'sweetalert2';

const Toast = Swal.mixin({
  toast: true,
  position: 'top-end',
  showConfirmButton: false,
  timer: 2000,
  timerProgressBar: true,
  showCloseButton: true,
  background: '#ffffff',
  customClass: {
    popup: 'my-swal-toast'
  },
  didOpen: (toast) => {
    toast.addEventListener('mouseenter', Swal.stopTimer)
    toast.addEventListener('mouseleave', Swal.resumeTimer)
  }
});

@Component({
  selector: 'app-product-bulk-import',
  standalone: true,
  imports: [CommonModule, FormsModule, FontAwesomeModule],
  templateUrl: './product-bulk-import.html',
  styleUrl: './product-bulk-import.scss',
})

export class ProductBulkImport 
{
  @Input() categories: any[] = [];
  @Input() subcategories: any[] = [];
  
  @Output() onUploadSuccess = new EventEmitter<void>();

  @ViewChild('fileInput') fileInput!: ElementRef;

  productsToImport: any[] = [];
  selectedFileName: string = '';
  isModalOpen: boolean = false; 
  faTrash = faTrash;

  isConnected = input<boolean>(true);

  public codeErrorDescription: { [key: number]: string } = 
  {
    400: "Bad Request, please verify your data or contact us if necessary",
    401: "Unauthorized, please sign in first",
    403: "Forbidden, you don't have permissions",
    404: "Not found, contact us Asap",
    500: "Internal server error, try later",
    503: "Service Unavailable, try later"
  };

  constructor
  (
    private productService: ProductService,
    private cdr: ChangeDetectorRef,
    private ngZone: NgZone
  ) {}

  getErrorMessages(code: number):string
  {
    return this.codeErrorDescription[code] ?? `Error ${code}`;
  }

  triggerFileSelect() 
  {
    this.fileInput.nativeElement.click();
  }

  onFileChange(event: any) 
  {
    const file = event.target.files[0];
    if (!file) return;

    this.selectedFileName = file.name;

    const reader = new FileReader();
    reader.onload = (e: any) => {
      this.ngZone.run(() => {
      const bstr: string = e.target.result;
      const wb: XLSX.WorkBook = XLSX.read(bstr, { type: 'binary' });
      const ws: XLSX.WorkSheet = wb.Sheets[wb.SheetNames[0]];
      const rawData: any[] = XLSX.utils.sheet_to_json(ws);

      //debugger;
      const cleanData = rawData.filter(row => row.Nombre && row.Nombre.toString().trim() !== '');
      
      this.productsToImport = cleanData.map(row => 
      {
        const cat = this.categories.find(c => 
          c.description?.trim().toLowerCase() === row.Categoria?.toString().trim().toLowerCase()
        );

        const subCat = this.subcategories.find(s => 
          s.description?.trim().toLowerCase() === row.Subcategoria?.toString().trim().toLowerCase()
        );

        return {
          name: row.Nombre || 'New Product',
          categoryId: cat ? cat.id : null,
          subCategoryId: subCat ? subCat.id : null,
          stock: row.Stock || 0,
          value: row.Precio || 0,
          description: row.Descripcion || ''
        };
      });

      if (this.productsToImport.length > 0) {
        this.isModalOpen = true;
        this.cdr.markForCheck();
        this.cdr.detectChanges();

      }

      });
    };

    reader.readAsBinaryString(file);
    event.target.value = '';
  }

  hasErrors(): boolean 
  {
    return this.productsToImport.some(p => 
      !p.name || 
      !p.categoryId || 
      p.stock < 0 || !p.stock ||
      p.value <= 0 || !p.value ||
      !p.description
    );
  }

  confirmBulkUpload() 
  {
    this.isModalOpen = false;
    this.cdr.detectChanges();

    Swal.fire({
      title: 'Are you sure?',
      text: `You are going to upload ${this.productsToImport.length} products to pharmacy inventory.`,
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: '#28a745',
      confirmButtonText: 'Yes, upload everything'
    }).then((result) => {
      if (result.isConfirmed) {

        Swal.fire({ 
          title: 'Loading...', 
          allowOutsideClick: false, 
          didOpen: () => { Swal.showLoading(); } 
        });

        const payload: BulkCreateProductDto = 
        {
          fileName: this.selectedFileName,
          products: this.productsToImport
        }

        this.productService.bulkCreateProducts(payload).subscribe({
          next: () => {
             Toast.fire({
              icon: 'success', 
              title: 'Products Loaded!',
              timer: 1000
              });
            this.closeModal(); 
            this.onUploadSuccess.emit();
          },
          error: (err) => {
            console.error(err);
            console.table(err.error.errors);
            Swal.fire('Upss', 'Something went wrong. Server says: ' + (err.error?.message || 'Error', `${this.getErrorMessages(err.status)}`));
          }
        });
      }
    });
  }

  closeModal() 
  {
    this.isModalOpen = false;
    this.productsToImport = [];
    this.selectedFileName = '';
  }

  getCategoryName(id: string) 
  { 
    return this.categories.find(c => c.id === id)?.description || 'Not found'; 
  }

  getSubCategoryName(id: string) 
  { 
    return this.subcategories.find(s => s.id === id)?.description || 'Not found'; 
  }

  removeProductFromImport(index: number) 
  { 
    this.productsToImport.splice(index, 1);
    if (this.productsToImport.length === 0) this.closeModal();
  }
}