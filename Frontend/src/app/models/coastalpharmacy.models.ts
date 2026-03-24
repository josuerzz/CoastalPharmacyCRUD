
export interface CDL_Identifier
{
    id: string,
    set: string, 
    elementNumber: number,
    code: string,
    description?: string,
    use: string,
    parentId?: string
}

export interface OBJ_Product
{
    id: string,
    name: string,
    categoryId: string,
    categoryName: string,
    subCategoryId?: string,
    subCategoryName: string,
    stock: number,
    description: string,
    value: number,
    status: number,
    createDate: string, // Datetime ISO format
    createUserId: string,
    createUser: SYS_User
}

export interface SYS_User
{
    Id: string,
    Email: string,
    Name: string,
    Surname: string,
    Status: number,
    RoleIdentifierId: string,
    RoleIdentifier?: CDL_Identifier,
    RefreshToken: string,
    TokenCreated: string, // Datetime ISO format
    TokenExpires: string // Datetime ISO format
}

// DTOs

export interface ProductCreateDto 
{
    name: string;
    categoryId: string;
    subCategoryId: string;
    stock: number;
    description: string;
    value: number;
}

export interface ProductUpdateDto 
{
    id: string;
    name: string;
    stock: number;
    value: number;
    description: string;
}

export interface IdentifierCreateDto 
{
    set: string;
    description: string;
    use: string;
    parentId?: string | null;
}

export interface BulkCreateProductDto 
{
    fileName: string,
    products: ProductCreateDto[];
}

// Auth

export interface UserCreateDto
{
    email: string;
    name: string;
    surname: string;
    password: string;
}

export interface LoginDto
{
    email: string;
    password: string;
}

export interface AuthCreateResponseDto
{
    message: string,
    email: string
}

export interface AuthResponseDto
{
    token: string
}