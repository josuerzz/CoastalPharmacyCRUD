import { inject } from "@angular/core";
import { Router, CanActivateFn } from "@angular/router";
import { AuthService } from "../services/auth.service";
import { map, catchError, of } from "rxjs";

export const authGuard: CanActivateFn = () =>
{
    const authService = inject(AuthService);
    const router = inject(Router);

    return authService.checkStatus().pipe(
        map(isAuth => {
            
            if(isAuth) return true;

            return router.createUrlTree(['/login']);
        }),
        catchError(() => of(router.createUrlTree(['/login'])))
    );
};