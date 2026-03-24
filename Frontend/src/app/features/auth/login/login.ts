import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AuthResponseDto, LoginDto } from '../../../models/coastalpharmacy.models';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { AuthService } from '../../../core/services/auth.service';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { Footer } from '../../components/footer/footer';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, 
            CommonModule, 
            RouterLink,
            FontAwesomeModule,
            Footer],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login 
{
  loginForm: FormGroup;
  faEye = faEye;
  faEyeSlash = faEyeSlash;
  showPassword: boolean = false;
  
  private authService = inject(AuthService);
  private router = inject(Router);

    constructor(private fb: FormBuilder)
    { 
      this.loginForm = this.fb.group({
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(8)]],
      });
    }

    toggleShow()
    {
      this.showPassword = !this.showPassword;
    }
  
    onLogin()
    {
      if(this.loginForm.valid)
      {
        const payload: LoginDto = {...this.loginForm.value}; 
        
        this.authService.loginUser(payload).subscribe({
          next: () =>
          {
            this.router.navigate(['/products']);
          },
          error: (err: HttpErrorResponse) => 
          {
            console.log('Error: ', err.message);
          }
        })
      }
      else
      {
        this.loginForm.markAllAsTouched();
      }
    }
}
