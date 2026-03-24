import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

import { OBJ_Product, 
         CDL_Identifier, 
         ProductCreateDto, 
         IdentifierCreateDto, BulkCreateProductDto } from "../../models/coastalpharmacy.models";

@Injectable({
    providedIn: 'root'
})

export class ProductService
{
    private http = inject(HttpClient);

    private apiUrl = 'http://localhost:5083/api';

    getProducts(): Observable<OBJ_Product[]>
    {
        return this.http.get<OBJ_Product[]>(`${this.apiUrl}/products`);
    }

    getProductById(id: string): Observable<OBJ_Product>
    {
        return this.http.get<OBJ_Product>(`${this.apiUrl}/products/${id}`);
    }

    createProduct(product: ProductCreateDto): Observable<ProductCreateDto>
    {
        return this.http.post<ProductCreateDto>(`${this.apiUrl}/products`, product)
    }

    getCategories(): Observable<CDL_Identifier[]>
    {
        return this.http.get<CDL_Identifier[]>(`${this.apiUrl}/identifiers/categories`);
    }

    getSubCategories(): Observable<CDL_Identifier[]>
    {
        return this.http.get<CDL_Identifier[]>(`${this.apiUrl}/identifiers/subcategories`);
    }

    deactivateProduct(id: string, name: string): Observable<void>
    {
        return this.http.put<void>(`${this.apiUrl}/products/deactivate/${id}`, {});
    }

    updateProduct(id: string, productDto: any): Observable<void>
    {
        return this.http.put<void>(`${this.apiUrl}/products/updateProduct/${id}`, productDto);
    }

    createIdentifier(identifier: IdentifierCreateDto): Observable<CDL_Identifier>
    {
        return this.http.post<CDL_Identifier>(`${this.apiUrl}/identifiers`, identifier)
    }

    exportExcel(): Observable<Blob> 
    {
        return this.http.get(`${this.apiUrl}/products/export`, {
            responseType: 'blob' 
         });
    }

    bulkCreateProducts(data: BulkCreateProductDto): Observable<any> 
    {
    return this.http.post(`${this.apiUrl}/products/bulk-create`, data);
    }

}