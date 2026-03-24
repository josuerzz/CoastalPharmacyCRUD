import { Component, OnInit, inject, ChangeDetectorRef, signal, computed } from '@angular/core';
import { CommonModule, formatDate } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../../core/services/product.service';
import { AuthService } from '../../../core/services/auth.service';
import { CDL_Identifier, IdentifierCreateDto, OBJ_Product, ProductCreateDto } from '../../../models/coastalpharmacy.models';
import { NgxPaginationModule } from 'ngx-pagination';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faCirclePlus, faCheck, 
         faXmark, 
         faPenToSquare, 
         faTrash, 
         faFileExcel, 
         faFileCircleQuestion,
         faRightFromBracket } from '@fortawesome/free-solid-svg-icons';
import { ProductBulkImport } from '../product-bulk-import/product-bulk-import';
import { forkJoin } from 'rxjs';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';

const Toast = Swal.mixin({
  toast: true,
  position: 'top-end',
  showConfirmButton: false,
  timer: 2000,
  timerProgressBar: true,
  showCloseButton: true,
  background: '#ffffff',
  customClass: 
  {
    popup: 'my-swal-toast'
  },
  didOpen: (toast) => 
  {
    toast.addEventListener('mouseenter', Swal.stopTimer)
    toast.addEventListener('mouseleave', Swal.resumeTimer)
  }
});

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NgxPaginationModule,
    FontAwesomeModule,
    ProductBulkImport
  ],
  templateUrl: './product-list.html',
  styleUrl: './product-list.scss',
})
export class ProductList implements OnInit {

  private productService = inject(ProductService);
  private authService = inject(AuthService);
  public userName = this.authService.currentUserFullName;
  private cdr = inject(ChangeDetectorRef);
  private router = inject(Router);

  public newProduct: ProductCreateDto =
  {
    name: '',
    categoryId: '',
    subCategoryId: '',
    stock: 0,
    description: '',
    value: 0
  }

  public newIdentifier: IdentifierCreateDto =
  {
    set: '',
    description: '',
    use: '',
    parentId: null
  }

  public categories: CDL_Identifier[] = [];
  public subCategories: CDL_Identifier[] = [];
  public subCategoriesFiltered: CDL_Identifier[] = [];
  public products: OBJ_Product[] = [];
  public productById: OBJ_Product | null = null;
  public loading: boolean = true;
  public productPrice: number | null = null;

  public codeErrorDescription: { [key: number]: string } = 
  {
    400: "Bad Request, please verify your data or contact us if necessary",
    401: "Unauthorized, please sign in first",
    403: "Forbidden, you don't have permissions",
    404: "Not found, contact us Asap",
    500: "Internal server error, try later",
    503: "Service Unavailable, try later"
  };
  public isAdmin = computed(() => this.authService.isAdmin());
  public rolName = computed(() => this.authService.isAdmin() ? 'Admin' : 'User');
  public pageSize = computed(() => this.authService.isAdmin() ? 5 : 10);
  public pageNumber: number = 1;
  
  faCirclePlus = faCirclePlus;
  faCheck = faCheck;
  faXmark = faXmark;
  faPenToSquare = faPenToSquare;
  faTrash = faTrash;
  faFileExcel = faFileExcel;
  faFileCircleQuestion = faFileCircleQuestion;
  faRightFromBracket = faRightFromBracket;

  showInputCategory: boolean = false;
  showInputSubCategory: boolean = false;

  iTextCategory: string = '';
  iTextSubCategory: string = '';

  showBulkImport: boolean = false;
  serverStatus = signal<boolean>(true);

  getErrorMessages(code: number):string
  {
    return this.codeErrorDescription[code] ?? `Error ${code}`;
  }

  async onUpdateProduct(product: OBJ_Product) {
    const { value: formValues } = await Swal.fire({
      title: 'Update Product',
      width: '600px',
      padding: '1.5rem',
      html: `
      <div class="swal-form-container">
        <div class="input-group-swal">
            <label class="label-swal">Product Name</label>
            <input id="swal-name" class="form-field-swal" value="${product.name}">
        </div>
        
        <div class="swal-grid">
            <div class="input-group-swal">
                <label class="label-swal">Stock</label>
                <input type="number" id="swal-stock" class="form-field-swal" value="${product.stock}">
            </div>
            <div class="input-group-swal">
                <label class="label-swal">Price</label>
                <input type="number" id="swal-value" class="form-field-swal" value="${product.value}">
            </div>
        </div>

        <div class="input-group-swal">
            <label class="label-swal">Description</label>
            <textarea id="swal-desc" class="form-field-swal textarea-swal">${product.description}</textarea>
        </div>
      </div>
    `,
      focusConfirm: false,
      confirmButtonText: 'Update',
      confirmButtonColor: '#17adff',
      showCancelButton: true,
      preConfirm: () => {
        const name = (document.getElementById('swal-name') as HTMLInputElement).value;
        const stock = (document.getElementById('swal-stock') as HTMLInputElement).value;
        const value = (document.getElementById('swal-value') as HTMLInputElement).value;
        const description = (document.getElementById('swal-desc') as HTMLTextAreaElement).value;

        if (!name || stock === '' || value === '' || description == '') {
          Swal.showValidationMessage('Name, Stock, Price and Description are required');
          return false;
        }

        if (parseInt(stock) < 0) {
          Swal.showValidationMessage('Stock cannot be negative');
          return false;
        }

         if (parseInt(value) < 0) {
          Swal.showValidationMessage('Price cannot be negative');
          return false;
        }

        return {
          id: product.id,
          name: name,
          stock: parseInt(stock),
          value: parseFloat(value),
          description: description
        }
      }
    });

    if (formValues) {

      this.productService.updateProduct(formValues.id, formValues).subscribe({

        next: () =>
        {
            Toast.fire({
            icon: 'success', 
            title: 'Product Updated!',
            timer: 1000
            });

            this.loadProducts();
        },
        error: (err) =>
        {
            Swal.fire('Upss', 'Something went wrong. Server says: ' + (err.error?.message || 'Error', `${this.getErrorMessages(err.status)}`));
        }

      })
      
    }
  }

  deactivateProduct(id: string, name: string)
  {

    Swal.fire({
        title: 'Are you sure?',
        text: `You are about to delete "${name}". This action cannot be undone.`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!'
      }).then((result) => {
        if (result.isConfirmed) 
        {
          
          this.productService.deactivateProduct(id, name).subscribe({
          next: () =>
          {
            Toast.fire({
            icon: 'info', 
            title: 'Product Deleted!',
            timer: 1000
            });

            this.loadProducts();
          },
            error: (err) =>
            {
              Swal.fire('Upss', 'Something went wrong. Server says: ' + (err.error?.message || 'Error', `${this.getErrorMessages(err.status)}`));
            }
          })

        }
      });    
  }

  ngOnInit(): void {
    
    this.authService.checkStatus().subscribe();

    forkJoin({
      categories: this.productService.getCategories(),
      subCategories: this.productService.getSubCategories(),
      products: this.productService.getProducts()
    })
    .subscribe({
      next: (res) => 
      {
        this.categories = res.categories;
        this.subCategories = res.subCategories;
        this.products = res.products;

        console.log('Data loaded!')
        this.serverStatus.set(true);
        this.cdr.detectChanges();

      },
      error: (err) =>  
      {
        this.serverStatus.set(false);
        console.log('Error initial loading', err)
      }
    })

  }

  loadProducts()
  {
    this.productService.getProducts().subscribe({
      next: (data) =>
      {
        this.products = data;
        this.cdr.detectChanges();
      },
      error: (err) =>
      {
        Swal.fire('Upss', 'Error uploading products. Server says: ' + (err.error?.message || 'Error', `${this.getErrorMessages(err.status)}`));
      }
    })
  }

  onCategoryChange(categoryId: string)
  {
    this.newProduct.subCategoryId = '';

    this.subCategoriesFiltered = this.subCategories.filter(
      sub => sub.parentId === categoryId
    );
  }

  formatPrice(event: any)
  {
    // i for i-nput
    let iValue = event.target.value;

    let cleanValue = iValue.replace(/\D/g, '');

    event.target.value = cleanValue;

    const numericValue = cleanValue ? Number(cleanValue) : 0;

    this.productPrice = numericValue;
    this.newProduct.value = numericValue;
  }

  resetForm()
  {
    this.newProduct =
    {
      name: '',
      categoryId: '',
      subCategoryId: '',
      stock: 0,
      description: '',
      value: 0,
    }
    this.productPrice = null;
    this.subCategoriesFiltered = [];
  }

  onCreateProduct()
  {

    //this.loading = true; -- Spinner

    this.productService.createProduct(this.newProduct).subscribe({
      next: (response) =>
      {
        this.loadProducts();
        this.resetForm();

        Toast.fire({
          icon: 'success', 
          title: 'Product Added!'
        });
      },
      error: (err) =>
      {
        
        Swal.fire('Upss', 'Something went wrong. Server says: ' + (err.error?.message || 'Error', `${this.getErrorMessages(err.status)}`));

      }
    })

  }

  onCreateCategory()
  {
    if (this.iTextCategory.trim())
    {
      
      this.newIdentifier.set = 'CAT';
      this.newIdentifier.description = this.iTextCategory;
      this.newIdentifier.use = 'Categoria de los productos';

      this.productService.createIdentifier(this.newIdentifier).subscribe({
        next: (data: CDL_Identifier) =>
        {
            Toast.fire({
            icon: 'success', 
            title: 'Category Added!',
            timer: 1000
            });
            
            this.categories.push(data);

            this.categories.sort((a, z) => (a.description || '').localeCompare(z.description || ''));

            this.cancelCategory();
            this.cdr.detectChanges();
            

        },
        error: (err) =>
        {
            Swal.fire('Upss', 'Something went wrong. Server says: ' + (err.error?.message || 'Error', `${this.getErrorMessages(err.status)}`));
        }
      })
      
    }
  }

  cancelCategory()
  {
    this.showInputCategory = false;
    this.iTextCategory = '';

    this.newIdentifier.set = '';
    this.newIdentifier.description = '';
    this.newIdentifier.use = '';
    this.newIdentifier.parentId = null;

    document.getElementById('category')?.focus();
  }

  onCreateSubCategory()
  {
    if (this.iTextSubCategory.trim() && this.newProduct.categoryId)
    {
      this.newIdentifier.set = 'SAT';
      this.newIdentifier.description = this.iTextSubCategory;
      this.newIdentifier.use = 'Subcategoria de los productos';
      this.newIdentifier.parentId = this.newProduct.categoryId;

      this.productService.createIdentifier(this.newIdentifier).subscribe({
        next: (data: CDL_Identifier) =>
        {
            Toast.fire({
            icon: 'success', 
            title: 'Subcategory Added!',
            timer: 1000
            });
            
            this.subCategories.push(data);
            this.subCategoriesFiltered.push(data);

            this.subCategoriesFiltered.sort((a, z) => (a.description || '').localeCompare(z.description || ''));

            this.cancelSubCategory();
            this.cdr.detectChanges();
            

        },
        error: (err) =>
        {
            Swal.fire('Upss', 'Something went wrong. Server says: ' + (err.error?.message || 'Error', `${this.getErrorMessages(err.status)}`));
        }
      })
    }
    else
    {
      Swal.fire('Upss', 'Debes seleccionar una categoría primero', 'warning');
    }
  }

  cancelSubCategory()
  {
    this.showInputSubCategory = false;
    this.iTextSubCategory = '';

    this.newIdentifier.set = '';
    this.newIdentifier.description = '';
    this.newIdentifier.use = '';
    this.newIdentifier.parentId = null;

    document.getElementById('subcategory')?.focus();
  }

  onExportExcel()
  {

      if (this.products.length == 0)
      { 
          Swal.fire('Upss', 'There is no data here', 'error')
          return;
      }

      this.productService.exportExcel().subscribe({
        next: (blob: Blob) => 
        {
          const url = window.URL.createObjectURL(blob);
          const link = document.createElement('a');
          link.href = url;

          const hoy = new Date();
          const date = formatDate(hoy, 'yyyy-MM-dd_HH-mm-ss', 'en-US');
          link.download = `products_${date}.xlsx`;

          link.click();

          window.URL.revokeObjectURL(url);
        },
        error: (err) =>
        {
          Swal.fire('Upss', 'Error exporting Data. Server says: ' + (err.error?.message || 'Error', `${this.getErrorMessages(err.status)}`));
        }
      })
  }

  onBulkSuccess() 
  {
    this.showBulkImport = false;
    this.loadProducts();
  }

  downloadGuideExcel()
  {
    const link = document.createElement("a");
    link.href = "templates/GUIDE_IMPORT_PRODUCTS.xlsx";
    link.download;
    link.click();
  }

  offlineAlert()
  {
    Swal.fire('Offline', 'Turn on your server before, please', 'warning')
  }

  onLogOut()
  {
    this.authService.logOutUser().subscribe({
      next: () => 
      {
        this.router.navigate(['/login'], { replaceUrl: true });
      },
      error: (err) => 
      {}
    })  
  }

}
