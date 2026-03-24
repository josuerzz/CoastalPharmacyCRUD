import { UserCreateDto, LoginDto, AuthCreateResponseDto, AuthResponseDto } from '../../models/coastalpharmacy.models';
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AuthService
{
    private http = inject(HttpClient);

    private apiUrl = 'http://localhost:5083/api';

    currentUserFullName = signal<string | null>(null);
    currentUserRole = signal<string | null>(null);

    registerUser(userRegister: UserCreateDto): Observable<AuthCreateResponseDto>
    {
      return this.http.post<AuthCreateResponseDto>(`${this.apiUrl}/auth/register`, userRegister);
    }

    loginUser(userLogin: LoginDto): Observable<AuthResponseDto>
    {
      return this.http.post<AuthResponseDto>(`${this.apiUrl}/auth/login`, userLogin, {
        withCredentials: true
      });
    }

    logOutUser()
    {
      return this.http.post(`${this.apiUrl}/auth/logout`, {})
    }

    checkStatus(): Observable<boolean> 
    {
      return this.http.get<any>(`${this.apiUrl}/auth/validate`).pipe(
        map((res) => 
        {
          this.currentUserFullName.set(res.fullName);
          this.currentUserRole.set(res.roleUser);
          return true;
        }),
        catchError(() => 
        {
          this.currentUserFullName.set(null);
          this.currentUserRole.set(null);
          return of(false);
        })
      );
    }

    isAdmin(): boolean
    {
      return this.currentUserRole() === 'admin';
    }
}
