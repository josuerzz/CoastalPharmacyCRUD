import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { errorInterceptor } from './core/error.interceptor';
import { withCredentialsInterceptor } from './core/credentials.interceptor';

export const appConfig: ApplicationConfig = 
{
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(
      withInterceptors([
        withCredentialsInterceptor,
        errorInterceptor])
    )
  ]
};
