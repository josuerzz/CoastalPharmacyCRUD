import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import Swal from 'sweetalert2';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      
      const isLoginRequest = req.url.includes('/auth/login');

      switch (error.status) 
      {
        case 0:
          Swal.fire('Offline', 'Server is not running. Check your connection.', 'warning');
          break;

        case 401:
          if (isLoginRequest) 
          {   
            Swal.fire('Login Failed', 'Invalid email or password', 'error');
          } else 
          {
            Swal.fire('Session expired', 'Please, Sig in again', 'info');
          }
          break;

        case 403:
          Swal.fire('Access Denied', 'You do not have permission to do this.', 'error');
          break;

        case 500:
          Swal.fire('Server Error', 'Something went wrong on our Server.', 'error');
          break;

        default:
          console.error('Unhandled error:', error);
          break;
      }

      return throwError(() => error);
    })
  );
};