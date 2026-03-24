import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { ProductList } from './features/products/product-list/product-list';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    {
        path: 'login',
        component: Login
    },
    {
        path: 'products',
        component: ProductList,
        canActivate: [authGuard]
    },
    {
        path: 'register',
        component: Register
    },
    {
        path: '',
        redirectTo: 'login',
        pathMatch: 'full'
    },
    {
        path: '**',
        redirectTo: 'login'
    }
];